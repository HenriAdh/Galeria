using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDAM
{
    public class Photos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Comment { get; set; }
        public bool Favorite { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
