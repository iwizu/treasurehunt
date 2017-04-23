using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Ultratimer
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Finished { get; set; }
        public string Winner { get; set; }
        public string ParticipantName { get; set; }
    }
}
