using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrettyHair;
using System.Data.SqlClient;

namespace PrettyHairUI
{
    class Program
    {

        Controller ctrl = new Controller();
        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.Run();
        }

        public void Run()
        {
            ctrl.InitializeRepositories();
            ctrl.CheckOrdersForToday();
            ctrl.Menu();

        }
    }
}
