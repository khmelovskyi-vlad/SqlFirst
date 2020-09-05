using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class DataSetRepository
    {
        public Dictionary<string, DataSetPrototype> Repository = new Dictionary<string, DataSetPrototype>();
        public DataSetRepository()
        {
            FillRepository();
        }
        private void FillRepository()
        {
            DataSet dataSet = new DataSet();
            var student = dataSet.Tables.Add("Student");
            student.PrimaryKey = new DataColumn[] { student.Columns.Add("Id", typeof(Guid)) };
            student.Columns.Add("FirstName", typeof(string));
            student.Columns.Add("LastName", typeof(string));
            student.Columns.Add("GroupId", typeof(Guid));
            student.Columns.Add("AverageScore", typeof(int));

            var group = dataSet.Tables.Add("Group");
            group.PrimaryKey = new DataColumn[] { group.Columns.Add("Id", typeof(Guid)) };
            group.Columns.Add("Name", typeof(string));
            group.Columns.Add("AverageScore", typeof(int));
            group.Columns.Add("CourseId", typeof(Guid));
            group.Columns.Add("SpecialtyId", typeof(Guid));

            var course = dataSet.Tables.Add("Course");
            course.PrimaryKey = new DataColumn[] { course.Columns.Add("Id", typeof(Guid)) };
            course.Columns.Add("Name", typeof(int));

            var specialty = dataSet.Tables.Add("Specialty");
            specialty.PrimaryKey = new DataColumn[] { specialty.Columns.Add("Id", typeof(Guid)) };
            specialty.Columns.Add("Name", typeof(string));

            var subject = dataSet.Tables.Add("Subject");
            subject.PrimaryKey = new DataColumn[] { subject.Columns.Add("Id", typeof(Guid)) };
            subject.Columns.Add("Name", typeof(string));

            var score = dataSet.Tables.Add("Score");
            score.PrimaryKey = new DataColumn[] { score.Columns.Add("Id", typeof(Guid)) };
            score.Columns.Add("Value", typeof(int));
            score.Columns.Add("SubjectId", typeof(Guid));
            score.Columns.Add("StudentId", typeof(Guid));
            score.Columns.Add("CourseId", typeof(Guid));

            var subjectCourse = dataSet.Tables.Add("SubjectCourse");
            subjectCourse.PrimaryKey = new DataColumn[] { subjectCourse.Columns.Add("SubjectId", typeof(Guid)), subjectCourse.Columns.Add("CourseId", typeof(Guid))};

            var subjectSpecialty = dataSet.Tables.Add("SubjectSpecialty");
            subjectSpecialty.PrimaryKey = new DataColumn[] { subjectSpecialty.Columns.Add("SubjectId", typeof(Guid)), subjectSpecialty.Columns.Add("SpecialtyId", typeof(Guid)) };

            dataSet.Relations.Add("GroupId-StudentId", dataSet.Tables["Group"].Columns["Id"], dataSet.Tables["Student"].Columns["GroupId"]);
            dataSet.Relations.Add("CourseId-GroupId", dataSet.Tables["Course"].Columns["Id"], dataSet.Tables["Group"].Columns["CourseId"]);
            dataSet.Relations.Add("SpecialtyId-GroupSpecialtyId", dataSet.Tables["Specialty"].Columns["Id"], dataSet.Tables["Group"].Columns["SpecialtyId"]);
            dataSet.Relations.Add("SubjectId-ScoreSubjectId", dataSet.Tables["Subject"].Columns["Id"], dataSet.Tables["Score"].Columns["SubjectId"]);
            dataSet.Relations.Add("StudentId-ScoreStudentId", dataSet.Tables["Student"].Columns["Id"], dataSet.Tables["Score"].Columns["StudentId"]);
            dataSet.Relations.Add("CourseId-ScoreCourseId", dataSet.Tables["Course"].Columns["Id"], dataSet.Tables["Score"].Columns["CourseId"]);
            FillMyDataSet(dataSet, 1);

            Repository.Add("first course", new DataSetPrototype(dataSet, "If you want to take a data set with information for first course, write 'first course'"));

            DataSet dataSet2 = new DataSet();
            var student2 = dataSet2.Tables.Add("Student");
            student2.PrimaryKey = new DataColumn[] { student2.Columns.Add("Id", typeof(Guid)) };
            student2.Columns.Add("FirstName", typeof(string));
            student2.Columns.Add("LastName", typeof(string));
            student2.Columns.Add("GroupId", typeof(Guid));
            student2.Columns.Add("AverageScore", typeof(int));

            var group2 = dataSet2.Tables.Add("Group");
            group2.PrimaryKey = new DataColumn[] { group2.Columns.Add("Id", typeof(Guid)) };
            group2.Columns.Add("Name", typeof(string));
            group2.Columns.Add("AverageScore", typeof(int));
            group2.Columns.Add("CourseId", typeof(Guid));
            group2.Columns.Add("SpecialtyId", typeof(Guid));

            var course2 = dataSet2.Tables.Add("Course");
            course2.PrimaryKey = new DataColumn[] { course2.Columns.Add("Id", typeof(Guid)) };
            course2.Columns.Add("Name", typeof(int));

            var specialty2 = dataSet2.Tables.Add("Specialty");
            specialty2.PrimaryKey = new DataColumn[] { specialty2.Columns.Add("Id", typeof(Guid)) };
            specialty2.Columns.Add("Name", typeof(string));

            var subject2 = dataSet2.Tables.Add("Subject");
            subject2.PrimaryKey = new DataColumn[] { subject2.Columns.Add("Id", typeof(Guid)) };
            subject2.Columns.Add("Name", typeof(string));

            var score2 = dataSet2.Tables.Add("Score");
            score2.PrimaryKey = new DataColumn[] { score2.Columns.Add("Id", typeof(Guid)) };
            score2.Columns.Add("Value", typeof(int));
            score2.Columns.Add("SubjectId", typeof(Guid));
            score2.Columns.Add("StudentId", typeof(Guid));
            score2.Columns.Add("CourseId", typeof(Guid));

            var subjectCourse2 = dataSet2.Tables.Add("SubjectCourse");
            subjectCourse2.PrimaryKey = new DataColumn[] { subjectCourse2.Columns.Add("SubjectId", typeof(Guid)), subjectCourse2.Columns.Add("CourseId", typeof(Guid)) };

            var subjectSpecialty2 = dataSet2.Tables.Add("SubjectSpecialty");
            subjectSpecialty2.PrimaryKey = new DataColumn[] { subjectSpecialty2.Columns.Add("SubjectId", typeof(Guid)), subjectSpecialty2.Columns.Add("SpecialtyId", typeof(Guid)) };

            dataSet2.Relations.Add("GroupId-StudentId", dataSet2.Tables["Group"].Columns["Id"], dataSet2.Tables["Student"].Columns["GroupId"]);
            dataSet2.Relations.Add("CourseId-GroupId", dataSet2.Tables["Course"].Columns["Id"], dataSet2.Tables["Group"].Columns["CourseId"]);
            dataSet2.Relations.Add("SpecialtyId-GroupSpecialtyId", dataSet2.Tables["Specialty"].Columns["Id"], dataSet2.Tables["Group"].Columns["SpecialtyId"]);
            dataSet2.Relations.Add("SubjectId-ScoreSubjectId", dataSet2.Tables["Subject"].Columns["Id"], dataSet2.Tables["Score"].Columns["SubjectId"]);
            dataSet2.Relations.Add("StudentId-ScoreStudentId", dataSet2.Tables["Student"].Columns["Id"], dataSet2.Tables["Score"].Columns["StudentId"]);
            dataSet2.Relations.Add("CourseId-ScoreCourseId", dataSet2.Tables["Course"].Columns["Id"], dataSet2.Tables["Score"].Columns["CourseId"]);
            FillMyDataSet(dataSet2, 2);

            Repository.Add("second course", new DataSetPrototype(dataSet2, "If you want to take a data set with information for second course, write 'second course'"));
        }
        private void FillMyDataSet(DataSet dataSet, int course)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
            SqlCommander sqlCommander = new SqlCommander();
            sqlCommander.Run(sqlConnectionStringBuilder,
                (sqlConnection) =>
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

                    //sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
                    //    "FROM [dbo].[Course] ", sqlConnection);
                    //sqlDataAdapter.Fill(dataSet.Tables["Course"]);


                    /*using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {

                        sqlDataAdapter.SelectCommand = new SqlCommand("SELECT TOP 1 * " +
                            "FROM [dbo].[Course] c " +
                            "WHERE c.[Name] = 1", sqlConnection);
                        sqlDataAdapter.SelectCommand.Transaction = transaction;
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlDataAdapter.InsertCommand = commandBuilder.GetInsertCommand();

                        sqlDataAdapter.Fill(dataSet.Tables["Course"]);

                        dataSet.Tables["Course"].Rows.Add(Guid.NewGuid(), 10);
                        sqlDataAdapter.InsertCommand.Transaction = transaction;

                        var s = sqlDataAdapter.Update(dataSet.Tables["Course"]);
                        commandBuilder = new SqlCommandBuilder(sqlDataAdapter);

                        sqlDataAdapter.SelectCommand = new SqlCommand("SELECT TOP 1 * " +
                            "FROM [dbo].[Specialty] sp ", sqlConnection);
                        sqlDataAdapter.SelectCommand.Transaction = transaction;
                        sqlDataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                        sqlDataAdapter.Fill(dataSet.Tables["Specialty"]);

                        dataSet.Tables["Specialty"].Rows.Add(Guid.NewGuid(), "100000");

                        var ss = sqlDataAdapter.Update(dataSet.Tables["Specialty"]);
                        transaction.Commit();
                    }*/

                    //sqlDataAdapter.InsertCommand = new SqlCommand("INSERT INTO [dbo].[Course] " +
                    //    "(Id, Name) " +
                    //    "VALUES " +
                    //    "(@par1, @par2), " +
                    //    "(NEWID(), 12)", sqlConnection);
                    //sqlDataAdapter.InsertCommand.Parameters.AddWithValue("@par1", Guid.NewGuid());
                    //sqlDataAdapter.InsertCommand.Parameters.AddWithValue("@par2", 11);

                    ////sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                    //dataSet.Tables["Course"].Rows[1].ItemArray = new object[] { 105, Guid.NewGuid() };

                    ////dataSet.Tables["Course"].Rows[2].Delete();

                    ////dataSet.Tables["Course"].Rows.Add(10, Guid.NewGuid());

                    ////commandBuilder.GetInsertCommand();
                    //var s = sqlDataAdapter.Update(dataSet.Tables["Course"]);
                    //foreach (DataTable table in dataSet.Tables)
                    //{
                    //    foreach (DataRow row in table.Rows)
                    //    {
                    //        foreach (var item in row.ItemArray)
                    //        {
                    //            Console.Write($"{item,20} |");
                    //        }
                    //        Console.WriteLine();
                    //    }
                    //}


                        sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
                        "FROM [dbo].[Course] " +
                        "WHERE [Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Course"]);


                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sp.Id, sp.[Name] " +
                        "FROM [dbo].[Specialty] sp " +
                        "JOIN [dbo].[Group] g ON g.SpecialtyId = sp.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Specialty"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT g.[Id], g.[Name], g.[AverageScore], g.[CourseId], g.[SpecialtyId] " +
                        "FROM [dbo].[Group] g " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Group"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sub.Id, sub.[Name] " +
                        "FROM [dbo].[Subject] sub " +
                        "JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = subCo.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Subject"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT st.Id, st.FirstName, st.LastName, st.GroupId, st.AverageScore " +
                        "FROM [dbo].[Student] st " +
                        "JOIN [dbo].[Group] g ON g.Id = st.GroupId " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Student"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sc.Id, sc.[Value], sc.[SubjectId], sc.[StudentId], sc.[CourseId] " +
                        "FROM [dbo].[Score] sc " +
                        "JOIN [dbo].[Course] c ON c.Id = sc.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["Score"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sc.SubjectId, sc.CourseId " +
                        "FROM [dbo].[SubjectCourse] sc " +
                        "JOIN [dbo].[Course] c ON c.Id = sc.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["SubjectCourse"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT subSpec.SubjectId, subSpec.SpecialtyId " +
                        "FROM [dbo].[SubjectSpecialty] subSpec " +
                        "JOIN [dbo].[Specialty] sp ON sp.Id = subSpec.SpecialtyId " +
                        "JOIN [dbo].[Group] g ON g.SpecialtyId = sp.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = @course", sqlConnection);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@course", course));
                    sqlDataAdapter.Fill(dataSet.Tables["SubjectSpecialty"]);
                    dataSet.AcceptChanges();
                    //var resRows = from sub in dataSet.Tables["Subject"].AsEnumerable()
                    //          join subCo in dataSet.Tables["SubjectCourse"].AsEnumerable() on Guid.Parse(sub["Id"].ToString()) equals Guid.Parse(subCo["SubjectId"].ToString())
                    //          join subSpec in dataSet.Tables["SubjectSpecialty"].AsEnumerable() on Guid.Parse(sub["Id"].ToString()) equals Guid.Parse(subSpec["SubjectId"].ToString())
                    //          select new
                    //          {
                    //              subId = sub["Id"],
                    //              specId = subSpec["SpecialtyId"],
                    //              coId = subCo["CourseId"]
                    //          };
                    //foreach (var resRow in resRows)
                    //{
                    //    Console.Write(resRow.subId);
                    //    Console.Write(resRow.specId);
                    //    Console.Write(resRow.coId);
                    //    Console.WriteLine();
                    //}
                    //Console.ReadKey();
                });
            //dataSet.Tables["Course"].Rows.Add(10, Guid.NewGuid());
            //sqlCommander.Run(sqlConnectionStringBuilder,
            //    (sqlConnection) =>
            //    {
            //        sqlConnection.Open();
            //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    
            //        sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
            //            "FROM [dbo].[Course] ", sqlConnection);

            //        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
            //        dataSet.Tables["Course"].Rows.Add(12, Guid.NewGuid());
            //        dataSet.AcceptChanges();

            //        var s = sqlDataAdapter.Update(dataSet.Tables["Course"]);
            //        foreach (DataTable table in dataSet.Tables)
            //        {
            //            foreach (DataRow row in table.Rows)
            //            {
            //                foreach (var item in row.ItemArray)
            //                {
            //                    Console.Write($"{item,20} |");
            //                }
            //                Console.WriteLine();
            //            }
            //        }
            //    });
        }
        private void AddCourse(SqlDataAdapter sqlDataAdapter, SqlConnection sqlConnection, DataSet dataSet, int course)
        {
            sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
                "FROM [dbo].[Course] " +
                "WHERE [Name] = sqlParameter", sqlConnection);
            SqlParameter sqlParameter = new SqlParameter("@course", course);
            sqlDataAdapter.Fill(dataSet.Tables["Course"]);
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
