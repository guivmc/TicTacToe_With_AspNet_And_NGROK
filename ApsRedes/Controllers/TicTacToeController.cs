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

                return View( matches.Single( m => m.p1.id == player.id || m.p2.id == player.id ) );
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
        }

        [HttpGet]
        public ActionResult DrawnBoard()
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                var match = matches.Single( m => ( m.p1.id == player.id || m.p2.id == player.id ) && m.p1 != null && m.p2 != null );

                var pos1 = match.board[0, 0];
                var pos2 = match.board[0, 1];
                var pos3 = match.board[0, 2];
                var pos4 = match.board[1, 0];
                var pos5 = match.board[1, 1];
                var pos6 = match.board[1, 2];
                var pos7 = match.board[2, 0];
                var pos8 = match.board[2, 1];
                var pos9 = match.board[2, 2];

                return Json( new { pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9 }, JsonRequestBehavior.AllowGet );
            }
            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }

        [HttpGet]
        public ActionResult Board()
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                var match = matches.Single( m => ( m.p1.id == player.id || m.p2.id == player.id ) && m.p1 != null && m.p2 != null );

                if( match != null )
                {
                    return View( match );
                }
            }
            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }

        [HttpPost]
        public ActionResult PlayTurn( int posX, int posY )
        {
            if( this.Session["Player"] != null )
            {
                var player = this.Session["Player"] as Player;

                var match = matches.Single( m => m.p1.id == player.id || m.p2.id == player.id );

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
                        return Json( new { invalidPos = true, turn = match.turn } );
                    }

                    int win1 = 0;
                    int win2 = 0;

                    //check col
                    for( int i = 0; i < 3; i++ )
                    {
                        if( match.board[posX, i] == 0 )
                            break;
                        else
                        {
                            if( match.board[posX, i] == 1 )
                            {
                                win1++;
                            }
                            else if( match.board[posX, i] == 2 )
                            {
                                win2++;
                            }
                        }
                        if( i == 3 - 1 )
                        {
                            //report win for s
                            if( win1 > 2 || win2 > 2 )
                            {
                                return Json( new { success = true, over = true, turn = match.turn } );
                            }
                        }
                    }

                    win1 = 0;
                    win2 = 0;

                    //check row
                    for( int i = 0; i < 3; i++ )
                    {
                        if( match.board[i, posY] == 0 )
                            break;
                        else
                        {
                            if( match.board[i, posY] == 1 )
                            {
                                win1++;
                            }
                            else if( match.board[i, posY] == 2 )
                            {
                                win2++;
                            }
                        }
                        if( i == 3 - 1 )
                        {
                            //report win for s
                            if( win1 > 2 || win2 > 2 )
                            {
                                return Json( new { success = true, over = true, turn = match.turn } );
                            }
                        }
                    }

                    win1 = 0;
                    win2 = 0;

                    //check diag
                    if( posX == posY )
                    {
                        //we're on a diagonal
                        for( int i = 0; i < 3; i++ )
                        {
                            if( match.board[i, i] == 0 )
                                break;
                            else
                            {
                                if( match.board[i, i] == 1 )
                                {
                                    win1++;
                                }
                                else if( match.board[i, i] == 2 )
                                {
                                    win2++;
                                }
                            }
                            if( i == 3 - 1 )
                            {
                                //report win for s
                                if( win1 > 2 || win2 > 2 )
                                {
                                    return Json( new { success = true, over = true, turn = match.turn } );
                                }
                            }
                        }
                    }

                    win1 = 0;
                    win2 = 0;

                    //check anti diag (thanks rampion)
                    if( posX + posY == 3 - 1 )
                    {
                        for( int i = 0; i < 3; i++ )
                        {
                            if( match.board[i, ( 3 - 1 ) - i] == 0 )
                                break;
                            else
                            {

                                if( match.board[i, ( 3 - 1 ) - i] == 1 )
                                {
                                    win1++;
                                }
                                else if( match.board[i, ( 3 - 1 ) - i] == 2 )
                                {
                                    win2++;
                                }
                            }
                            if( i == 3 - 1 )
                            {
                                //report win for s
                                if( win1 > 2 || win2 > 2 )
                                {
                                    return Json( new { success = true, over = true, turn = match.turn } );
                                }
                            }
                        }
                    }

                    if( match.roundCounter <= 8 )
                    {
                        match.roundCounter++;
                        match.turn = !match.turn;
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