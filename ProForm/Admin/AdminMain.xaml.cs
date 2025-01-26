using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для MainAdmin.xaml
    /// </summary>
    public partial class AdminMain : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public AdminMain()
        {
            InitializeComponent();
            SubscriptionTable.ItemsSource = context.Subscriptions.ToList();

            SubscriptionTypeID.ItemsSource = context.TypeSubscription.ToList();
            SubscriptionTypeID.DisplayMemberPath = "TypeTitle";
            SubscriptionStatusID.ItemsSource = context.SubscriptionStatuses.ToList();
            SubscriptionStatusID.DisplayMemberPath = "SubscriptionStatuseType";
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminClients users = new AdminClients();
            users.Show();
            Close();
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminSchedule adminSchedule = new AdminSchedule();
            adminSchedule.Show();
            Close();
        }

        private void PaymentsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPayments adminPayments = new AdminPayments();
            adminPayments.Show();
            Close();
        }

        private void SubscriptionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubscriptionTable.SelectedItem != null)
            {
                var subs = SubscriptionTable.SelectedItem as Subscriptions;

                SubscriptionTypeID.Text = subs.TypeSubscription.TypeTitle.ToString();
                SubscriptionPaymentAmount.Text = subs.SubscriptionPrice.ToString();
                SubscriptionAvailableServices.Text = subs.AdditionalServices.AdditionalServiceTitle.ToString();
                SubscriptionPaymentDiscount.Text = subs.Discount.ToString();
                SubscriptionActualDate.Text = subs.SubscriptionPeriodExpired.ToString();
                SubscriptionStatusID.Text = subs.SubscriptionStatuses.SubscriptionStatuseType.ToString();
            }
        }
    }
}
