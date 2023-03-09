using System;
using System.Collections.Generic;
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

        public Battleships() {

            Grid<BattleshipTile> Board = new Grid<BattleshipTile>(new System.Drawing.Size(10,10));
            shipLengths = remainingPieces.GetRange(0,remainingPieces.Count);

            

        }   
    }
#endif
    

}
