using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Games.Battleships
{
    public class BattleshipsTemplate : IBattleships
    {
        public EventHandler<int>? ShipSunk { get; set; }
		public EventHandler<bool>? GameOver { get; set; }
		public List<int> remainingPieces { get; set; } = new List<int>() { 4, 3, 2, 2, 2 };
        public Grid<BattleshipTile> Tiles { get; set; }
        public List<int> shipLengths { get; set; } = new List<int>();

        public Action<Point>? TileChanged { get; set; }

        public void setTile(Point coord, BattleshipTile Tile)
        {
            Tiles[coord] = Tile.Copy();
            TileChanged?.Invoke(coord);
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
        public void constructBoard(List<Point> bombPositions, List<int> shipLengths)
        {
            Tiles = new Grid<BattleshipTile>(new System.Drawing.Size(10, 10), new(-1, false, false, false, false, false));
			this.shipLengths = shipLengths;
			this.remainingPieces = shipLengths;

			foreach (Point bomb in bombPositions)
            {

                Tiles[bomb].hasBomb = true;

            }


        }
    }}



