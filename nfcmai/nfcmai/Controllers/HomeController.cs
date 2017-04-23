using nfcmai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nfcmai.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            string cookievalue;            
                //Assuming user comes back after several hours. several < 12.
                //Read the cookie from Request.
                HttpCookie myCookie = Request.Cookies["TreasureHunt"];
                if (myCookie == null)
                {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
                Guid g = Guid.NewGuid();
                    cookievalue = g.ToString();
                    //create a cookie
                    myCookie = new HttpCookie("TreasureHunt");
                    //Add key-values in the cookie
                    myCookie.Values.Add("userid", cookievalue.ToString());
                    //set cookie expiry date-time. Made it to last for next 12 hours.
                    myCookie.Expires = DateTime.Now.AddYears(20);
                    //Most important, write the cookie to client.
                    Response.Cookies.Add(myCookie);
                }
                else
                {
                    //ok - cookie is found.
                    //Gracefully check if the cookie has the key-value as expected.
                    if (!string.IsNullOrEmpty(myCookie.Values["userid"]))
                    {
                        string userId = myCookie.Values["userid"].ToString();
                        //Yes userId is found. Mission accomplished.
                    }
                }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #region Games CRUD
        public JsonResult GamesDataHandler(DTParameters param)
        {
            try
            {
                using (KeptorDBEntities c = new KeptorDBEntities())
                {
                    //int g = -1;
                    //if (param.parentId != null && param.parentId != "")
                        //g = Convert.ToInt32(param.parentId);

                    var dtsource = c.TreasureHuntGames
                        .Select(i => new UGames()
                        {
                             Id = i.Id,
                             Name = i.Name,
                              Started = i.Started,
                             Finished = i.Finished,
                            Winner = i.Winner,
                            WinnerStr = i.Winner.HasValue?i.TreasureHuntParticipants1.Name:"",
                             UniqueId = i.UniqueId,
                        }).ToList();


                    List<String> columnSearch = new List<string>();

                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                    }

                    List<UGames> data = new GamesResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
                    int count = new GamesResultSet().Count(param.Search.Value, dtsource, columnSearch);
                    DTResult<UGames> result = new DTResult<UGames>
                    {
                        draw = param.Draw,
                        data = data,
                        recordsFiltered = count,
                        recordsTotal = count
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult CreateGamePartialView()
        {
            return PartialView("CreateGamePartialView");
        }
        [HttpPost]
        public void CreateGame(UGames i)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                HttpCookie myCookie = Request.Cookies["TreasureHunt"];
                if (myCookie!=null&&!string.IsNullOrEmpty(myCookie.Values["userid"]))
                {
                    string userId = myCookie.Values["userid"].ToString();
                    //Yes userId is found. Mission accomplished.                
                    Guid g = Guid.Parse(userId);
                    TreasureHuntGames o = new TreasureHuntGames();
                    o.Finished = i.Finished;
                    o.UniqueId = g;
                    o.Name = i.Name;
                    o.Started = i.Started;
                    if (i.Started.HasValue)
                        o.Started = i.Started;
                    else
                        o.Started = null;
                    o.Winner = i.Winner;

                    c.TreasureHuntGames.Add(o);
                    c.SaveChanges();
                }
            }
        }
        [HttpGet]
        public ActionResult EditGamePartialView(string id)
        {
            UGames o = new UGames();
            if (id != "")
            {
                int selectedId = Convert.ToInt32(id);
                using (KeptorDBEntities c = new KeptorDBEntities())
                {
                    var d = c.TreasureHuntGames.FirstOrDefault(i => i.Id == selectedId);
                    if (d != null)
                    {
                        o.Finished = d.Finished;
                        o.Name = d.Name;
                        o.Started = d.Started;
                        if (d.Started.HasValue)
                            o.Started = d.Started;
                        else
                            o.Started = null;
                        o.Winner = d.Winner;
                        return PartialView("EditGamePartialView", o);
                    }
                    else
                        return PartialView("EditGamePartialView", o);
                }
            }
            else
                return PartialView("EditGamePartialView", o);
        }
        public void EditGame(UGames cat)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                TreasureHuntGames o = c.TreasureHuntGames.FirstOrDefault(i => i.Id == cat.Id);
                if (o != null)
                {
                    o.Finished = cat.Finished;
                    o.Name = cat.Name;
                    o.Started = cat.Started;
                    if (cat.Started.HasValue)
                        o.Started = cat.Started;
                    else
                        o.Started = null;
                    o.Winner = cat.Winner;
                    c.SaveChanges();
                }
            }
        }
        [HttpGet]
        public ActionResult DeleteGamePartialView()
        {
            return PartialView("DeleteGamePartialView");
        }
        [HttpPost]
        public void DeleteGame(int id)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                TreasureHuntGames o = c.TreasureHuntGames.FirstOrDefault(i => i.Id == id);
                if (o != null)
                {

                    var a1 = c.TreasureHuntParticipants.Where(i=>i.TreasureHuntId==id).ToList();
                    c.TreasureHuntParticipants.RemoveRange(a1);
                    var a2 = c.TreasureHuntTips.Where(i => i.TreasureHuntGamesId == id).ToList();
                    c.TreasureHuntTips.RemoveRange(a2);

                    c.TreasureHuntGames.Remove(o);
                    c.SaveChanges();
                }
            }
        }
        #endregion
        #region Tip CRUD
        public JsonResult TipsDataHandler(DTParameters param)
        {
            try
            {
                using (KeptorDBEntities c = new KeptorDBEntities())
                {
                    int g = -1;
                    if (param.parentId != null && param.parentId != "")
                    g = Convert.ToInt32(param.parentId);

                    var dtsource = c.TreasureHuntTips.Where(i => i.TreasureHuntGamesId == g)
                        .Select(i => new UTips()
                        {
                            Id = i.Id,
                            HFCode = i.HFCode,
                            IsFinal = i.IsFinal.HasValue ? i.IsFinal.Value : false,
                            Keimeno = i.Keimeno,
                            Picture = i.Picture,
                            TakenBy = i.TakenBy.HasValue ? i.TreasureHuntParticipants.Name : "",
                            TakenById = i.TakenBy.HasValue ? i.TakenBy.Value : -1,
                            Titlos = i.Titlos,
                            TreasureHuntId = i.TreasureHuntGamesId,
                            IsFirst = i.IsFirst
                        }).ToList();


                    List<String> columnSearch = new List<string>();

                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                    }

                    List<UTips> data = new TipsResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
                    int count = new TipsResultSet().Count(param.Search.Value, dtsource, columnSearch);
                    DTResult<UTips> result = new DTResult<UTips>
                    {
                        draw = param.Draw,
                        data = data,
                        recordsFiltered = count,
                        recordsTotal = count
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult CreateTipPartialView()
        {
            return PartialView("CreateTipPartialView");
        }
        [HttpPost]
        public void CreateTip(UTips i)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                    TreasureHuntTips o = new TreasureHuntTips();
                    o.HFCode = i.HFCode;
                    o.IsFinal = i.IsFinal;
                o.IsFirst = i.IsFirst;
                o.Keimeno = i.Keimeno!=null?i.Keimeno:"";
                    o.Picture = i.Picture;
                o.Titlos = i.Titlos;                    
                    o.TreasureHuntGamesId = i.TreasureHuntId;
                    c.TreasureHuntTips.Add(o);
                    c.SaveChanges();
                
            }
        }
        [HttpGet]
        public ActionResult EditTipPartialView(string id)
        {
            UTips o = new UTips();
            if (id != "")
            {
                int selectedId = Convert.ToInt32(id);
                using (KeptorDBEntities c = new KeptorDBEntities())
                {
                    var d = c.TreasureHuntTips.FirstOrDefault(i => i.Id == selectedId);
                    if (d != null)
                    {
                        o.HFCode = d.HFCode;
                        o.IsFinal = d.IsFinal;
                        o.IsFirst = d.IsFirst;
                        o.Keimeno = d.Keimeno != null ? d.Keimeno : "";
                        o.Picture = d.Picture;
                        o.Titlos = d.Titlos;
                        o.TakenBy = d.TakenBy.HasValue ? d.TreasureHuntParticipants.Name : "";
                        o.TreasureHuntId = d.TreasureHuntGamesId;
                        return PartialView("EditTipPartialView", o);
                    }
                    else
                        return PartialView("EditTipPartialView", o);
                }
            }
            else
                return PartialView("EditTipPartialView", o);
        }
        public void EditTip(UTips d)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                TreasureHuntTips o = c.TreasureHuntTips.FirstOrDefault(i => i.Id == d.Id);
                if (o != null)
                {
                    o.HFCode = d.HFCode;
                    o.IsFinal = d.IsFinal;
                    o.IsFirst = d.IsFirst;
                    o.Keimeno = d.Keimeno != null ? d.Keimeno : "";
                    o.Picture = d.Picture;
                    o.Titlos = d.Titlos;                    
                    o.TreasureHuntGamesId = d.TreasureHuntId;
                    if (String.IsNullOrEmpty(d.TakenBy))
                        o.TakenBy = null;
                    c.SaveChanges();
                }
            }
        }
        [HttpGet]
        public ActionResult DeleteTipPartialView()
        {
            return PartialView("DeleteTipPartialView");
        }
        [HttpPost]
        public void DeleteTip(int id)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                TreasureHuntTips o = c.TreasureHuntTips.FirstOrDefault(i => i.Id == id);
                if (o != null)
                {
                    c.TreasureHuntTips.Remove(o);
                    c.SaveChanges();
                }
            }
        }
        #endregion
        #region Participants
        public JsonResult ParticipantsDataHandler(DTParameters param)
        {
            try
            {
                using (KeptorDBEntities c = new KeptorDBEntities())
                {
                    int g = -1;
                    if (param.parentId != null && param.parentId != "")
                        g = Convert.ToInt32(param.parentId);

                    var dtsource = c.TreasureHuntParticipants.Where(i => i.TreasureHuntId == g)
                        .Select(i => new UParticipants()
                        {
                            Id = i.Id,
                             Name = i.Name,                           
                            TreasureHuntId = i.TreasureHuntId
                        }).ToList();


                    List<String> columnSearch = new List<string>();

                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                    }

                    List<UParticipants> data = new ParticipantsResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
                    int count = new ParticipantsResultSet().Count(param.Search.Value, dtsource, columnSearch);
                    DTResult<UParticipants> result = new DTResult<UParticipants>
                    {
                        draw = param.Draw,
                        data = data,
                        recordsFiltered = count,
                        recordsTotal = count
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult DeleteParticipantPartialView()
        {
            return PartialView("DeleteParticipantPartialView");
        }
        [HttpPost]
        public void DeleteParticipant(int id)
        {
            using (KeptorDBEntities c = new KeptorDBEntities())
            {
                TreasureHuntParticipants o = c.TreasureHuntParticipants.Include("TreasureHuntGames").Include("TreasureHuntTips").
                    FirstOrDefault(i => i.Id == id);
                if (o != null)
                {                   
                    var a1 = o.TreasureHuntGames;
                    if (a1.Winner.HasValue && a1.Winner.Value == id)
                        a1.Winner = null;
                    var a2 = o.TreasureHuntTips.ToList();
                    foreach(var itm in a2)
                    {
                        if (itm.TakenBy.HasValue && itm.TakenBy.Value == id)
                            itm.TakenBy = null;
                    }
                    c.TreasureHuntParticipants.Remove(o);
                    c.SaveChanges();
                }
            }
        }
        #endregion
    }
}