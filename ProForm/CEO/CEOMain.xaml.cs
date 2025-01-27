using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
    /// Логика взаимодействия для MainCEO.xaml
    /// </summary>
    public partial class CEOMain : Window
    {
        private ProFormEntities context = new ProFormEntities();
        public CEOMain()
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

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            Close();
        }

        private void SubscriptionTableBtn_Click(object sender, RoutedEventArgs e)
        {
            CEOSubscription ceoSubscription = new CEOSubscription();
            ceoSubscription.Show();
            Close();
        }

        private void PaymentsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentsTable.SelectedItem != null)
            {
                var selected = PaymentsTable.SelectedItem as Payments;

                PaymentsPaymentDate.Text = selected.PaymentDate.ToString();
                PaymentsPaymentAmount.Text = selected.PaymentAmount.ToString();
                PaymentsPaymentReceiptNumber.Text = selected.PaymentReceiptNumber.ToString();
                PaymentsPaymentMembershipEndDate.Text = selected.PaymentMembershipEndDate.ToString();

                PaymentsClientID.Text = selected.Clients.ClientPhoneNumber.ToString();
                PaymentsPaymentMethodID.Text = selected.PaymentMethods.PaymentMethodTitle.ToString();
                PaymentsPaymentMembershipID.Text = selected.Subscriptions.TypeSubscription.TypeTitle.ToString();
                PaymentsPaymentServiceID.Text = selected.AdditionalServices.AdditionalServiceTitle.ToString();
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

        private void SaveReportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"PaymentsReport_{DateTime.Now:yyyy-MM-dd}.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string fileName = saveFileDialog.FileName;

                    PdfDocument document = new PdfDocument();
                    document.Info.Title = "Отчёт о платежах";

                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    XFont titleFont = new XFont("Arial", 16);
                    XFont headerFont = new XFont("Arial", 12);
                    XFont contentFont = new XFont("Arial", 10);

                    gfx.DrawString("Отчёт о платежах", titleFont, XBrushes.Black,
                                   new XRect(0, 20, page.Width, 0), XStringFormats.TopCenter);

                    gfx.DrawString($"Дата создания: {DateTime.Now:dd.MM.yyyy}", contentFont, XBrushes.Black,
                                   new XRect(20, 50, page.Width - 40, 0), XStringFormats.TopLeft);

                    if (PaymentsTable.ItemsSource is IEnumerable<Payments> payments)
                    {
                        double startX = 20;
                        double startY = 80;
                        double rowHeight = 20;

                        gfx.DrawString("Клиент", headerFont, XBrushes.Black, startX, startY);
                        gfx.DrawString("Дата", headerFont, XBrushes.Black, startX + 120, startY);
                        gfx.DrawString("Сумма", headerFont, XBrushes.Black, startX + 220, startY);
                        gfx.DrawString("Метод оплаты", headerFont, XBrushes.Black, startX + 320, startY);
                        gfx.DrawString("Абонемент", headerFont, XBrushes.Black, startX + 420, startY);

                        startY += rowHeight;

                        foreach (var payment in payments)
                        {
                            gfx.DrawString(payment.Clients?.ClientSurname ?? "N/A", contentFont, XBrushes.Black, startX, startY);
                            gfx.DrawString(payment.PaymentDate ?? "N/A", contentFont, XBrushes.Black, startX + 120, startY);
                            gfx.DrawString(payment.PaymentAmount.ToString("C"), contentFont, XBrushes.Black, startX + 220, startY);
                            gfx.DrawString(payment.PaymentMethods?.PaymentMethodTitle ?? "N/A", contentFont, XBrushes.Black, startX + 320, startY);
                            gfx.DrawString(payment.Subscriptions?.TypeSubscription?.TypeTitle ?? "N/A", contentFont, XBrushes.Black, startX + 420, startY);

                            startY += rowHeight;

                            if (startY > page.Height - 50)
                            {
                                page = document.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                startY = 20;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данные для отчёта отсутствуют.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    document.Save(fileName);

                    MessageBox.Show($"Отчёт успешно сохранён в файл: {fileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveReportTrainerScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            // Загрузка данных из базы данных
            List<TrainerSchedules> trainerSchedules = context.TrainerSchedules.ToList();

            if (trainerSchedules == null || trainerSchedules.Count == 0)
            {
                MessageBox.Show("Нет данных для отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создаем новый PDF-документ
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Отчет по расписанию тренеров";

                // Добавляем страницу
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Шрифты
                XFont titleFont = new XFont("Arial", 14);
                XFont contentFont = new XFont("Arial", 10);

                // Начальная позиция для текста
                double startX = 50;
                double startY = 50;
                double lineHeight = 20;

                // Заголовок
                gfx.DrawString("Отчет по расписанию тренеров", titleFont, XBrushes.Black, startX, startY);
                startY += lineHeight * 2;

                // Шапка таблицы
                gfx.DrawString("Тренер", contentFont, XBrushes.Black, startX, startY);
                gfx.DrawString("Дата", contentFont, XBrushes.Black, startX + 200, startY);
                gfx.DrawString("Начало работы", contentFont, XBrushes.Black, startX + 300, startY);
                gfx.DrawString("Конец работы", contentFont, XBrushes.Black, startX + 400, startY);
                startY += lineHeight;

                // Данные таблицы
                foreach (var schedule in trainerSchedules)
                {
                    // Проверка на переполнение страницы
                    if (startY + lineHeight > page.Height - 50)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        startY = 50;
                    }

                    // Заполнение данных
                    gfx.DrawString(schedule.Trainers?.TrainerSurname ?? "N/A", contentFont, XBrushes.Black, startX, startY);
                    gfx.DrawString(schedule.WorkDate, contentFont, XBrushes.Black, startX + 200, startY);
                    gfx.DrawString(schedule.StartTime, contentFont, XBrushes.Black, startX + 300, startY);
                    gfx.DrawString(schedule.EndTime, contentFont, XBrushes.Black, startX + 400, startY);
                    startY += lineHeight;
                }

                // Сохранение документа
                string filePath = "C:\\Users\\Вячеслав\\OneDrive\\Desktop\\TrainerScheduleReport.pdf";
                document.Save(filePath);

                // Уведомление об успехе
                MessageBox.Show($"Отчет сохранен: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
