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

namespace nfcmai.Controllers
{
    public class TreasureHuntTipsController : ApiController
    {
        private KeptorDBEntities db = new KeptorDBEntities();

        // GET: api/TreasureHuntTips
        public IQueryable<TreasureHuntTips> GetTreasureHuntTips()
        {

            db.Configuration.ProxyCreationEnabled = false;

            return db.TreasureHuntTips;
        }

        public Clues GetTreasureHuntTips([FromUri] int gameId, [FromUri] string participant)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var d = db.TreasureHuntTips.FirstOrDefault(i => i.TreasureHuntGamesId == gameId && 
            i.IsFirst.HasValue && 
            i.IsFirst.Value);
            Clues cl = new Clues();
            if (d != null)
            {
                var part = db.TreasureHuntParticipants.FirstOrDefault(i =>i.Name == participant);
                if(part==null)
                {
                    part = new TreasureHuntParticipants();
                    part.Name = participant;
                    part.TreasureHuntId = gameId;
                    db.TreasureHuntParticipants.Add(part);
                }
                else
                {
                    part.TreasureHuntId = gameId;                  
                }
                db.SaveChanges();

                cl.HFCode = d.HFCode;
                cl.Id = d.Id;
                cl.IsFinal = d.IsFinal.HasValue ? d.IsFinal.Value : false;
                cl.IsFirst = d.IsFirst.HasValue ? d.IsFirst.Value : false;
                cl.Keimeno = d.Keimeno;
                cl.Picture = d.Picture;
                cl.Titlos = d.Titlos;
            }
            return cl;
        }
        public Clues GetTreasureHuntTips([FromUri] string participant, [FromUri] string HFCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Clues cl = new Clues();
            var part = db.TreasureHuntParticipants.FirstOrDefault(i => i.Name == participant);
            if (part != null)
            {
                var d = db.TreasureHuntTips.FirstOrDefault(i => i.HFCode == HFCode);
                if (d != null)
                {
                    d.TakenBy = part.Id;
                    d.TreasureHuntParticipants = part;
                    if(d.IsFinal.HasValue&&d.IsFinal.Value)
                    {
                        var game = db.TreasureHuntGames.FirstOrDefault(i => i.Id == d.TreasureHuntGamesId);
                        if(game!=null)
                        {
                            game.Winner = part.Id;
                            game.TreasureHuntParticipants1 = part;
                        }
                    }
                    db.SaveChanges();

                    cl.HFCode = d.HFCode;
                    cl.Id = d.Id;
                    cl.IsFinal = d.IsFinal.HasValue ? d.IsFinal.Value : false;
                    cl.IsFirst = d.IsFirst.HasValue ? d.IsFirst.Value : false;
                    cl.Keimeno = d.Keimeno;
                    cl.Picture = d.Picture;
                    cl.Titlos = d.Titlos;
                }
            }
            return cl;
        }

        // GET: api/TreasureHuntTips/5
        [ResponseType(typeof(TreasureHuntTips))]
        public IHttpActionResult GetTreasureHuntTips(int id)
        {
            TreasureHuntTips treasureHuntTips = db.TreasureHuntTips.Find(id);
            if (treasureHuntTips == null)
            {
                return NotFound();
            }

            return Ok(treasureHuntTips);
        }

        // PUT: api/TreasureHuntTips/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTreasureHuntTips(int id, TreasureHuntTips treasureHuntTips)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != treasureHuntTips.Id)
            {
                return BadRequest();
            }

            db.Entry(treasureHuntTips).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreasureHuntTipsExists(id))
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

        // POST: api/TreasureHuntTips
        [ResponseType(typeof(TreasureHuntTips))]
        public IHttpActionResult PostTreasureHuntTips(TreasureHuntTips treasureHuntTips)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TreasureHuntTips.Add(treasureHuntTips);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = treasureHuntTips.Id }, treasureHuntTips);
        }

        // DELETE: api/TreasureHuntTips/5
        [ResponseType(typeof(TreasureHuntTips))]
        public IHttpActionResult DeleteTreasureHuntTips(int id)
        {
            TreasureHuntTips treasureHuntTips = db.TreasureHuntTips.Find(id);
            if (treasureHuntTips == null)
            {
                return NotFound();
            }

            db.TreasureHuntTips.Remove(treasureHuntTips);
            db.SaveChanges();

            return Ok(treasureHuntTips);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TreasureHuntTipsExists(int id)
        {
            return db.TreasureHuntTips.Count(e => e.Id == id) > 0;
        }
    }
    public class Clues
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Titlos { get; set; }
        public string Keimeno { get; set; }
        public bool IsFinal { get; set; }
        public string HFCode { get; set; }
        public bool IsFirst { get; set; }
    }
}