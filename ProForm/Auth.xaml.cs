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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using static ProForm.Trainers;

namespace ProForm
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void AuthExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AuthEnterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = AuthLogin.Text;
            string password = AuthPassword.Password;

            using (var context = new ProFormEntities())
            {
                var user = context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);

                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {user.UserLogin}! Ваша роль: {user.Roles.RoleTitle}",
                                    "Успешная авторизация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    if (user.Roles.RoleTitle == "Руководитель")
                    {
                        CEOMain ceoMain = new CEOMain();
                        ceoMain.Show();
                        Close();
                    }
                    else if (user.Roles.RoleTitle == "Администратор")
                    {
                        AdminMain adminMain = new AdminMain();
                        adminMain.Show();
                        Close();
                    }
                    else if (user.Roles.RoleTitle == "Тренер")
                    {
                        TrainerMain trainerMain = new TrainerMain();
                        trainerMain.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Для вашей роли не предусмотрено окно.",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!",
                                    "Ошибка авторизации",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        private void AuthLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AuthEnterBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void AuthPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AuthEnterBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
