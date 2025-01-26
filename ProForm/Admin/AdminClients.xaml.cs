using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProForm
{
    /// <summary>
    /// Логика взаимодействия для AdminUsers.xaml
    /// </summary>
    public partial class AdminClients : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public AdminClients()
        {
            InitializeComponent();
            LoadClientsData();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminMain mainAdmin = new AdminMain();
            mainAdmin.Show();
            Close();
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminSchedule mainSchedule = new AdminSchedule();
            mainSchedule.Show();
            Close();
        }

        private void PaymentsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPayments mainPayments = new AdminPayments();
            mainPayments.Show();
            Close();
        }

        private void LoadClientsData()
        {
            using (var context = new ProFormEntities())
            {
                var clients = context.Clients.ToList();
                ClientsTable.ItemsSource = clients;
            }
        }

        private void ClientsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsTable.SelectedItem is Clients selectedClient)
            {
                ClientSurname.Text = selectedClient.ClientSurname;
                ClientName.Text = selectedClient.ClientName;
                ClientMiddleName.Text = selectedClient.ClientMiddleName;
                ClientBirthday.Text = selectedClient.ClientBirthdayDate;
                ClientPhoneNumber.Text = selectedClient.ClientPhoneNumber;
                ClientEmail.Text = selectedClient.ClientEmail;
                ClientPurchaseDate.Text = selectedClient.ClientSubscriptionStartDate;
            }

        }
    }
}
