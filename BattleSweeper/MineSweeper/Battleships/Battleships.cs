using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{

    public class Battleships
    {
        
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });
        Grid<BattleshipTile> Board; 
        List<int> shipLengths = new List<int>();
        int n = 0;
        public MoveResult placeShip(Point point, bool Vertical)
        {
            for (int i = 1; i < shipLengths[n]; i++) {

                if (Vertical == true) { 

                    if (Board[point.Y+i,point.X].ship > 1) {
                    return MoveResult.Illegal;
                    }
                }
                else {
                    if (Board[point.Y, point.X+i].ship > 0) {
                        return MoveResult.Illegal;
                    }
                }
            }
           

            n++;
            return MoveResult.Success;
        }
        public void constructBoard(List<Point> bombPositions) {
           

            Board = new Grid<BattleshipTile>(new System.Drawing.Size(10,10));
            shipLengths = remainingPieces.GetRange(0,remainingPieces.Count);

            foreach (Point bomb in bombPositions)
            {

                Board[bomb].hasBomb = true;

            }

        }   
    }

    

}
