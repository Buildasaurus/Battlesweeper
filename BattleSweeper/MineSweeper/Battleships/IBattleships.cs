using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Games.Battleships
{
    public class BattleshipTile:ICopyable<BattleshipTile>
    {
        /// <summary>
        /// Determines the number of a ship, to distinguish the ships and know when a ship is sunken
        /// </summary>
        public int ship;
        
        /// <summary>
        /// Indicates wether a tile has been shot before to determine illegal shots
        /// </summary>
        public bool hasBeenShot;
        
        /// <summary>
        /// Indicates wether the tile has a bomb
        /// </summary>
        public bool hasBomb;
        public BattleshipTile(int Ship, bool hasBeenShot, bool hasBomb)
        {
            this.Ship = Ship;
            this.hasBeenShot = hasBeenShot;
            this.hasBomb = hasBomb;
        }
        public BattleshipTile Copy()
        {
            return new BattleshipTile(Ship,hasBeenShot,hasBomb);
        }

    }
    public interface IBattleships
    {
        /// <summary>
        /// Allows for game to retrive grid
        /// </summary>
        public Grid<BattleshipTile>? Tiles { get; }
        /// <summary>
        /// List of ship lengths, for placing different ships, and distinguishing them
        /// </summary>
        public List<int> shipLengths { get; }
        /// <summary>
        /// Lists the remaining pieces of each ship
        /// </summary>
        public List<int> remainingPieces { get; }
        /// <summary>
        /// Checks the result of a shot and updates tiles accordingly
        /// </summary>
        /// <returns></returns>
        public MoveResult Shoot(Point coord);
        /// <summary>
        /// Places the bombs from minesweeper on the grid 
        /// </summary>
        /// <param name="bombCoords">list fo coordinates with all bombs</param>
        public void placeBomb(List<Point> bombCoords);
        /// <summary>
        /// Constructs the board based on a size. The board is always quadratic
        /// </summary>
        /// <param name="size"></param>
        public void constructBoard(Size size);
        /// <summary>
        /// Checks if a player wins, based on the List remainingPieces
        /// </summary>
        /// <returns>If active player wins the game from the shot</returns>
        public bool checkWin();
        /// <summary>
        /// Used for placing ships when the game is stated
        /// </summary>
        /// <param name="coord">coord for ship placement</param>
        /// <param name="vertical">If direction of ship is vertical</param>
        /// <returns></returns>
        public bool placeShip(Point coord,bool vertical);
    
    }
}
