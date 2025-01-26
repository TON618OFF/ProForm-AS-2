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
    /// Логика взаимодействия для TrainerAttendance.xaml
    /// </summary>
    public partial class TrainerAttendance : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public TrainerAttendance()
        {
            InitializeComponent();
            TrainerAttendanceTable.ItemsSource = context.GroupTraining.ToList();
        }

        private void ScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            TrainerMain schedule = new TrainerMain();
            schedule.Show();
            Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
