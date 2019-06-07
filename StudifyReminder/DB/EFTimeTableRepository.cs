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
    class EFTimeTableRepository
    {
        private StudEntities context;

        public EFTimeTableRepository()
        {
            context = new StudEntities();
        }

        public EFTimeTableRepository(StudEntities context)
        {
            this.context = context;
        }

        public IEnumerable<TimeTable> getTimeTable()
        {
            return context.TimeTable.Local;
        }

        public ObservableCollection<TimeTable> getTimeTableLocal()
        {
            return context.TimeTable.Local;
        }

        public void addTimeTable(TimeTable timeTable)
        {
            context.TimeTable.Add(timeTable);
            context.SaveChanges();
        }

        public void Clear()
        {
            context.TimeTable.Local.Clear();
        }

        public void Update(TimeTable timeTable)
        {
            context.Entry(timeTable).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Remove(TimeTable timeTable)
        {
            context.TimeTable.Remove(timeTable);
            context.SaveChanges();
        }

        public void getTimeTableByIdGroupAndWeek(int id, string week)
        {
            context.TimeTable.Where(p => p.idGroup == id && p.Week == week).Load();
        }

        public bool CheckTimeTableByIdGroup(Student student)
        {
            if (context.TimeTable.Where(p => p.idGroup == student.idGroup).Count() == 0)
                return true;
            else return false;
        }

        public List<TimeTable> GetLabsOnTomorrow(string week, User user, int tomorrow)
        {
            return context.TimeTable.Where(t => t.Week == week &&
                t.idGroup == user.Student.idGroup &&
                (t.Day) == tomorrow &&
                t.LessonType.ToLower() == "лр").ToList();
        }

        public IEnumerable<string> GetSubjects(Student student)
        {
            return context.TimeTable.Where(p => (p.Group.idGroup == student.idGroup) && (p.LessonName != "")).OrderBy(p => p.LessonName).Select(p => p.LessonName).Distinct().ToList();
        }
    }
}
