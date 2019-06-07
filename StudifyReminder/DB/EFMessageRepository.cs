using StudifyReminder.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudifyReminder.DB
{
    class EFMessageRepository
    {
        User user = User.CurrentUser;

        private StudEntities context;

        public EFMessageRepository()
        {
            context = new StudEntities();
        }

        public EFMessageRepository(StudEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Message> getMessages()
        {
            return context.Message;
        }

        public int CountMessages()
        {
            return context.Message.Count();
        }

        public void addMessage(Message message)
        {
            context.Message.Add(message);
            context.SaveChanges();
        }

        public void Update(Message message)
        {
            context.Entry(message).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateAll()
        {
            context.SaveChanges();
        }
        public void Remove(Message message)
        {
            context.Message.Remove(message);
            context.SaveChanges();
        }

        public Message GetMessageById(int id)
        {
            return context.Message.FirstOrDefault(x => x.idMessage == id);
        }

        public void OrderMessage()
        {
            context.Message.Distinct().OrderByDescending(p => p.DateOfCreate).Load();
        }

        public IEnumerable<Message> getMessagesLocal()
        {
            return context.Message.Local;
        }

        public void RemoveMessageById (Message message)
        {
            context.Message.Remove(GetMessageById(message.idMessage));
            context.SaveChanges();
        }
    }
}
