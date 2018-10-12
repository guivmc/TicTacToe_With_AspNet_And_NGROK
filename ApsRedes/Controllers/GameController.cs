using ApsRedes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsRedes.Controllers
{
    public class GameController : Controller
    {
        public static List<Player> players { get; set; } = new List<Player>();

        public static bool mark { get; set; } = false;

        // GET: Game
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddPlayer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPlayer(Player player)
        {           
            player.mark = (mark = !mark);
            players.Add(player);

            if (this.Session["Player"] == null) this.Session["Player"] = player;

            return View("Lobby", players);
        }

        [HttpGet]
        public ActionResult Lobby()
        {
            return View(players);
        }
    }
}