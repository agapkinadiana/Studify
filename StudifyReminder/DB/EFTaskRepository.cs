using StudifyReminder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudifyReminder.DB
{
    class EFTaskRepository
    {
        private StudEntities context;

        public EFTaskRepository()
        {
            context = new StudEntities();
        }

        public IEnumerable<Model.Task> getTasks()
        {
            return context.Task;
        }

        public void SaveTask()
        {
            context.SaveChanges();
        }

        public ObservableCollection<Model.Task> getTasksLocal()
        {
            return context.Task.Local;
        }

        public void addTask(Model.Task task)
        {
            context.Task.Add(task);
            context.SaveChanges();
        }

        public void Update(Model.Task task)
        {
            context.Entry(task).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateAll()
        {
            context.SaveChanges();
        }

        public void Remove(Model.Task task)
        {
            context.Task.Remove(task);
            context.SaveChanges();
        }

        public void OrderTasks(Student student)
        {
            context.Task.Where(p => p.idStudent == student.idStudent).OrderBy(p => p.DueDate).Load();
        }

        public Model.Task GetTaskById(Model.Task task)
        {
            return context.Task.FirstOrDefault(p => p.idTask == task.idTask);
        }

        public void ChangeComplite(Model.Task task)
        {
            GetTaskById(task).isComplite = task.isComplite;
            context.SaveChanges();
        }

        public Model.Task Find(Model.Task task)
        {
            return context.Task.Find(task.idTask);
        }

        public List<Model.Task> GetTaskByStudId(User user)
        {
            return context.Task.Where(p => p.idStudent == user.Student.idStudent).ToList();
        }

        public void RemoveById(Model.Task task)
        {
            context.Task.Remove(GetTaskById(task));
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)
        {
            foreach (Model.Task task in getTasks())
            {
                if (task.idStudent == student.idStudent)
                {
                    context.Task.Remove(task);
                }
            }
            context.SaveChanges();
        }
    }
}
