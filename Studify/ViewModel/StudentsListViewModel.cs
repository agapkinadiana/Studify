﻿using Studify.DB;
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
    class StudentsListViewModel : INotifyPropertyChanged
    {
        EFUserRepository eFUser = new EFUserRepository();
        EFProgressRepository eFProgress = new EFProgressRepository();
        EFTaskRepository eFTask = new EFTaskRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public IEnumerable<Student> getStudents()
        {
            return eFStudent.getStudens();
        }

        public IEnumerable<Student> getStudentsNoHeadman()
        {
            return eFStudent.getStudentsNoHeadman();
        }

        public void UpdateAll()
        {
            eFStudent.UpdateAll();
        }

        public void RemoveStudent(Student student)
        {
            eFStudent.Remove(student);
        }

        public void RemoveAllInfAboutStudent(Student student)
        {
            eFProgress.RemoveByStudId(student);
            eFTask.RemoveByStudId(student);
            eFUser.RemoveUserById(student);
            eFStudent.RemoveStudentById(student);
            tmpStudents.Remove(student);
        }

        User User = User.CurrentUser;

        ObservableCollection<Student> tmpStudents = new ObservableCollection<Student>();

        public ObservableCollection<Student> Students
        {
            get { return tmpStudents; }
        }

        Student selectedItem;

        public Student SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public StudentsListViewModel()
        {
            var students = this.getStudents();
            tmpStudents.Clear();
            foreach (Student a in students)
                tmpStudents.Add(a);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
