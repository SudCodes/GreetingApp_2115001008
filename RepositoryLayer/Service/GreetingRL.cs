using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private string GreetingMSG = "Hello World";

        private string GetMessage()
        {
            return GreetingMSG;
        }
        public string GetGreetingRL()
        {
            return GetMessage();
        }
    }
}
