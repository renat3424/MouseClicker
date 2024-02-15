using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp7
{
    public class Db
    {
        MySqlConnection con = new MySqlConnection("server=localhost;port=3307;username=root;password=root;database=mouseclicker");
        public void OpenConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    con.Open();
                    Console.WriteLine("Successfully connected to database");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }


        }
        public void insertEvent(string mouseEvent, string date, double x, double y, string login) {
            try
            {   
                MySqlCommand command = new MySqlCommand("INSERT INTO events (e_date, e_event, x, y, login) VALUES (@time, @event, @X, @Y, @log)", con);

                
                command.Parameters.Add("@event", MySqlDbType.VarChar).Value = mouseEvent;
                
                command.Parameters.Add("@time", MySqlDbType.Timestamp).Value = date;
                command.Parameters.Add("@X", MySqlDbType.Double).Value = x;
                command.Parameters.Add("@Y", MySqlDbType.Double).Value = y;
                command.Parameters.Add("@log", MySqlDbType.VarChar).Value = login;
         


                command.ExecuteNonQuery();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


            }
        }

        public string GetPhone()
        {

            

            MySqlCommand command = con.CreateCommand();

            command.CommandText = "SELECT * from services WHERE s_name = 'phone'";
            
            
            MySqlDataReader reader = command.ExecuteReader();



            reader.Read();
            string phone = reader.GetString(2);
            reader.Close();
            return phone;

        }

        public string GetEmail()
        {



            MySqlCommand command = con.CreateCommand();

            command.CommandText = "SELECT * from services WHERE s_name = 'email'";


            MySqlDataReader reader = command.ExecuteReader();



            reader.Read();

            string email = reader.GetString(2);
            reader.Close();
            return email;

        }
        public void CloseConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }


        }


        public bool Login(string login, string password)
        {

            try
            {
                
                
                DataTable table = new DataTable();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                string query = "select * from profiles where login=@log and password=@pwd";
                MySqlCommand command = new MySqlCommand(query, con);
                command.Parameters.AddWithValue("@log", login);
                command.Parameters.AddWithValue("@pwd", password);


                adp.SelectCommand = command;
                adp.Fill(table);


                if (table.Rows.Count > 0)
                {
                    return true;


                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
            


        }

        public bool LoginAsAdmin(string login, string password)
        {

            try
            {


                DataTable table = new DataTable();
                MySqlDataAdapter adp = new MySqlDataAdapter();
                string query = "select * from admins where login=@log and password=@pwd";
                MySqlCommand command = new MySqlCommand(query, con);
                command.Parameters.AddWithValue("@log", login);
                command.Parameters.AddWithValue("@pwd", password);


                adp.SelectCommand = command;
                adp.Fill(table);


                if (table.Rows.Count > 0)
                {
                    return true;


                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }



        }

        public DataTable Data()
        {
            DataTable table = new DataTable();
            try
            {




                MySqlCommand command = new MySqlCommand("select * from events", con);
                


                MySqlDataReader reader = command.ExecuteReader();

                table.Load(reader);

                return table;



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return table;

            }


        }

        public DataTable Data(string login)
        {
            DataTable table = new DataTable();
            try
            {

               


                MySqlCommand command = new MySqlCommand("select e_date, e_event, x, y from events where login=@login", con);
                command.Parameters.AddWithValue("@login", login);

                
                MySqlDataReader reader = command.ExecuteReader();

                table.Load(reader);

                return table;



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return table;

            }


        }
       
    }
}
