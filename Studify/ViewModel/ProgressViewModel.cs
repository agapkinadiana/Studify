using Studify.DB;
using Studify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using Studify.ErrorMessage;

namespace Studify.ViewModel
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        EFProgressRepository eFProgress = new EFProgressRepository();
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        int allTasks=1;
        int complitedTasks=0;
        int countProgress=0;

        User user = User.CurrentUser;
        Student stud = new Student();

        Progress selectedItem;
        ObservableCollection<Progress> progresses = new ObservableCollection<Progress>();

        public ObservableCollection<Progress> Progresses
        {
            get { return progresses; }
            set { progresses = value; }
        }

        public Progress SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                    selectedItem = value;
                OnPropertyChanged("SelectedItem");                
            }
        }

        public ProgressViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            Update();
        }

        public int ComplitedTasks
        {
            get { return complitedTasks; }
            set
            {
                if (value >= 0)
                    complitedTasks = value;
                OnPropertyChanged("ComplitedTasks");
            }
        }

        public int NeededTasks
        {
            get { return allTasks; }
            set
            {
                if (value > 0)
                    allTasks = value;
                Update();
                OnPropertyChanged("NeededTasks");
            }
        }

        public int CountProgress
        {
            get { return countProgress; }
            set
            {
                countProgress = value;
                OnPropertyChanged("CountProgress");
            }
        }

        void Update()
        {
            Progresses.Clear();
            foreach (Progress progress in eFProgress.getProgressById(stud))
            {
                Progresses.Add(progress);
            }
            if (SelectedItem != null)
            {
                CountProgress = (int)(SelectedItem.ComplitedTasks * 100 / SelectedItem.NeededTasks);
                
                eFProgress.Find(SelectedItem).TaskProgress = CountProgress;
                eFProgress.Find(SelectedItem).ComplitedTasks = SelectedItem.ComplitedTasks;
                eFProgress.Find(SelectedItem).NeededTasks = SelectedItem.NeededTasks;
                SelectedItem.TaskProgress = CountProgress;
                SaveProgress();

            }
        }

        public void addComplitedTasks()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.ComplitedTasks < SelectedItem.NeededTasks)
                {
                    SelectedItem.ComplitedTasks += 1;
                    Update();
                }
            } else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void minusComplitedTasks()
        {
            if (SelectedItem != null)
            {
                SelectedItem.ComplitedTasks -= 1;
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void addNeededTasks()
        {
            if (SelectedItem != null)
            {
                SelectedItem.NeededTasks += 1;
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void minusNeededTasks()
        {
            if (SelectedItem != null)
            {
                SelectedItem.NeededTasks -= 1;
                if (SelectedItem.ComplitedTasks > SelectedItem.NeededTasks)
                {
                    SelectedItem.ComplitedTasks -= 1;
                }
                Update();
            }
            else MyMessageBox.Show("Choose element!", MessageBoxButton.OK);
        }

        public void RemoveById()
        {
            eFProgress.RemoveById(SelectedItem);
            Progresses.Remove(SelectedItem);
        }

        public void OrderProgress()
        {
            eFProgress.OrderProgress(stud);
        }

        public IEnumerable<string> GetSubjects()
        {
            return eFTimeTable.GetSubjects(stud);
        }

        public void addProgress(Progress progress)
        {
            Progresses.Add(progress);
            eFProgress.addProgress(progress);
        }

        public Progress GetProgressById(Progress progress)
        {
            return eFProgress.GetProgressById(progress);
        }

        public void SaveProgress()
        {
            eFProgress.SaveProgress();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
