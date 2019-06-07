using Studify.DB;
using Studify.Model;
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

namespace Studify.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainPageView : Window
    {
        User User = User.CurrentUser;
        Student stud = new Student();
        EFMessageRepository eFMessage = new EFMessageRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public MainPageView()
        {
            InitializeComponent();

            stud = eFStudent.GetStudentById((int)User.idStudent);
            
            if (stud.isHeadman == true)
            {
                Group_list.Visibility = Visibility.Visible;
            }
            else Group_list.Visibility = Visibility.Hidden;

            DataContext = this;
            MainPage.Content = new StartPage();

            Count_Message.Badge = eFMessage.CountMessages();
            Choose_Theme_Unchecked(this, new RoutedEventArgs());
            AddRemindersToAutorun();
        }

        private void AddRemindersToAutorun()
        {
            // открываем нужную ветку в реестре   
            // @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"  

            Microsoft.Win32.RegistryKey Key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

            string path = System.IO.Path.GetFullPath(@"StudifyReminder\StudifyReminder.exe");
            //добавляем первый параметр - название ключа  
            // Второй параметр - это путь к   
            // исполняемому файлу программы.  
            Key.SetValue("NtOrg", "\"" + path + "\"");
            Key.Close();
        }

        private void MaxsizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.MaxHeight = SystemParameters.WorkArea.Height;
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void Roll_Up_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Task_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new TaskListView();
        }

        private void Group_list_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new StudentsListView();
        }

        private void TimeTable_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new TimeTableView();
        }

        private void Message_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new MessageView();
            Count_Message.Badge = eFMessage.CountMessages();
        }

        private void Choose_Theme_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Resources = new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,/Themes/Dark.xaml")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }

        private void Choose_Theme_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Resources = new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,/Themes/Light.xaml")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }

        private void Progress_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new ProgressView();
        }

        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            AuthorizationView authorization = new AuthorizationView();
            authorization.Show();
        }
    }
}
