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
    /// Логика взаимодействия для AdminSchedule.xaml
    /// </summary>
    public partial class AdminSchedule : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public AdminSchedule()
        {
            InitializeComponent();
            ScheduleTable.ItemsSource = context.TrainerSchedules.ToList();

            TrainerID.ItemsSource = context.Trainers.ToList();
            TrainerID.DisplayMemberPath = "TrainerSurname";
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminMain adminMain = new AdminMain();
            adminMain.Show();
            Close();
        }

        private void ClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminClients adminClients = new AdminClients();
            adminClients.Show();
            Close();
        }

        private void PaymentsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPayments adminPayments = new AdminPayments();
            adminPayments.Show();
            Close();
        }

        private void ScheduleTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleTable.SelectedItem is TrainerSchedules selectedTrainer)
            {
                TrainerID.Text = selectedTrainer.Trainers.TrainerSurname.ToString();
                TrainerWorkDate.Text = selectedTrainer.WorkDate.ToString();
                TrainerWorkStartTime.Text = selectedTrainer.StartTime.ToString();
                TrainerWorkEndTime.Text = selectedTrainer.EndTime.ToString();
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TrainerID.SelectedItem == null)
                {
                    MessageBox.Show("Выберите тренера из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TrainerWorkDate.Text) ||
                    string.IsNullOrWhiteSpace(TrainerWorkStartTime.Text) ||
                    string.IsNullOrWhiteSpace(TrainerWorkEndTime.Text))
                {
                    MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedTrainer = TrainerID.SelectedItem as Trainers;
                DateTime workDate;
                TimeSpan startTime, endTime;

                if (!DateTime.TryParse(TrainerWorkDate.Text, out workDate))
                {
                    MessageBox.Show("Введите корректную дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!TimeSpan.TryParse(TrainerWorkStartTime.Text, out startTime) ||
                    !TimeSpan.TryParse(TrainerWorkEndTime.Text, out endTime))
                {
                    MessageBox.Show("Введите корректное время.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (startTime >= endTime)
                {
                    MessageBox.Show("Время окончания должно быть позже времени начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newSchedule = new TrainerSchedules
                {
                    Trainer_ID = selectedTrainer.ID_Trainer,
                    WorkDate = workDate.ToString("dd.MM.yyyy"), 
                    StartTime = startTime.ToString(@"hh\:mm"),  
                    EndTime = endTime.ToString(@"hh\:mm")      
                };

                context.TrainerSchedules.Add(newSchedule);
                context.SaveChanges();

                ScheduleTable.ItemsSource = context.TrainerSchedules.ToList();

                MessageBox.Show("График успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                TrainerWorkDate.Clear();
                TrainerWorkStartTime.Clear();
                TrainerWorkEndTime.Clear();
                TrainerID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScheduleTable.SelectedItem == null)
                {
                    MessageBox.Show("Выберите график для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedSchedule = ScheduleTable.SelectedItem as TrainerSchedules;

                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить выбранный график?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    context.TrainerSchedules.Remove(selectedSchedule);
                    context.SaveChanges();

                    ScheduleTable.ItemsSource = context.TrainerSchedules.ToList();

                    MessageBox.Show("График успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScheduleTable.SelectedItem == null)
                {
                    MessageBox.Show("Выберите график для изменения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var selectedSchedule = ScheduleTable.SelectedItem as TrainerSchedules;

                if (TrainerID.SelectedItem == null || string.IsNullOrWhiteSpace(TrainerWorkDate.Text) ||
                    string.IsNullOrWhiteSpace(TrainerWorkStartTime.Text) || string.IsNullOrWhiteSpace(TrainerWorkEndTime.Text))
                {
                    MessageBox.Show("Заполните все поля перед изменением.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedSchedule.Trainer_ID = (TrainerID.SelectedItem as Trainers).ID_Trainer;
                selectedSchedule.WorkDate = TrainerWorkDate.Text;
                selectedSchedule.StartTime = TrainerWorkStartTime.Text;
                selectedSchedule.EndTime = TrainerWorkEndTime.Text;

                context.SaveChanges();

                ScheduleTable.ItemsSource = context.TrainerSchedules.ToList();

                MessageBox.Show("График успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при изменении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
