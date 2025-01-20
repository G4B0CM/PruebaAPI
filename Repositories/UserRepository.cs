using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamingApp.Models;

namespace GamingApp.Repositories
{
    public class UserRepository
    {
        string _dbPath;


        public string StatusMessage { get; set; }

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<User>();
        }

        public UserRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void agregarUsuario(string name, string username,string email)
        {
            int result = 0;
            try
            {
                Init();

                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                if (string.IsNullOrEmpty(username))
                    throw new Exception("Valid description required");

                if (string.IsNullOrEmpty(email))
                    throw new Exception("Valid description required");

                result = conn.Insert(new User { Name = name, Username = username,Email = email });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }

        }

        public List<User> GetAllPeople()
        {
            try
            {
                Init();
                return conn.Table<User>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<User>();
        }
        public void EliminarPersona(string name)
        {
            int result = 0;
            try
            {
                Init();

                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");
                var person = conn.Table<Models.Gamer>().FirstOrDefault(p => p.name == name);
                result = conn.Delete(person);

                StatusMessage = string.Format("{0} record(s) deleted (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete {0}. Error: {1}", name, ex.Message);
            }
        }
        
    }
}
