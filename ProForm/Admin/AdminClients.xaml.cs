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
            ClientsTable.ItemsSource = context.Clients.ToList();
            ClientSubscriptionType.ItemsSource = context.TypeSubscription.ToList();
            ClientSubscriptionType.DisplayMemberPath = "TypeTitle";
            ClientSubscriptionType.SelectedValuePath = "ID_TypeSubscription";

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
                ClientSubscriptionType.SelectedValue = selectedClient.ClientSubscriptionType_ID;
                ClientPurchaseDate.Text = selectedClient.ClientSubscriptionStartDate;
                ClientExpiredDate.Text = selectedClient.ClientSubscriptionEndDate;
            }

        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newClient = new Clients
                {
                    ClientSurname = ClientSurname.Text,
                    ClientName = ClientName.Text,
                    ClientMiddleName = ClientMiddleName.Text,
                    ClientBirthdayDate = ClientBirthday.Text,
                    ClientPhoneNumber = ClientPhoneNumber.Text,
                    ClientEmail = ClientEmail.Text,
                    ClientSubscriptionType_ID = ClientSubscriptionType.SelectedIndex,
                    ClientSubscriptionStartDate = ClientPurchaseDate.Text,
                    ClientSubscriptionEndDate = ClientExpiredDate.Text
                };

                if (string.IsNullOrWhiteSpace(newClient.ClientSurname) ||
                    string.IsNullOrWhiteSpace(newClient.ClientName) ||
                    string.IsNullOrWhiteSpace(newClient.ClientBirthdayDate) ||
                    string.IsNullOrWhiteSpace(newClient.ClientPhoneNumber) ||
                    string.IsNullOrWhiteSpace(newClient.ClientEmail) ||
                    newClient.ClientSubscriptionType_ID <= 0)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.Clients.Add(newClient);
                context.SaveChanges();

                ClientsTable.ItemsSource = context.Clients.ToList();

                MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ClearInputFields()
        {
            ClientSurname.Text = string.Empty;
            ClientName.Text = string.Empty;
            ClientMiddleName.Text = string.Empty;
            ClientBirthday.Text = null;
            ClientPhoneNumber.Text = string.Empty;
            ClientEmail.Text = string.Empty;
            ClientPurchaseDate.Text = null;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsTable.SelectedItem is Clients selectedClient)
                {
                    selectedClient.ClientSurname = ClientSurname.Text;
                    selectedClient.ClientName = ClientName.Text;
                    selectedClient.ClientMiddleName = ClientMiddleName.Text;
                    selectedClient.ClientBirthdayDate = ClientBirthday.Text;
                    selectedClient.ClientPhoneNumber = ClientPhoneNumber.Text;
                    selectedClient.ClientEmail = ClientEmail.Text;
                    selectedClient.ClientSubscriptionType_ID = (int)ClientSubscriptionType.SelectedValue; 
                    selectedClient.ClientSubscriptionStartDate = ClientPurchaseDate.Text;
                    selectedClient.ClientSubscriptionEndDate = ClientExpiredDate.Text;

                    if (string.IsNullOrWhiteSpace(selectedClient.ClientSurname) ||
                        string.IsNullOrWhiteSpace(selectedClient.ClientName) ||
                        string.IsNullOrWhiteSpace(selectedClient.ClientBirthdayDate) ||
                        string.IsNullOrWhiteSpace(selectedClient.ClientPhoneNumber) ||
                        string.IsNullOrWhiteSpace(selectedClient.ClientEmail) ||
                        selectedClient.ClientSubscriptionType_ID <= 0)
                    {
                        MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    context.SaveChanges();

                    ClientsTable.ItemsSource = context.Clients.ToList();

                    MessageBox.Show("Данные клиента успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Выберите клиента для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsTable.SelectedItem is Clients selectedClient)
                {
                    var result = MessageBox.Show(
                        "Вы уверены, что хотите удалить этого клиента?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        context.Clients.Remove(selectedClient);
                        context.SaveChanges();

                        ClientsTable.ItemsSource = context.Clients.ToList();

                        MessageBox.Show("Клиент успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        ClearInputFields();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
