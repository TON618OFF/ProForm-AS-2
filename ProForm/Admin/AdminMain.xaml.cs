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
            SubscriptionAvailableServices.ItemsSource = context.AdditionalServices.ToList();
            SubscriptionAvailableServices.DisplayMemberPath = "AdditionalServiceTitle";

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

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SubscriptionTypeID.SelectedItem is TypeSubscription selectedType)
                {
                    int typeId = selectedType.ID_TypeSubscription;

                    if (SubscriptionAvailableServices.SelectedItem is AdditionalServices selectedService)
                    {
                        int servicesId = selectedService.ID_AdditionalService;

                        decimal price = decimal.TryParse(SubscriptionPaymentAmount.Text, out decimal tempPrice) ? tempPrice : 0;
                        int? discount = int.TryParse(SubscriptionPaymentDiscount.Text, out int tempDiscount) ? (int?)tempDiscount : null;
                        string period = SubscriptionActualDate.Text;
                        int statusId = (SubscriptionStatusID.SelectedItem as SubscriptionStatuses)?.ID_SubscriptionStatus ?? 0;

                        if (price <= 0)
                        {
                            MessageBox.Show("Поле 'Стоимость Абонемента' не заполнено или неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(period))
                        {
                            MessageBox.Show("Поле 'Период Действия Абонемента' не заполнено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (statusId == 0)
                        {
                            MessageBox.Show("Поле 'Статус Абонемента' не выбрано!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        var newSubscription = new Subscriptions
                        {
                            TypeSubscription_ID = typeId,
                            SubscriptionPrice = price,
                            SubscriptionAvailableServices_ID = servicesId,
                            Discount = discount,
                            SubscriptionPeriodExpired = period,
                            SubscriptionStatus_ID = statusId
                        };

                        context.Subscriptions.Add(newSubscription);
                        context.SaveChanges();

                        SubscriptionTable.ItemsSource = context.Subscriptions.ToList();

                        MessageBox.Show("Абонемент успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Поле 'Доступные Услуги' не выбрано или неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Поле 'Тип Абонемента' не выбрано!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении абонемента: {ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SubscriptionTable.SelectedItem is Subscriptions selectedSubscription)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный абонемент?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        context.Subscriptions.Remove(selectedSubscription);

                        context.SaveChanges();

                        SubscriptionTable.ItemsSource = context.Subscriptions.ToList();

                        MessageBox.Show("Абонемент успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении абонемента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите абонемент для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SubscriptionTable.SelectedItem is Subscriptions selectedSubscription)
            {
                try
                {
                    if (SubscriptionTypeID.SelectedItem is TypeSubscription selectedType)
                        selectedSubscription.TypeSubscription_ID = selectedType.ID_TypeSubscription;
                    else
                        throw new Exception("Поле 'Тип Абонемента' не выбрано!");

                    if (SubscriptionAvailableServices.SelectedItem is AdditionalServices selectedService)
                        selectedSubscription.SubscriptionAvailableServices_ID = selectedService.ID_AdditionalService;
                    else
                        throw new Exception("Поле 'Доступные Услуги' не выбрано!");

                    selectedSubscription.SubscriptionPrice = decimal.TryParse(SubscriptionPaymentAmount.Text, out decimal price) ? price : throw new Exception("Поле 'Стоимость Абонемента' неверно заполнено!");
                    selectedSubscription.Discount = int.TryParse(SubscriptionPaymentDiscount.Text, out int discount) ? (int?)discount : null;
                    selectedSubscription.SubscriptionPeriodExpired = string.IsNullOrWhiteSpace(SubscriptionActualDate.Text) ? throw new Exception("Поле 'Период Действия Абонемента' не заполнено!") : SubscriptionActualDate.Text;

                    if (SubscriptionStatusID.SelectedItem is SubscriptionStatuses selectedStatus)
                        selectedSubscription.SubscriptionStatus_ID = selectedStatus.ID_SubscriptionStatus;
                    else
                        throw new Exception("Поле 'Статус Абонемента' не выбрано!");

                    context.SaveChanges();

                    SubscriptionTable.ItemsSource = context.Subscriptions.ToList();

                    MessageBox.Show("Абонемент успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении абонемента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите абонемент для изменения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
