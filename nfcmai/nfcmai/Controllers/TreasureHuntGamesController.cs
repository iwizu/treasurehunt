using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using nfcmai;
using System.Runtime.Serialization;

namespace nfcmai.Controllers
{
    public class TreasureHuntGamesController : ApiController
    {

        private KeptorDBEntities db = new KeptorDBEntities();

        // GET: api/TreasureHuntGames
        public List<Game> GetTreasureHuntGames()
        {
            db.Configuration.ProxyCreationEnabled = false;
            DateTime dt = DateTime.Now;
            List<TreasureHuntGames> d = db.TreasureHuntGames.
                Where(i=>(!i.Finished.HasValue)||(i.Finished.HasValue&&!i.Finished.Value))
                .ToList();
            List<Game> lst = new List<Game>();
            foreach (var i in d)
            {
                Game g = new Game();
                g.Finished = i.Finished.HasValue ? i.Finished.Value : false;
                g.Id = i.Id;
                g.Name = i.Name;
                g.ParticipantName = "";
                if (i.TreasureHuntParticipants1 != null)
                    g.Winner = i.TreasureHuntParticipants1.Name;
                else
                    g.Winner = "";
                lst.Add(g);
            }
            return lst;
        }

        // GET: api/TreasureHuntGames/5
        [ResponseType(typeof(TreasureHuntGames))]
        public IHttpActionResult GetTreasureHuntGames(int id)
        {
            TreasureHuntGames treasureHuntGames = db.TreasureHuntGames.Find(id);
            if (treasureHuntGames == null)
            {
                return NotFound();
            }

            return Ok(treasureHuntGames);
        }

        // PUT: api/TreasureHuntGames/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTreasureHuntGames(int id, TreasureHuntGames treasureHuntGames)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != treasureHuntGames.Id)
            {
                return BadRequest();
            }

            db.Entry(treasureHuntGames).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreasureHuntGamesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TreasureHuntGames
        [ResponseType(typeof(TreasureHuntGames))]
        public IHttpActionResult PostTreasureHuntGames(TreasureHuntGames treasureHuntGames)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TreasureHuntGames.Add(treasureHuntGames);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = treasureHuntGames.Id }, treasureHuntGames);
        }

        // DELETE: api/TreasureHuntGames/5
        [ResponseType(typeof(TreasureHuntGames))]
        public IHttpActionResult DeleteTreasureHuntGames(int id)
        {
            TreasureHuntGames treasureHuntGames = db.TreasureHuntGames.Find(id);
            if (treasureHuntGames == null)
            {
                return NotFound();
            }

            db.TreasureHuntGames.Remove(treasureHuntGames);
            db.SaveChanges();

            return Ok(treasureHuntGames);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TreasureHuntGamesExists(int id)
        {
            return db.TreasureHuntGames.Count(e => e.Id == id) > 0;
        }
    }
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Finished { get; set; }
        public string Winner { get; set; }
        public string ParticipantName { get; set; }
    }
}