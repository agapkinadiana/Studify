using Studify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studify.DB
{
    class EFTimeTableRepository
    {
        private StudEntities context;

        public EFTimeTableRepository()
        {
            context = new StudEntities();
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

        public void UpdateTT (TimeTable timeTable)
        {
            TimeTable tmp = context.TimeTable.FirstOrDefault(x => x.idTimeTable == timeTable.idTimeTable);
            tmp.LessonName = timeTable.LessonName;
            tmp.Auditorium = timeTable.Auditorium;
            tmp.LessonType = timeTable.LessonType;
            Save();
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

        public void getTimeTableByIdGroupAndWeekAdmin(int course, int group, int subgroup, string week)
        {
            context.TimeTable.Where(p => p.Group.Course == course && p.Week == week && p.Group.GroupNumber == group && p.Group.Subgroup == subgroup).Load();
        }

        public bool CheckTimeTableByIdGroup(Student student)
        {
            if (context.TimeTable.Where(p => p.idGroup == student.idGroup).Count() == 0)
                return true;
            else return false;
        }

        public IEnumerable<string> GetSubjects(Student student)
        {
            return context.TimeTable.Where(p => (p.Group.idGroup == student.idGroup) && (p.LessonName != "")).OrderBy(p => p.LessonName).Select(p => p.LessonName).Distinct().ToList();
        }
    }
}
