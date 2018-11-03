using ApsRedes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ApsRedes.Controllers
{
    public class TicTacToeController : Controller
    {
        public static List<Match> matches { get; set; } = new List<Match>();

        // GET: TicTacToe
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TicTacToeLobby()
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                //Check if player is already in a Match
                if( matches.Find( m => m.p1.id == player.id || ( m.p2 != null && m.p2.id == player.id ) ) != null )
                {
                    return View( matches.Single( m => m.p1.id == player.id || m.p2.id == player.id ) );
                }

                //Create a new Match if there is not one open
                if( matches.Find( m => m.p1 == null || m.p2 == null ) == null )
                {
                    player.mark = true;
                    matches.Add( new Match { p1 = player } );
                }
                //If there is an open Match add a second player
                else if( matches.Find( m => m.p1 != null && m.p2 == null ) != null )
                {
                    player.mark = false;
                    matches.Single( m => m.p1 != null && m.p2 == null ).p2 = player;
                }

                //if(matches.Find(m => m.p1 != null && m.p2 != null && m.p1.id == m.p2.id) != null)
                //{
                //    matches.Single(m => m.p1 != null && m.p2 != null && m.p1.id == m.p2.id).p2 = null;
                //}

                return View( matches.Single( m => m.p1.id == player.id || m.p2.id == player.id ) );
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
        }

        [HttpGet]
        public ActionResult Board()
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                var match = matches.Find( m =>( m.p1.id == player.id || m.p2.id == player.id) && m.p1 != null && m.p2 != null );

                if( match != null )
                {
                    //match.board = new int[3, 3];
                    return View(match);
                }
            }
            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }

        [HttpPost]
        public ActionResult PlayTurn(int posX, int posY)
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                var match = matches.Find( m => m.p1.id == player.id || m.p2.id == player.id );

                if( player.mark == match.turn )
                {
                    if( match.board[posX, posY] == 0 )
                    {
                        if( match.turn )
                        {
                            match.board[posX, posY] = 1;
                        }
                        else
                        {
                            match.board[posX, posY] = 2;
                        }
                    }
                    else
                    {
                        return Json( new { invalidPos = true } );
                    }

                    match.roundCounter++;
                    match.turn = !match.turn;

                    if( match.roundCounter <= 9 )
                    {
                        return Json( new { success = true, turn = !match.turn } );
                    }
                    else
                    {
                        return Json( new { over = true } );
                    }
                }
                else
                {
                    return Json( new { error = true } );
                }
            }

            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }
    }
}