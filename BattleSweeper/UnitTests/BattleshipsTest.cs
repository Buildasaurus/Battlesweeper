using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Games.Battleships;
using Games;



namespace Tests
{
    public class BattleshipsTest
    {
        [Fact]
        public MoveResult placeShipTest()
        {
            Battleships game = new Battleships();
            List<Point> bombPosition = new List<Point>();
            game.constructBoard(bombPositions);
            game.placeShip(Tiles[1,1],false);
            game.placeShip(Tiles[1, 2], true);
            game.placeShip(Tiles[4, 5], false);
            game.placeShip(Tiles[8, 8], false);
            game.placeShip(Tiles[3, 7], true);

            Assert.True(Tiles[1, 1].ship = 0);
            Assert.True(Tiles[2, 1].ship = 0);
            Assert.True(Tiles[3, 1].ship = 0);
            Assert.True(Tiles[4, 1].ship = 0);
            Assert.True(Tiles[5, 1].ship = -1);

            Assert.True(Tiles[1, 2].ship = 1);
            Assert.True(Tiles[1, 2].atEnd = true);
            Assert.True(Tiles[1, 3].ship = 1);
            Assert.True(Tiles[1, 4].ship = 1);
            Assert.True(Tiles[1, 5].ship = 1);
            Assert.True(Tiles[1, 6].ship = 0);

            Assert.True(Tiles[4, 5].ship = 2);

        }

        /*[Fact]
        public void shootTest()
        {
            Battleships game = new Battleships();
            List<Point> bombPosition = new List<Point>();
            game.constructBoard(bombPositions);
            bool GameOverInvoked = false;
            game.GameOver += (s, won) => { Assert.True(won); GameOverInvoked = true; };





            Assert.True(GameOverInvoked);
        }
        



        [Fact]
        public void checkWinTest()
        {
            Battleships game = new Battleships();
            List<Point> bombPosition = new List<Point>();
            game.constructBoard(bombPositions);
            bool GameOverInvoked = false;
            game.GameOver += (s, won) => { Assert.True(won); GameOverInvoked = true; };





            Assert.True(GameOverInvoked);
        }
        */

    }
}
