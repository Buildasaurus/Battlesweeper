using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{
#if false
    public class Battleships
    {
        
        List<int> remainingPieces = new List<int>(new int[] { 4, 3, 2, 2, 2 });

        List<int> shipLengths = new List<int>();
        int n = 0;
        public MoveResult placeShip(Point point, bool Vertical)
        {
            for (int i = 1; i < shipLengths[n]; i++)
            {
                if (Board[point].ship > 1)
                {
                    return MoveResult.Illegal;
                }
            }
           

            n++;
            return MoveResult.Success;
        }
        public void constructBoard(List<Point> bombPositions) {
           

            Grid<BattleshipTile> Board = new Grid<BattleshipTile>(new System.Drawing.Size(10,10));
            shipLengths = remainingPieces.GetRange(0,remainingPieces.Count);

            foreach (Point bomb in bombPositions)
            {

                Board[bomb].hasBomb = true;

            }

        }   
    }
#endif
    

}
