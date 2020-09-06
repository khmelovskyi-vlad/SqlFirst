using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class CommandMaster
    {
        public CommandMaster(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        private IUserInteractor userInteractor;

        public void Run()
        {
            var inf = userInteractor.ReadCommant();
        }
    }
}
