using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class DataSetMaster
    {
        public DataSetMaster(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        private IUserInteractor userInteractor;
        public void Run()
        {
            DataSet dataSet = new DataSet();
            DataSetRepository dataSetRepository = new DataSetRepository();
            if (userInteractor.ReadCreationMode() == CreationMode.Predefined)
            {
                var dataSets = userInteractor.SelectDataSets(dataSetRepository);
                dataSet = CreateDataSet(dataSets);
                if (userInteractor.CheckNeedAddData("If you want to add some students to the data set, click 'Enter'"))
                {
                    var students = AddSomeRandomStudents(dataSet);
                    if (userInteractor.CheckNeedAddData("If you want to add some score to added students to the data set, click 'Enter'"))
                    {
                        AddSomeRandomStudentsScores(dataSet, students);
                        if (userInteractor.CheckNeedAddData("If you want to add some score to added students to the data set, click 'Enter'"))
                        {
                            AddStudentsScoresToDataBase(dataSet);
                        }
                        else
                        {
                            dataSet.Tables["Score"].AcceptChanges();
                            dataSet.Tables["Student"].AcceptChanges();
                        }
                    }
                }
                //foreach (var row in dataSet.Tables["Student"].Select())
                //{
                //    foreach (var item in row.ItemArray)
                //    {
                //        Console.Write($"{item , 20} |");
                //    }
                //    Console.WriteLine();
                //}
            }
            else
            {
                var tables = userInteractor.CreateDataSetTables();
                dataSet.Tables.AddRange(tables);
            }
        }
        private DataSet CreateDataSet(DataSet[] dataSets)
        {
            for (int i = 1; i < dataSets.Length; i++)
            {
                dataSets[0].Merge(dataSets[i]);
            }
            return dataSets[0];
        }
        private void AddStudentsScoresToDataBase(DataSet dataSet)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
            SqlCommander sqlCommander = new SqlCommander();
            sqlCommander.Run(sqlConnectionStringBuilder,
                (sqlConnection) =>
                {
                    sqlConnection.Open();
                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT TOP 1 * " +
                            "FROM [dbo].[Student]", sqlConnection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlDataAdapter.SelectCommand.Transaction = transaction;

                        sqlDataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        sqlDataAdapter.InsertCommand.Transaction = transaction;

                        var count = sqlDataAdapter.Update(dataSet.Tables["Student"]);
                        Console.WriteLine(count);


                        commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlDataAdapter.SelectCommand = new SqlCommand("SELECT TOP 1 * " +
                            "FROM [dbo].[Score]", sqlConnection);
                        sqlDataAdapter.SelectCommand.Transaction = transaction;

                        sqlDataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        sqlDataAdapter.InsertCommand.Transaction = transaction;

                        var sour = dataSet.Tables["Score"].GetChanges();
                        count = sqlDataAdapter.Update(dataSet.Tables["Score"]);
                        Console.WriteLine(count);

                        transaction.Commit();
                    }
                });
        }

        private void AddSomeRandomStudentsScores(DataSet dataSet, List<DataRow> newStudents)
        {
            Random random = new Random();
            var count = 0;
            foreach (DataRow newStudent in newStudents)
            {
                var group = GetGroup(dataSet, newStudent);
                var subjectIds = GetSubjectIds(dataSet, group);
                subjectIds = GetRandomSubjects(subjectIds, random);
                count = count + subjectIds.Count();
                FillScore(dataSet, random, subjectIds, group, newStudent);
            }
            Console.WriteLine(count);
        }
        private DataRow GetGroup(DataSet dataSet, DataRow newStudent)
        {
            return newStudent.GetParentRow("GroupId-StudentId");
            //var s = dataSet.Tables["Student"].Rows.ChildRelations;
            //return (from st in dataSet.Tables["Student"].AsEnumerable()
            //            join g in dataSet.Tables["Group"].AsEnumerable() on Guid.Parse(st["GroupId"].ToString()) equals Guid.Parse(g["Id"].ToString())
            //            where Guid.Parse(st["Id"].ToString()) == Guid.Parse(newStudent["Id"].ToString())
            //            select new
            //            {
            //                Id = Guid.Parse(g["Id"].ToString()),
            //                CourseId = Guid.Parse(g["CourseId"].ToString()),
            //                SpecialtyId = Guid.Parse(g["SpecialtyId"].ToString())
            //            }).Select(met => (met.Id, met.CourseId, met.SpecialtyId)).ToList();
        }
        private List<DataRow> AddSomeRandomStudents(DataSet dataSet)
        {
            Random random = new Random();
            var buffer = "qwertyuiopasdfghjklzxcvbnm";
            List<DataRow> newStudents = new List<DataRow>();
            for (int i = 0; i < 20; i++)
            {
                var group = GetRandomGroup(dataSet, random);
                var newStudent = dataSet.Tables["Student"].Rows.Add(new object[] { Guid.NewGuid(), GetRandomString(20, buffer, random),
                    GetRandomString(20, buffer, random), Guid.Parse(group["Id"].ToString()), null });
                newStudents.Add(newStudent);
                //var subjectIds = GetSubjectIds(dataSet, group);
                //subjectIds = GetRandomSubjects(subjectIds, random);
                //FillScore(dataSet, random, subjectIds, group, newStudent);
            }
            return newStudents;
        }
        private void FillScore(DataSet dataSet, Random random, List<Guid> subjectIds, DataRow group, DataRow newStudent)
        {
            foreach (var subjectId in subjectIds)
            {
                var newScore = dataSet.Tables["Score"].Rows.Add(new object[] { Guid.NewGuid(), random.Next(1, 6),
                    subjectId, Guid.Parse(newStudent["Id"].ToString()), Guid.Parse(group["CourseId"].ToString())});
                //foreach (var item in newScore.ItemArray)
                //{
                //    Console.Write($"{item,-40}");
                //}
                //Console.WriteLine();
            }
        }
        private List<Guid> GetSubjectIds(DataSet dataSet, DataRow group)
        {
            return (from sub in dataSet.Tables["Subject"].AsEnumerable()
                          join subCo in dataSet.Tables["SubjectCourse"].AsEnumerable()
                          on new { item1 = Guid.Parse(sub["Id"].ToString()), item2 = Guid.Parse(@group["CourseId"].ToString()) }
                          equals new { item1 = Guid.Parse(subCo["SubjectId"].ToString()), item2 = Guid.Parse(subCo["CourseId"].ToString()) }
                          join subSpec in dataSet.Tables["SubjectSpecialty"].AsEnumerable()
                          on new { item1 = Guid.Parse(sub["Id"].ToString()), item2 = Guid.Parse(@group["SpecialtyId"].ToString()) }
                          equals new { item1 = Guid.Parse(subSpec["SubjectId"].ToString()), item2 = Guid.Parse(subSpec["SpecialtyId"].ToString()) }
                          select new
                          {
                              subjectId = Guid.Parse(sub["Id"].ToString()),
                          }).Select(met => met.subjectId).ToList();
        }
        private DataRow GetRandomGroup(DataSet dataSet, Random random)
        {
            return dataSet.Tables["Group"].Rows[random.Next(0, dataSet.Tables["Group"].Rows.Count)];
        }
        private List<T> GetRandomSubjects<T>(List<T> subIds, Random random)
        {
            var deleteCount = random.Next(0, subIds.Count());
            for (int i = 0; i < deleteCount; i++)
            {
                subIds.Remove(subIds[random.Next(0, subIds.Count())]);
            }
            return subIds;
        }
        private string GetRandomString(int maxLength, string buffer, Random random)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < maxLength; i++)
            {
                stringBuilder.Append(buffer[random.Next(0, buffer.Length)]);
            }
            return stringBuilder.ToString();
        }
        private static SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = "WIN-DHV0BQSLTCR";
            sqlConnectionStringBuilder.UserID = "SQLFirst";
            sqlConnectionStringBuilder.Password = "Test1234";
            sqlConnectionStringBuilder.InitialCatalog = "Vlad";
            return sqlConnectionStringBuilder;
        }
    }
}
