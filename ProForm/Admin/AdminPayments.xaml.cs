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
            PaymentsClientID.DisplayMemberPath = "ClientPhoneNumber"; 

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
                PaymentsClientID.Text = selectedPayment.Clients.ClientPhoneNumber.ToString();
                PaymentsPaymentDate.Text = selectedPayment.PaymentDate.ToString();
                PaymentsPaymentAmount.Text = selectedPayment.PaymentAmount.ToString();
                PaymentsPaymentMethodID.Text = selectedPayment.PaymentMethods.PaymentMethodTitle.ToString();
                PaymentsPaymentMembershipEndDate.Text = selectedPayment.PaymentMembershipEndDate.ToString();
                PaymentsPaymentReceiptNumber.Text = selectedPayment.PaymentReceiptNumber.ToString();
                PaymentsPaymentMembershipID.Text = selectedPayment.Subscriptions.TypeSubscription.TypeTitle.ToString();
                PaymentsPaymentServiceID.Text = selectedPayment.AdditionalServices.AdditionalServiceTitle.ToString();
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newPayment = new Payments
                {
                    PaymentClient_ID = (PaymentsClientID.SelectedItem as Clients)?.ID_Client ?? 0,
                    PaymentDate = PaymentsPaymentDate.Text,
                    PaymentAmount = decimal.Parse(PaymentsPaymentAmount.Text),
                    PaymentMethod_ID = (PaymentsPaymentMethodID.SelectedItem as PaymentMethods)?.ID_PaymentMethod ?? 0,
                    PaymentMembershipEndDate = PaymentsPaymentMembershipEndDate.Text,
                    PaymentReceiptNumber = PaymentsPaymentReceiptNumber.Text,
                    PaymentSubscription_ID = (PaymentsPaymentMembershipID.SelectedItem as TypeSubscription)?.ID_TypeSubscription ?? 0,
                    PaymentAdditionalService_ID = (PaymentsPaymentServiceID.SelectedItem as AdditionalServices)?.ID_AdditionalService
                };

                if (newPayment.PaymentClient_ID == 0 ||
                    string.IsNullOrEmpty(newPayment.PaymentDate) ||
                    newPayment.PaymentMethod_ID == 0 ||
                    newPayment.PaymentAmount <= 0)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.Payments.Add(newPayment);
                context.SaveChanges();

                PaymentsTable.ItemsSource = context.Payments.ToList();

                MessageBox.Show("Платеж успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPayment = PaymentsTable.SelectedItem as Payments;

                if (selectedPayment == null)
                {
                    MessageBox.Show("Пожалуйста, выберите платеж для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.Payments.Remove(selectedPayment);
                context.SaveChanges();

                PaymentsTable.ItemsSource = context.Payments.ToList();

                MessageBox.Show("Платеж успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPayment = PaymentsTable.SelectedItem as Payments;

                if (selectedPayment == null)
                {
                    MessageBox.Show("Пожалуйста, выберите платеж для изменения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedPayment.PaymentClient_ID = (PaymentsClientID.SelectedItem as Clients)?.ID_Client ?? selectedPayment.PaymentClient_ID;
                selectedPayment.PaymentDate = PaymentsPaymentDate.Text;
                selectedPayment.PaymentAmount = decimal.Parse(PaymentsPaymentAmount.Text);
                selectedPayment.PaymentMethod_ID = (PaymentsPaymentMethodID.SelectedItem as PaymentMethods)?.ID_PaymentMethod ?? selectedPayment.PaymentMethod_ID;
                selectedPayment.PaymentMembershipEndDate = PaymentsPaymentMembershipEndDate.Text;
                selectedPayment.PaymentReceiptNumber = PaymentsPaymentReceiptNumber.Text;
                selectedPayment.PaymentSubscription_ID = (PaymentsPaymentMembershipID.SelectedItem as TypeSubscription)?.ID_TypeSubscription ?? selectedPayment.PaymentSubscription_ID;
                selectedPayment.PaymentAdditionalService_ID = (PaymentsPaymentServiceID.SelectedItem as AdditionalServices)?.ID_AdditionalService ?? selectedPayment.PaymentAdditionalService_ID;

                if (selectedPayment.PaymentClient_ID == 0 ||
                    string.IsNullOrEmpty(selectedPayment.PaymentDate) ||
                    selectedPayment.PaymentMethod_ID == 0 ||
                    selectedPayment.PaymentAmount <= 0)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.SaveChanges();

                PaymentsTable.ItemsSource = context.Payments.ToList();

                MessageBox.Show("Платеж успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
