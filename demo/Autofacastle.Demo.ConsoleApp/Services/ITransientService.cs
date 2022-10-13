using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    public interface ITransientService
    {
        public void BusinessOne();

        public Task<bool> BusinessTwo();

        public Task BusinessThree();
    }
}
