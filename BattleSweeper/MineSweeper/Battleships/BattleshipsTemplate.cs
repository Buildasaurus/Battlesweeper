using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{
    public class BattleshipsTemplate:IBattleships
    {
        public EventHandler<int>? ShipSunk;
        public EventHandler<bool>? GameOver;
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        Grid<BattleshipTile> Tiles;
        List<int> shipLengths = new List<int>();

        public Grid<BattleshipTile>? Tiles => Tiles;

        List<int> IBattleships.shipLengths => shipLengths;

        List<int> IBattleships.remainingPieces => remainingPieces;

        public void setTile(Point coord, BattleshipTile Tile)
        {
            Tiles[coord] = Tile.Copy();
        }
        public MoveResult shoot(Point coord)
        {
                return MoveResult.Failure;
        }
        public bool shootExecution(Point coord)
        {
            return true;
        }

        public MoveResult placeShip(Point coord, bool Vertical)
        { 
        return MoveResult.Success;
        }

        public void checkWin()
        {

        }
        public void constructBoard(List<Point> bombPositions)
        {


            Tiles = new Grid<BattleshipTile>(new System.Drawing.Size(10, 10));
            shipLengths = remainingPieces.GetRange(0, remainingPieces.Count);

            foreach (Point bomb in bombPositions)
            {

                Tiles[bomb].hasBomb = true;

            }

        }








}



