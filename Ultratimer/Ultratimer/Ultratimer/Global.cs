using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultratimer
{
    public static class Global
    {
        private static Game game = new Game();
        private static List<Game> games = new List<Game>();     
           
        public static List<Game> Games
        {
            get
            {
                return games;
            }
            set
            {
                games = value;

            }
        }
        public static Game AGame
        {
            get
            {
                return game;
            }
            set
            {
                game = value;

            }
        }       
    }
}
