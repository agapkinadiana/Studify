using StudifyReminder.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudifyReminder.DB
{
    class EFUserRepository
    {
        private StudEntities context;

        public EFUserRepository()
        {
            context = new StudEntities();
        }

        public IEnumerable<User> getUsers()
        {
            return context.User;
        }

        public void addUser(User user)
        {
            context.User.Add(user);
            context.SaveChanges();
        }

        public User Find(string login)
        {
            return context.User.Find(login);
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(User user)
        {
            context.User.Remove(user);
            context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return context.User.FirstOrDefault(x => x.idStudent == id);
        }

        public void RemoveUserById(Student student)
        {
            context.User.Remove(GetUserById(student.idStudent));
            context.SaveChanges();
        }

        public User GetUserByLogin(string login)
        {
            return context.User.FirstOrDefault(x => x.Login == login);
        }
    }
}
