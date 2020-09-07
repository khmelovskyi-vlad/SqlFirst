using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class CommandMaster
    {
        public CommandMaster(IUserInteractor userInteractor, Initializer initializer)
        {
            this.userInteractor = userInteractor;
            this.initializer = initializer;
        }
        private IUserInteractor userInteractor;
        private Initializer initializer;
        public async Task Run()
        {
            while (true)
            {
                var mode = userInteractor.ReadMode();
                switch (mode)
                {
                    case Mode.Initialize:
                        await initializer.FirstInitializeData();
                        break;
                    case Mode.AddStudent:
                        await initializer.AddStudents();
                        break;
                    case Mode.AddScore:
                        await initializer.AddScore();
                        break;
                    case Mode.Procedure:
                        break;
                    case Mode.ShowCleverStudent:
                        break;
                    case Mode.ShowStudentScoresCount:
                        break;
                    case Mode.ShowData:
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task FirstInitialize()
        {
           await initializer.FirstInitializeData();
        }
        private async Task ShowData()
        {
            var dataType = userInteractor.SelectDataType();
            switch (dataType)
            {
                case DataType.Student:
                    userInteractor.ShowStudents(await initializer.GetStudents());
                    break;
                case DataType.Score:
                    userInteractor.ShowScores(await initializer.GetScores());
                    break;
                case DataType.Group:
                    userInteractor.ShowGroups(await initializer.GetGroups());
                    break;
                case DataType.Course:
                    userInteractor.ShowCourses(await initializer.GetCourses());
                    break;
                case DataType.Subject:
                    userInteractor.ShowSubjects(await initializer.GetSubjects());
                    break;
                case DataType.Specialty:
                    userInteractor.ShowSpecialties(await initializer.GetSpecialties());
                    break;
                default:
                    break;
            }
        }
    }
}
