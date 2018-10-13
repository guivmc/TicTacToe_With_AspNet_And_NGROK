using ApsRedes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ApsRedes.Controllers
{
    public class TicTacToeController : Controller
    {
        public static List<Match> matches { get; set; } = new List<Match>();
        // GET: TicTacToe
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TicTacToeLobby()
        {
            if (this.Session["Player"] != null)
            {
                var player = this.Session["Player"] as Player;

                if (matches.Find(m => m.p1 == null || m.p2 == null) == null)
                {
                    matches.Add(new Match { p1 = player });
                }
                else if (matches.Find(m => m.p1 != null && m.p2 == null) != null)
                {
                    matches.Single(m => m.p1 != null && m.p2 == null).p2 = player;
                }

                if(matches.Find(m => m.p1 != null && m.p2 != null && m.p1.id == m.p2.id) != null)
                {
                    matches.Single(m => m.p1 != null && m.p2 != null && m.p1.id == m.p2.id).p2 = null;
                }

                return View(matches.Single(m => m.p1.id == player.id || m.p2.id == player.id));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}