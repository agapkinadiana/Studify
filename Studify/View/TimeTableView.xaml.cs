using Studify.DB;
using Studify.Model;
using Studify.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для TimeTableView.xaml
    /// </summary>
    public partial class TimeTableView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        TimeTableViewModel timeTableViewModel;
        EFStudentRepository eFStudent = new EFStudentRepository();

        public TimeTableView()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            InitializeComponent();
            timeTableViewModel = new TimeTableViewModel();            
            LoadTimeTableIfEmpty();
            LoadGroupsId();
        }


        private void LoadGroupsId()
        {
            if (stud.IsHeadman)
            {
                //IdGroup.Visibility = Visibility.Visible;
                //IdGroupL.Visibility = Visibility.Visible;
                Choose_course.Visibility = Visibility.Visible;
                Choose_group.Visibility = Visibility.Visible;
                Choose_subgroup.Visibility = Visibility.Visible;
            }
            else
            {
                //IdGroup.Visibility = Visibility.Hidden;
                //IdGroupL.Visibility = Visibility.Hidden;
                Choose_course.Visibility = Visibility.Hidden;
                Choose_group.Visibility = Visibility.Hidden;
                Choose_subgroup.Visibility = Visibility.Hidden;
            }
            //IdGroup.ItemsSource = timeTableViewModel.GetIdGroups();
            //IdGroup.SelectedValue = stud.idGroup;
            Choose_course.ItemsSource = timeTableViewModel.GetCourse();
            Choose_course.SelectedValue = stud.Group.Course;
            Choose_group.ItemsSource = timeTableViewModel.GetGroups();
            Choose_group.SelectedValue = stud.Group.GroupNumber;
            Choose_subgroup.ItemsSource = timeTableViewModel.GetSubgroups();
            Choose_subgroup.SelectedValue = stud.Group.Subgroup;
        }


        private void UpdateTT(string week)
        {
            if (stud.IsHeadman)
            {
                int course = Convert.ToInt32(Choose_course.SelectedValue);
                int group = Convert.ToInt32(Choose_group.SelectedValue);
                int subgroup = Convert.ToInt32(Choose_subgroup.SelectedValue);
                timeTableViewModel.GetByWeekAdmin(week, course, group, subgroup);
                DataContext = new TimeTableViewModel(course, group, subgroup, week);
                //int idGroup = Convert.ToInt32(IdGroup.SelectedItem);
                //timeTableViewModel.GetByWeek(week, idGroup);
                //DataContext = new TimeTableViewModel(idGroup, week);
            }

            else
            {
                timeTableViewModel.GetByWeek(week, (int)stud.idGroup);
                DataContext = new TimeTableViewModel((int)stud.idGroup, week);
            }
        }

        private void Stud_Week_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTT(((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString());
        }

        private void Stud_Week_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var el in Stud_Week.Items)
            {
                if ((el is ComboBoxItem))
                {
                    if ((el as ComboBoxItem).Content.ToString() == timeTableViewModel.CurrentWeek())
                    {
                        (el as ComboBoxItem).IsSelected = true;
                        break;
                    }
                }
            }
        }

        private void LoadTimeTableIfEmpty()
        {
            timeTableViewModel.LoadTT();
        }
    }
}
