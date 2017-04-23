using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultratimer
{
    public class Clues
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Titlos { get; set; }
        public string Keimeno { get; set; }
        public bool IsFinal { get; set; }
        public string HFCode { get; set; }
        public bool IsFirst { get; set; }

    }
}
