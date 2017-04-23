using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SQLite;
using System.IO;
using Android;
using Xamarin.Forms;

namespace Ultratimer.Data
{
  public  class GameDatabase
    {
        static object locker = new object();

        SQLiteConnection database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        public GameDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            // create the tables
            database.CreateTable<Clues>();

        }
        public void ClearDatabase()
        {
            lock (locker)
            {
                database.Query<Clues>("DELETE FROM [Clues]");
            }
        }
        #region Clues
        public IEnumerable<Clues> GetClues()
        {
            lock (locker)
            {
                return (from i in database.Table<Clues>() select i).ToList();
            }
        }

        public IEnumerable<Clues> GetItemsWithSql()
        {
            lock (locker)
            {
                return database.Query<Clues>("SELECT * FROM [Clues] ");
            }
        }
        public Clues GetClue(int Id)
        {
            lock (locker)
            {
                return database.Table<Clues>().FirstOrDefault(x => x.Id == Id);
            }
        }

        public int SaveClue(Clues item)
        {
            lock (locker)
            {                
                    return database.Insert(item);                
            }
        }

        public int DeleteClue(int Id)
        {
            lock (locker)
            {
                return database.Delete<Clues>(Id);
            }
        }
        public int DeleteAllClues()
        {
            database.Query<Clues>("DELETE FROM [Clues]");
            return 1;
        }
        #endregion
    }
}
