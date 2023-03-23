using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{
    internal class BattleshipsTemplate
    {
        public EventHandler<int>? ShipSunk;
        public EventHandler<bool>? GameOver;
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        Grid<BattleshipTile> Board;
        List<int> shipLengths = new List<int>();
        
        public void setTile(Point coord, BattleshipTile Tile)
        {
            Board[coord] = Tile.Copy();
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


            Board = new Grid<BattleshipTile>(new System.Drawing.Size(10, 10));
            shipLengths = remainingPieces.GetRange(0, remainingPieces.Count);

            foreach (Point bomb in bombPositions)
            {

                Board[bomb].hasBomb = true;

            }

        }
    }



}



