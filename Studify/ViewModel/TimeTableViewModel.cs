using Studify.DB;
using Studify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Studify.ViewModel
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        User user = User.CurrentUser;
        Student stud = new Student();
        Group gr = new Group();

        IEnumerable<Group> groups;

        EFGroupRepository eFGroup = new EFGroupRepository();
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public TimeTableViewModel()
        {
            groups = eFGroup.getGroups();
            stud = eFStudent.GetStudentById((int)user.idStudent);
        }

        public IEnumerable<int> GetCourse()
        {
            return eFGroup.GetCourse();
        }

        public IEnumerable<int> GetGroups()
        {
            return eFGroup.GetGroups();
        }

        public IEnumerable<int> GetSubgroups()
        {
            return eFGroup.GetSubgroups();
        }

        private TimeTable selectedTimeTable;

        private ObservableCollection<TimeTable> timeTables = new ObservableCollection<TimeTable>();

        public ObservableCollection<TimeTable> TimeTables
        {
            get { return timeTables; }
            set { timeTables = value; }
        }

        public TimeTable SelectedTimeTable
        {
            get
            {
                return selectedTimeTable;
            }
            set
            {
                if (value != null)
                {
                    //eFTimeTable.UpdateTT(SelectedTimeTable);
                    selectedTimeTable = value;
                    Save();
                    OnPropertyChanged("SelectedTimeTable");
                }
                
            }
        }

        public ObservableCollection<TimeTable> GetByWeekAdmin(string week, int course, int group, int subgroup)
        {
            TimeTables.Clear();
            foreach (TimeTable tt in getTimeTable())
            {
                gr = eFGroup.GetGroupById((int)tt.idGroup);
                if (gr.Course == course && tt.Week == week && gr.GroupNumber == group && gr.Subgroup == subgroup)
                {
                    TimeTables.Add(tt);
                }
            }
            return TimeTables;
        }

        public ObservableCollection<TimeTable> GetByWeek(string week, int id)
        {
            TimeTables.Clear();
            foreach (TimeTable tt in getTimeTable())
            {
                if (tt.idGroup == id && tt.Week == week)
                {
                    TimeTables.Add(tt);
                }
            }
            return TimeTables;
        }

        public void LoadTT()
        {
            if (eFTimeTable.CheckTimeTableByIdGroup(stud))
            {
                for (int i = 1; i < 7; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        TimeTable t1 = new TimeTable { Day = i, LessonNumber = j, idGroup = stud.idGroup, Week = "First", Auditorium = "", LessonName = "", LessonType = "" };
                        TimeTable t2 = new TimeTable { Day = i, LessonNumber = j, idGroup = stud.idGroup, Week = "Second", Auditorium = "", LessonName = "", LessonType = "" };
                        eFTimeTable.addTimeTable(t1);
                        eFTimeTable.addTimeTable(t2);
                        TimeTables.Add(t1);
                        TimeTables.Add(t2);
                    }
                }
            }
        }

        public TimeTableViewModel(int id, string week)
        {
            eFTimeTable.Clear();
            eFTimeTable.getTimeTableByIdGroupAndWeek(id, week);
            TimeTables = eFTimeTable.getTimeTableLocal();
        }

        public TimeTableViewModel(int course, int group, int subgroup, string week)
        {
            eFTimeTable.Clear();
            eFTimeTable.getTimeTableByIdGroupAndWeekAdmin(course, group, subgroup, week);
            TimeTables = eFTimeTable.getTimeTableLocal();
        }

        public void getTimeTableByIdGroupAndWeek(int id, string week)
        {
            eFTimeTable.getTimeTableByIdGroupAndWeek(id, week);
        }

        public IEnumerable<TimeTable> getTimeTable()
        {
            return eFTimeTable.getTimeTable();
        }

        public void Save()
        {
            eFTimeTable.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string CurrentWeek()
        {
            int dayStart = FirstSeptDay().DayOfYear - (int)FirstSeptDay().DayOfWeek + 1;//Номер понедельника в году в неделе с первым сентября
            if ((DaysSinceStart(dayStart) / 7) % 2 == 0)
            {
                return "First";
            }
            else return "Second";
        }

        private int DaysSinceStart(int dayStart)
        {
            if (DateTime.Now.Month > 8)
                return DateTime.Now.DayOfYear - dayStart;
            else
                if (DateTime.IsLeapYear(FirstSeptDay().Year))
                return 366 - dayStart + DateTime.Now.DayOfYear;
            else
                return 365 - dayStart + DateTime.Now.DayOfYear;
        }

        private DateTime FirstSeptDay()
        {
            DateTime d = DateTime.Now;
            DateTime ds;
            if (d.Month < 9)
                ds = new DateTime(DateTime.Now.Year - 1, 9, 1);
            else
                ds = new DateTime(DateTime.Now.Year, 9, 1);
            return ds;
        }
    }
}
