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
    /// Логика взаимодействия для MainTrainer.xaml
    /// </summary>
    public partial class TrainerMain : Window
    {
        private int _trainerID;

        private ProFormEntities context = new ProFormEntities();
        public TrainerMain()
        {
            InitializeComponent();
            TrainerScheduleTable.ItemsSource = context.TrainerSchedules.ToList();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AttendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            TrainerAttendance trainerAttendance = new TrainerAttendance();
            trainerAttendance.Show();
            Close();
        }
    }
}
