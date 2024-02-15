using FluentEmail.Core;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IS proxy;
        bool Listen = false;
        string login = "";
        int numberofnotes=0;
        DataTable tab;
        string email="";
        string phone="";
        bool admin = false;
        public MainWindow()
        {


            InitializeComponent();
            
            
        }
     
        Point prevPoint;
        private void Window_MouseButtonAction(object sender, MouseButtonEventArgs e)
        {
            if (Listen) { 
                Point point = PointToScreen(Mouse.GetPosition(this));
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    proxy.MouseEvent(1, point.X, point.Y, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), login);
                    numberofnotes++;
                    number.Text = numberofnotes.ToString();
                }
                else if(e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
                {
                    proxy.MouseEvent(2, point.X, point.Y, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), login);
                    numberofnotes++;
                    number.Text = numberofnotes.ToString();

                }
                else
                {
                    proxy.MouseEvent(3, point.X, point.Y, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), login);
                    numberofnotes++;
                    number.Text = numberofnotes.ToString();
                }
            }
            
        }



        private void ListenButton_Click(object sender, RoutedEventArgs e)
        {

            if (!Listen)
            {

                Listen = true;
                ListenButton.Content = "Stop Listen";
                proxy.StartRecord(login);
            }
            else
            {
                Listen = false;
                ListenButton.Content = "Start Listen";
                prevPoint = PointToScreen(Mouse.GetPosition(this));
                proxy.StopRecord(login);

            }
        }

 
          private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (Listen)
            {
                Point point = PointToScreen(Mouse.GetPosition(this));
                if (Math.Abs(point.X - prevPoint.X) >= 10 || Math.Abs(point.Y - prevPoint.Y) >= 10)
                {

                    prevPoint = point;
                    proxy.MouseEvent(4, point.X, point.Y, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), login);
                    numberofnotes++;
                    number.Text = numberofnotes.ToString();
                }
            }

        }

        private void funk(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("net.tcp://localhost:6565/S");

            var binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IS>(binding);
            var endpoint = new EndpointAddress(uri);
            proxy = channel.CreateChannel(endpoint);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (admin)
            {
                proxy.LogOut(login);
                UpdateButton.IsEnabled = false;
                login = "";
                DG.ItemsSource = null;
                LoginButton.Content = "Login";
                user.Content = "Loged out of account";
                eventFilter.IsEnabled = false;
                dateFilter.IsEnabled = false;
                timeFilter.IsEnabled = false;
                phone = "";
                email = "";
                admin = false;
                return;

            }
            if (ListenButton.IsEnabled)
            {
                proxy.LogOut(login);
                ListenButton.IsEnabled = false;
                UpdateButton.IsEnabled = false;
                login = "";
                DG.ItemsSource = null;
                LoginButton.Content = "Login";
                numberofnotes = 0;
                number.Text = numberofnotes.ToString();
                user.Content = "Loged out of account";
                eventFilter.IsEnabled = false;
                dateFilter.IsEnabled = false;
                timeFilter.IsEnabled = false;
                phone = "";
                email = "";

            }
            else
            {
                if (proxy.LoginAsAdmin(logText.Text, pwdText.Password))
                {
                    
                    UpdateButton.IsEnabled = true;
                    login = logText.Text;
                    tab = proxy.AdminData();
                    DG.ItemsSource = tab.DefaultView;
                    LoginButton.Content = "Logout";
                    eventFilter.IsEnabled = true;
                    dateFilter.IsEnabled = true;
                    timeFilter.IsEnabled = true;

                    user.Content = "Loged in as admin!";
                    admin = true;
                    return;
                }

                if (proxy.Login(logText.Text, pwdText.Password))
                {

                    ListenButton.IsEnabled = true;
                    UpdateButton.IsEnabled = true;
                    login = logText.Text;
                    tab= proxy.Data(login);
                    DG.ItemsSource = tab.DefaultView;
                    LoginButton.Content = "Logout";
                    eventFilter.IsEnabled = true;
                    dateFilter.IsEnabled = true;
                    timeFilter.IsEnabled = true;
                    phone = proxy.GetPhone();
                    email = proxy.GetEmail();
                    numberofnotes = DG.Items.Count;
                    number.Text = numberofnotes.ToString();
                    
                    user.Content = "Success!";
                   
                }
                else
                {
                    user.Content = "No such user";
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (admin)
            {
                tab = proxy.AdminData();
                DG.ItemsSource = tab.DefaultView;
            }
            else
            {
                tab = proxy.Data(login);
                DG.ItemsSource = tab.DefaultView;

            }
        }

        private void number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(numberofnotes%50==0 && numberofnotes != 0)
            {
                var sen = new SmtpSender(() => new SmtpClient("localhost")
                {


                    EnableSsl = false,
                    DeliveryMethod=SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation=@"C:\Mail"


                });

                Email.DefaultSender = sen;
                string message = "User " + login + " has made " + numberofnotes + " notes";
                var eM = Email.From("mouseclicker@gmail.com").To(email).Subject(message).Send();
                sendWhatsApp(phone, message);

            }
        }

        private void sendWhatsApp(string number, string message)

        {

            try{ 


                System.Diagnostics.Process.Start("http://api.whatsapp.com/send?phone=" + number + "&text=" + message);

            }

            catch (Exception ex)

            {

            }

        }

        public void Filter()
        {
            
            string eventQuery = (eventFilter.Text == "None") ? "True " : $"e_event='{eventFilter.Text}'";

            DateTime nextday;
            if (dateFilter.Text != "")
            {
                if (timeFilter.Value == null)
                {
                    nextday = ((DateTime)dateFilter.SelectedDate).AddDays(1);
                    eventQuery += $"and e_date>='{dateFilter.Text}' and e_date<'{nextday}'";
                }
                else
                {
                    nextday = ((DateTime)dateFilter.SelectedDate).Date.Add(((DateTime)timeFilter.Value).TimeOfDay);
                    eventQuery += $"and e_date>='{nextday}' and e_date<'{nextday.AddSeconds(60)}'";
                }
            }
            
            DataView dataview = new DataView(tab);

            dataview.RowFilter = eventQuery;
            
            DG.ItemsSource = dataview;

           
           


        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            
                Filter();
            
        }
        
        private void TextBox_TextChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void timeFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Filter();
        }
    }
}
