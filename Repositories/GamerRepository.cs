using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamingApp.Models;

namespace GamingApp.Repositories
{
    public class GamerRepository
    {
        string _dbPath;


        public string StatusMessage { get; set; }

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Gamer>();
        }

        public GamerRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void agregarGamer(string name,string description)
        {
            int result = 0;
            try
            {
                Init();

                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                if (string.IsNullOrEmpty(description))
                    throw new Exception("Valid description required");

                result = conn.Insert(new Gamer { name = name, description = description });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }

        }

        public List<Gamer> GetAllPeople()
        {
            try
            {
                Init();
                return conn.Table<Gamer>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Gamer>();
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
