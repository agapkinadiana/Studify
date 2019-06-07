﻿using Studify.Model;
using Studify.ViewModel;
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
    /// Логика взаимодействия для StudentsListView.xaml
    /// </summary>
    public partial class StudentsListView : Page
    {
        StudentsListViewModel studentsListViewModel = new StudentsListViewModel();

        public StudentsListView()
        {
            InitializeComponent();
            DataContext = studentsListViewModel;

            Students_Grid.ItemsSource = studentsListViewModel.Students;
        }

        private void Students_Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                studentsListViewModel.UpdateAll();
                Students_Grid.ItemsSource = studentsListViewModel.Students;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var exc in ex.EntityValidationErrors)
                {
                    foreach (var err in (exc as System.Data.Entity.Validation.DbEntityValidationResult).ValidationErrors)
                    {
                        MessageBox.Show((err as System.Data.Entity.Validation.DbValidationError).ErrorMessage);
                        break;
                    }
                    break;
                }

            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Students_Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            if (currentCell.Column == Students_Grid.Columns[2])
            {
                Students_Grid.BeginEdit();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            studentsListViewModel.RemoveAllInfAboutStudent(studentsListViewModel.SelectedItem);
            Students_Grid.ItemsSource = studentsListViewModel.Students;
        }
    }
}
