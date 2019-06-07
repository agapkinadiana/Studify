using Studify.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studify.DB
{
    class EFGroupRepository
    {
        private StudEntities context;

        public EFGroupRepository()
        {
            context = new StudEntities();
        }

        public IEnumerable<Group> getGroups()
        {
            return context.Group;
        }

        public Group GetGroup(int groupnumber, int course, int subgroup)
        {
            return context.Group.FirstOrDefault(x => x.GroupNumber == groupnumber && x.Course == course && x.Subgroup ==subgroup);
        }

        public void addGroup(Group group)
        {
            if(context.Group.Where(x => x.GroupNumber == group.GroupNumber && x.Course == group.Course && x.Subgroup == group.Subgroup).Count() == 0)
            {
                context.Group.Add(group);
            }
            context.SaveChanges();
        }

        public void Update(Group group)
        {
            context.Entry(group).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Group group)
        {
            context.Group.Remove(group);
            context.SaveChanges();
        }

        public Group GetGroupById(int id)
        {
            return context.Group.FirstOrDefault(x => x.idGroup == id);
        }

        public void RemoveGroupById(Student student)
        {
            context.Group.Remove(GetGroupById((int)student.idGroup));
            context.SaveChanges();
        }

        //public List<int> GetIdGroups()
        //{
        //    List<int> idGroups = new List<int>();
        //    foreach (Group g in getGroups())
        //    {
        //        if (g.idGroup != 0)
        //            idGroups.Add(g.idGroup);
        //    }
        //    return idGroups;
        //}

        public IEnumerable<int> GetCourse()
        {
            List<int> GrCourse = new List<int>();
            foreach (Group g in getGroups())
            {
                if (g.Course != 0)
                    GrCourse.Add((int)g.Course);
                GrCourse.Sort();
            }
            return GrCourse.Distinct();
        }

        public IEnumerable<int> GetGroups()
        {
            List<int> Groups = new List<int>();
            foreach (Group g in getGroups())
            {
                if (g.GroupNumber != 0)
                    Groups.Add((int)g.GroupNumber);
                Groups.Sort();
            }
            return Groups.Distinct();
        }

        public IEnumerable<int> GetSubgroups()
        {
            List<int> GrSubGroups = new List<int>();
            foreach (Group g in getGroups())
            {
                if (g.Subgroup != 0)
                    GrSubGroups.Add((int)g.Subgroup);
                GrSubGroups.Sort();
            }
            return GrSubGroups.Distinct();
        }

    }
}
