using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    interface IUserInteractor
    {
        List<Student> ReadStudents(List<Group> groups);
        List<Score> ReadScores(List<Student> students, List<Subject> subjects);
        List<Score> ReadScoresToChange(List<Score> scores, List<Student> students);
        string ReadCommant();
    }
}
