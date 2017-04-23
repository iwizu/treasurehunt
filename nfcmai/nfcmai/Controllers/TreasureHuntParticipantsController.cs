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
    public class TreasureHuntParticipantsController : ApiController
    {
        private KeptorDBEntities db = new KeptorDBEntities();

        // GET: api/TreasureHuntParticipants
        public IQueryable<TreasureHuntParticipants> GetTreasureHuntParticipants()
        {
            return db.TreasureHuntParticipants;
        }

        // GET: api/TreasureHuntParticipants/5
        [ResponseType(typeof(TreasureHuntParticipants))]
        public IHttpActionResult GetTreasureHuntParticipants(int id)
        {
            TreasureHuntParticipants treasureHuntParticipants = db.TreasureHuntParticipants.Find(id);
            if (treasureHuntParticipants == null)
            {
                return NotFound();
            }

            return Ok(treasureHuntParticipants);
        }

        // PUT: api/TreasureHuntParticipants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTreasureHuntParticipants(int id, TreasureHuntParticipants treasureHuntParticipants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != treasureHuntParticipants.Id)
            {
                return BadRequest();
            }

            db.Entry(treasureHuntParticipants).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreasureHuntParticipantsExists(id))
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

        // POST: api/TreasureHuntParticipants
        [ResponseType(typeof(TreasureHuntParticipants))]
        public IHttpActionResult PostTreasureHuntParticipants(TreasureHuntParticipants treasureHuntParticipants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TreasureHuntParticipants.Add(treasureHuntParticipants);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = treasureHuntParticipants.Id }, treasureHuntParticipants);
        }

        // DELETE: api/TreasureHuntParticipants/5
        [ResponseType(typeof(TreasureHuntParticipants))]
        public IHttpActionResult DeleteTreasureHuntParticipants(int id)
        {
            TreasureHuntParticipants treasureHuntParticipants = db.TreasureHuntParticipants.Find(id);
            if (treasureHuntParticipants == null)
            {
                return NotFound();
            }

            db.TreasureHuntParticipants.Remove(treasureHuntParticipants);
            db.SaveChanges();

            return Ok(treasureHuntParticipants);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TreasureHuntParticipantsExists(int id)
        {
            return db.TreasureHuntParticipants.Count(e => e.Id == id) > 0;
        }
    }
}