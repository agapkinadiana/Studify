using StudifyReminder.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudifyReminder.DB
{
    class EFProgressRepository
    {
        private StudEntities context;

        public EFProgressRepository()
        {
            context = new StudEntities();
        }

        public IEnumerable<Progress> getProgress()
        {
            return context.Progress;
        }

        public IEnumerable<Progress> getProgressById(Student student)
        {
            return context.Progress.Where(p => p.idStudent == student.idStudent);
        }

        public void addProgress(Progress progress)
        {
            context.Progress.Add(progress);
            context.SaveChanges();
        }

        public void SaveProgress()
        {
            context.SaveChanges();
        }

        public void Update(Progress progress)
        {
            context.Entry(progress).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Remove(Progress progress)
        {
            context.Progress.Remove(progress);
            context.SaveChanges();
        }

        public Progress Find(Progress progress)
        {
            return context.Progress.Find(progress.idProgress);
        }

        public Progress GetProgressById(Progress progress)
        {
            return context.Progress.FirstOrDefault(p => p.idProgress == progress.idProgress);
        }

        public void RemoveById(Progress progress)
        {
            context.Progress.Remove(GetProgressById(progress));
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)
        {
            foreach(Progress progress in getProgress())
            {
                if(progress.idStudent == student.idStudent)
                {
                    context.Progress.Remove(progress);
                }
            }
            context.SaveChanges();
        }

        public void OrderProgress(Student student)
        {
            context.Progress.Where(p => p.idStudent == student.idStudent).OrderBy(p => p.LessonName).Load();
        }
    }
}
