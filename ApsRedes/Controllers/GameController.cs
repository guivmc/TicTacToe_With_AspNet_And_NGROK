using ApsRedes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ApsRedes.Controllers
{
    public class GameController : Controller
    {
        public static List<Player> players { get; set; } = new List<Player>();
        public static int id { get; set; } = 0;

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
            if (this.Session["Player"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Lobby", players);
            }
        }

        [HttpPost]
        public ActionResult AddPlayer(Player player)
        {
            if (player.name != null)
            {
                player.mark = (mark = !mark);
                player.id = id;
                players.Add(player);

                this.Session["Player"] = player;


                id++;

                return RedirectToAction("Lobby", players);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Lobby()
        {
            return View(players);
        }
    }
}