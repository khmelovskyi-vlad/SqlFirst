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
            student.Columns.Add("FirstName", typeof(string));
            student.Columns.Add("LastName", typeof(string));
            student.Columns.Add("GroupId", typeof(Guid));
            student.Columns.Add("AverageScore", typeof(int));
            student.PrimaryKey = new DataColumn[] { student.Columns.Add("Id", typeof(Guid)) };

            var group = dataSet.Tables.Add("Group");
            group.Columns.Add("Name", typeof(string));
            group.Columns.Add("AverageScore", typeof(int));
            group.Columns.Add("CourseId", typeof(Guid));
            group.Columns.Add("SpecialtyId", typeof(Guid));
            group.PrimaryKey = new DataColumn[] { group.Columns.Add("Id", typeof(Guid)) };

            var course = dataSet.Tables.Add("Course");
            course.Columns.Add("Name", typeof(int));
            course.PrimaryKey = new DataColumn[] { course.Columns.Add("Id", typeof(Guid)) };

            var specialty = dataSet.Tables.Add("Specialty");
            specialty.Columns.Add("Name", typeof(string));
            specialty.PrimaryKey = new DataColumn[] { specialty.Columns.Add("Id", typeof(Guid)) };

            var subject = dataSet.Tables.Add("Subject");
            subject.Columns.Add("Name", typeof(string));
            subject.PrimaryKey = new DataColumn[] { subject.Columns.Add("Id", typeof(Guid)) };

            var score = dataSet.Tables.Add("Score");
            score.Columns.Add("Value", typeof(int));
            score.Columns.Add("SubjectId", typeof(Guid));
            score.Columns.Add("StudentId", typeof(Guid));
            score.Columns.Add("CourseId", typeof(Guid));
            score.PrimaryKey = new DataColumn[] { score.Columns.Add("Id", typeof(Guid)) };

            var subjectCourse = dataSet.Tables.Add("SubjectCourse");
            subjectCourse.PrimaryKey = new DataColumn[] { subjectCourse.Columns.Add("SubjectId", typeof(Guid)), subjectCourse.Columns.Add("CourseId", typeof(Guid))};

            var subjectSpecialty = dataSet.Tables.Add("SubjectSpecialty");
            subjectSpecialty.PrimaryKey = new DataColumn[] { subjectSpecialty.Columns.Add("SubjectId", typeof(Guid)), subjectSpecialty.Columns.Add("SpecialtyId", typeof(Guid)) };

            dataSet.Relations.Add(dataSet.Tables["Group"].Columns["Id"], dataSet.Tables["Student"].Columns["GroupId"]);
            dataSet.Relations.Add(dataSet.Tables["Course"].Columns["Id"], dataSet.Tables["Group"].Columns["CourseId"]);
            dataSet.Relations.Add(dataSet.Tables["Specialty"].Columns["Id"], dataSet.Tables["Group"].Columns["SpecialtyId"]);
            dataSet.Relations.Add(dataSet.Tables["Subject"].Columns["Id"], dataSet.Tables["Score"].Columns["SubjectId"]);
            dataSet.Relations.Add(dataSet.Tables["Student"].Columns["Id"], dataSet.Tables["Score"].Columns["StudentId"]);
            dataSet.Relations.Add(dataSet.Tables["Course"].Columns["Id"], dataSet.Tables["Score"].Columns["CourseId"]);
            FillMyDataSet(dataSet);

            Repository.Add("first course", new DataSetPrototype(dataSet, "If you want to take a data set with information for first course, write 'first course'"));
        }
        private void FillMyDataSet(DataSet dataSet)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
            SqlCommander sqlCommander = new SqlCommander();
            sqlCommander.Run(sqlConnectionStringBuilder,
                (sqlConnection) =>
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

                    /*sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
                        "FROM [dbo].[Course] ", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Course"]);

                    //sqlDataAdapter.InsertCommand = new SqlCommand("INSERT INTO [dbo].[Course] " +
                    //    "(Id, Name) " +
                    //    "VALUES " +
                    //    "(@par1, @par2), " +
                    //    "(NEWID(), 12)", sqlConnection);
                    //sqlDataAdapter.InsertCommand.Parameters.AddWithValue("@par1", Guid.NewGuid());
                    //sqlDataAdapter.InsertCommand.Parameters.AddWithValue("@par2", 11);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                    //sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                    dataSet.Tables["Course"].Rows[2].Delete();
                    //dataSet.Tables["Course"].Rows.Remove(l);
                    //dataSet.Tables["Course"].Rows.Add(10, Guid.NewGuid());
                    //dataSet.Tables["Course"].Rows.Add(11, Guid.NewGuid());
                    //dataSet.Tables["Course"].Rows.Add(12, Guid.NewGuid());
                    var s = sqlDataAdapter.Update(dataSet.Tables["Course"]);
                    foreach (DataTable table in dataSet.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (var item in row.ItemArray)
                            {
                                Console.Write($"{item,20} |");
                            }
                            Console.WriteLine();
                        }
                    }*/

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT * " +
                        "FROM [dbo].[Course] " +
                        "WHERE [Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Course"]);


                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sp.Id, sp.[Name] " +
                        "FROM [dbo].[Specialty] sp " +
                        "JOIN [dbo].[Group] g ON g.SpecialtyId = sp.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Specialty"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT g.[Id], g.[Name], g.[AverageScore], g.[CourseId], g.[SpecialtyId] " +
                        "FROM [dbo].[Group] g " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Group"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sub.Id, sub.[Name] " +
                        "FROM [dbo].[Subject] sub " +
                        "JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = subCo.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Subject"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT st.Id, st.FirstName, st.LastName, st.GroupId, st.AverageScore " +
                        "FROM [dbo].[Student] st " +
                        "JOIN [dbo].[Group] g ON g.Id = st.GroupId " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Student"]);
                    
                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sc.Id, sc.[Value], sc.[SubjectId], sc.[StudentId], sc.[CourseId] " +
                        "FROM [dbo].[Score] sc " +
                        "JOIN [dbo].[Course] c ON c.Id = sc.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["Score"]);

                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT sc.SubjectId, sc.CourseId " +
                        "FROM [dbo].[SubjectCourse] sc " +
                        "JOIN [dbo].[Course] c ON c.Id = sc.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["SubjectCourse"]);
                    
                    sqlDataAdapter.SelectCommand = new SqlCommand("SELECT subSpec.SubjectId, subSpec.SpecialtyId " +
                        "FROM [dbo].[SubjectSpecialty] subSpec " +
                        "JOIN [dbo].[Specialty] sp ON sp.Id = subSpec.SpecialtyId " +
                        "JOIN [dbo].[Group] g ON g.SpecialtyId = sp.Id " +
                        "JOIN [dbo].[Course] c ON c.Id = g.CourseId " +
                        "WHERE c.[Name] = 1", sqlConnection);
                    sqlDataAdapter.Fill(dataSet.Tables["SubjectSpecialty"]);
                });
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
