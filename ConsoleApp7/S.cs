using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class S : IS
    {
        Db database;
        public S(Db database)
        {
            this.database = database;
        }



        public string GetEmail() {


            return database.GetEmail();
        
         }

        
        public string GetPhone()
        {

            return database.GetPhone();
        }
        public void MouseEvent(int mouseEvent, double X, double Y, string date, string login)
        {

            switch (mouseEvent)
            {
                case 1:
                    
                    database.insertEvent("MouseLeftClick", date, X, Y, login);
                    break;
                case 2:
                   
                    database.insertEvent("MouseRightClick", date, X, Y, login);
                    break;
                case 3:
                    
                    database.insertEvent("MouseWheelClick", date, X, Y, login);
                    break;
                case 4:
                    
                    database.insertEvent("MouseMove", date, X, Y, login);
                    break;

            }

        }


        public bool Login(string login, string password)
        {

            Console.WriteLine("User "+login+" is trying to authorize");
            bool log = database.Login(login, password);
            if (log)
            {
                Console.WriteLine("User " + login + " is successfuly authorized");
            }
            return log;
        }


        public DataTable Data(string login)
        {

            return database.Data(login);
        }

        public bool LoginAsAdmin(string login, string password)
        {
            bool log = database.LoginAsAdmin(login, password);
            if (log)
            {
                Console.WriteLine("User " + login + " is authorized as admin");
            }
            return log;
        }

        public DataTable AdminData()
        {
            return database.Data();
        }

        public void StartRecord(string login)
        {
            Console.WriteLine("User " + login + " has started recording");
        }

        public void StopRecord(string login)
        {
            Console.WriteLine("User " + login + " has stopped recording");
        }

        public void LogOut(string login)
        {
            Console.WriteLine("User " + login + " is out");
        }
    }
}
