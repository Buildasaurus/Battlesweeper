﻿using System;
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
        public void placeShipTest()
        {

            Battleships game = new Battleships();
            List<Point> bombPositions = new List<Point>();
            game.constructBoard(bombPositions, new List<int>() { 4, 3, 2, 2, 2 });
            game.placeShip(new(1,1),false);
            game.placeShip(new(8, 8), true);
            game.placeShip(new(4, 5), false);
            game.placeShip(new(5, 1), false);
            game.placeShip(new(3, 7), true);

            /*Assert.True(game.Tiles[1, 1].ship == 0);
            Assert.True(game.Tiles[2, 1].ship == 0);
            Assert.True(game.Tiles[3, 1].ship == 0);
            Assert.True(game.Tiles[4, 1].ship == 0);
            Assert.True(game.Tiles[5, 1].ship == -1);
            Assert.True(game.Tiles[1, 2].ship == 1);
            Assert.True(game.Tiles[1, 2].atStart == true);
            Assert.True(game.Tiles[1, 3].ship == 1);
            Assert.True(game.Tiles[1, 4].ship == 1);
            Assert.True(game.Tiles[1, 5].ship == -1);
            Assert.True(game.Tiles[1, 6].ship == -1);
            Assert.True(game.Tiles[4, 5].ship == 2);
            */
            Assert.True(game.Tiles[8, 8].ship == -1);
        }

        [Fact]
        public void shootTest()
        {
            Battleships game = new Battleships();
            List<Point> bombPosition = new List<Point>();
            game.constructBoard(bombPosition, new List<int>() { 1 });
            game.placeShip(new(1, 1), true);
            bool ShipSunkInvoked = false;
            game.ShipSunk += (s, sunk) => { Assert.True(sunk == 0); ShipSunkInvoked = true; };
            game.shoot(new(2, 1));
            game.shoot(new(1, 1));
            game.shoot(new(1, 2));
            game.shoot(new(1, 3));
            game.shoot(new(1, 4));
            

            
            Assert.True(game.Tiles[1, 2].hasBeenShot == true);
            Assert.True(game.Tiles[1, 1].hasBeenShot == true);
            Assert.True(game.Tiles[1, 2].hasBeenShot == true);
            Assert.True(game.Tiles[1, 3].hasBeenShot == true);
            Assert.True(game.Tiles[1, 4].hasBeenShot == true);
            Assert.True(game.Tiles[1, 4].ship == 0);
            Assert.True(ShipSunkInvoked == true);
            Assert.True(game.Tiles[2, 1].hasBeenShot == true);
        }



        
        [Fact]
        public void checkWinTest()
        {
            Battleships game = new Battleships();
            List<Point> bombPosition = new List<Point>();
            game.constructBoard(bombPosition, new List<int>() { 4});
            bool GameOverInvoked = false;
            game.GameOver += (s, won) => { Assert.True(won); GameOverInvoked = true; };
            game.placeShip(new(1, 1), false);

            game.shoot(new(1,1));
            game.shoot(new(2, 1));
            game.shoot(new(3, 1));
            game.shoot(new(4, 1));

            Assert.True(GameOverInvoked);
        }
        

    }
}
