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
    /// Логика взаимодействия для AdminPayments.xaml
    /// </summary>
    public partial class AdminPayments : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public AdminPayments()
        {
            InitializeComponent();
            PaymentsTable.ItemsSource = context.Payments.ToList();
            PaymentsClientID.ItemsSource = context.Clients.ToList();
            PaymentsClientID.DisplayMemberPath = "ClientFullName"; 

            PaymentsPaymentMethodID.ItemsSource = context.PaymentMethods.ToList();
            PaymentsPaymentMethodID.DisplayMemberPath = "PaymentMethodTitle"; 

            PaymentsPaymentMembershipID.ItemsSource = context.TypeSubscription.ToList();
            PaymentsPaymentMembershipID.DisplayMemberPath = "TypeTitle"; 

            PaymentsPaymentServiceID.ItemsSource = context.AdditionalServices.ToList();
            PaymentsPaymentServiceID.DisplayMemberPath = "AdditionalServiceTitle";

        }

        private void SubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminMain adminMain = new AdminMain();
            adminMain.Show();
            Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminClients adminClients = new AdminClients();
            adminClients.Show();
            Close();
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminSchedule adminSchedule = new AdminSchedule();
            adminSchedule.Show();
            Close();
        }

        private void PaymentsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentsTable.SelectedItem is Payments selectedPayment)
            {
                PaymentsClientID.Text = selectedPayment.Clients.ClientSurname.ToString();
                PaymentsPaymentDate.Text = selectedPayment.PaymentDate.ToString();
                PaymentsPaymentAmount.Text = selectedPayment.PaymentAmount.ToString();
                PaymentsPaymentMethodID.Text = selectedPayment.PaymentMethods.PaymentMethodTitle.ToString();
                PaymentsPaymentMembershipEndDate.Text = selectedPayment.PaymentMembershipEndDate.ToString();
                PaymentsPaymentReceiptNumber.Text = selectedPayment.PaymentReceiptNumber.ToString();
                PaymentsPaymentMembershipID.Text = selectedPayment.Subscriptions.TypeSubscription.TypeTitle.ToString();
                PaymentsPaymentServiceID.Text = selectedPayment.AdditionalServices.AdditionalServiceTitle.ToString();
            }
        }
    }
}
