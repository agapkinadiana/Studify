using Studify.DB;
using Studify.ErrorMessage;
using Studify.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Studify.ViewModel
{
    class AuthorizationViewModel
    {
        IEnumerable<User> users;
        IEnumerable<Model.Group> groups;
        IEnumerable<Student> students;

        EFUserRepository eFUser = new EFUserRepository();
        EFGroupRepository eFGroup = new EFGroupRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        string login;
        int numStudCard;
        int course;
        int group;
        int subgroup;
        string name;
        string reg_login;
        string password;
        string reg_password;
        string db_password;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public int NumStudCard
        {
            get { return numStudCard; }
            set { numStudCard = value; }
        }

        public int Course
        {
            get { return course; }
            set { course = value; }
        }

        public int Group
        {
            get { return group; }
            set { group = value; }
        }

        public int Subgroup
        {
            get { return subgroup; }
            set { subgroup = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Reg_Login
        {
            get { return reg_login; }
            set { reg_login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Reg_Password
        {
            get { return reg_password; }
            set { reg_password = value; }
        }

        public string Db_Password
        {
            get { return db_password; }
            set { db_password = value; }
        }

        public AuthorizationViewModel()
        {
            users = eFUser.getUsers();
            groups = eFGroup.getGroups();
            students = eFStudent.getStudens();
        }

        public bool Registration (string password1, string password2)
        {
            Reg_Password = password1;
            Db_Password = password2;
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Reg_Login) && !String.IsNullOrEmpty(Reg_Password) && !String.IsNullOrEmpty(Db_Password))
            {
                if (Group >= 1 && Group <= 10)
                {
                    if (Course >= 1 && Course <= 5)
                    {
                        if (Subgroup == 1 || Subgroup == 2)
                        {
                            if (NumStudCard.ToString().Length == 8)
                            {
                                if (password1.Equals(password2))
                                {
                                    User tmp1 = eFUser.GetUserByLogin(Reg_Login);
                                    Student tmp2 = eFStudent.GetStudentById(NumStudCard);
                                    if (tmp2 == null)
                                    {
                                        if (tmp1 == null)
                                        {
                                            Model.Group tmp = eFGroup.GetGroup(Group, Course, Subgroup);
                                            if (tmp == null)
                                            {
                                                eFGroup.addGroup(new Model.Group(Group, Course, Subgroup));
                                            }
                                            eFStudent.addStudent(new Student(NumStudCard, eFGroup.GetGroup(Group, Course, Subgroup).idGroup, Name));
                                            eFUser.addUser(new User(NumStudCard, Reg_Login, User.getHash(Reg_Password)));
                                            eFUser.Save();
                                            return true;
                                        }
                                        else
                                        {
                                            MyMessageBox.Show("User with this login already exists!", MessageBoxButton.OK);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MyMessageBox.Show("A user with that student card number is already there!", MessageBoxButton.OK);
                                        return false;
                                    }
                                }
                                else
                                {
                                    MyMessageBox.Show("Passwords must match!", MessageBoxButton.OK);
                                    return false;
                                }
                            }
                            else
                            {
                                MyMessageBox.Show("Student card number must contain 8 digits!", MessageBoxButton.OK);
                                return false;
                            }
                        }
                        else
                        {
                            MyMessageBox.Show("Subgroup must be 1 or 2!", MessageBoxButton.OK);
                            return false;
                        }
                    }
                    else
                    {
                        MyMessageBox.Show("Course must be 1-5!", MessageBoxButton.OK);
                        return false;
                    }
                }
                else
                {
                    MyMessageBox.Show("Group must be 1-10!", MessageBoxButton.OK);
                    return false;
                }
            }
            else
            {
                MyMessageBox.Show("Check entered data!", MessageBoxButton.OK);
                return false;
            }
        }

        public User СompareDataOfUser(string password)
        {
            Password = password;
            if (!String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password))
            {
                User tmp = eFUser.GetUserByLogin(Login);
                if (tmp != null)
                {
                    if (User.getHash(Password).Equals(tmp.Password))
                    {
                        User.CurrentUser = tmp;
                        CreateRestoringFile(Login, Password);
                        return tmp;
                    }
                    else return null;
                }
                else
                    return null;
            }
            return null;
        }

        private void CreateRestoringFile(string login, string password)
        {
            XDocument doc = new XDocument(
                                    new XElement("Usr",
                                    new XAttribute("login", login),
                                    new XAttribute("password", password)));
            doc.Save(@"RestUsr.xml");
            doc.Save(@"StudifyReminder\Resources\RestUsr.xml");
            Directory.CreateDirectory(@"C:\Windows\syswow64\Resources");
            doc.Save(@"C:\Windows\syswow64\Resources\RestUsr.xml");
        }

        public IEnumerable<User> getUsers()
        {
            return eFUser.getUsers();
        }

    }
}
