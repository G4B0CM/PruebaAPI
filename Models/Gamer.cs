using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingApp.Models
{
    [Table ("Gamer")]
    public class Gamer
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
