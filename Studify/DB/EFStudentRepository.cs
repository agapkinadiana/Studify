using Studify.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studify.DB
{
    class EFStudentRepository
    {
        private StudEntities context;

        public EFStudentRepository()
        {
            context = new StudEntities();
        }

        public IEnumerable<Student> getStudens()
        {
            return context.Student;
        }

        public void addStudent(Student student)
        {
            context.Student.Add(student);
            context.SaveChanges();
        }

        public void Update(Student student)
        {
            context.Entry(student).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateAll()
        {
            context.SaveChanges();
        }

        public void Remove(Student student)
        {
            context.Student.Remove(student);
            context.SaveChanges();
        }

        public Student GetStudentById(int id)
        {
            return context.Student.FirstOrDefault(x => x.idStudent == id);
        }

        public void RemoveStudentById(Student student)
        {
            context.Student.Remove(GetStudentById(student.idStudent));
            context.SaveChanges();
        }

        public IEnumerable<Student> getStudentsNoHeadman()
        {
            return context.Student.Where(s => s.isHeadman == false);
        }
    }
}
