using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    interface IUserInteractor
    {
        List<Student> ReadStudents(List<Group> groups);
        List<Score> ReadScores(List<Student> students, List<Subject> subjects);
        List<Score> ReadScoresToChange(List<Score> scores);
        ManipulationDataMode ReadMode();
        DataType SelectDataType();

        //void ShowStudents(List<Student> students);
        //void ShowScores(List<Score> scores);
        //void ShowGroups(List<Group> groups);
        //void ShowCourses(List<Course> courses);
        //void ShowSubjects(List<Subject> subjects);
        //void ShowSpecialties(List<Specialty> specialties);
        void ShowData<TEntity>(IQueryable<TEntity> specialties);

        //void ShowStudentScoresCount(List<StudentScoresCount> studentScoresCounts);
        SqlParameter ReadMaxFoursCount();
        void WriteLine(string line);
        SqlParameter[] ReadParametersForString();
        string CreateSql();
        void WriteRowsNumberAffected(int rowsNumberAffected);
        void ShowNumberCoursesScores(List<NumberCourseScores> numberCoursesScores);
    }
}
