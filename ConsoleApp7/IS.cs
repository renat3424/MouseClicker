using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    [ServiceContract]
    interface IS
    {
        [OperationContract]
        void MouseEvent(int mouseEvent, double X, double Y, string date, string login);

        [OperationContract]
        bool Login(string login, string password);

        [OperationContract]
        bool LoginAsAdmin(string login, string password);

        [OperationContract]
        DataTable Data(string login);

        [OperationContract]
        DataTable AdminData();
        [OperationContract]
        string GetEmail();

        [OperationContract]
        string GetPhone();

        [OperationContract]
        void StartRecord(string login);

        [OperationContract]
        void StopRecord(string login);

        [OperationContract]
        void LogOut(string login);
    }
    
}
