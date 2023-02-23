using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.MineSweeper
{
    public class MineSweeperTemplate : IMineSweeper
    {
        public Grid<MineSweeperTile>? Tiles => m_grid;

        public HashSet<Point> InitialBombs => throw new NotImplementedException();

        public HashSet<Point> CurrentBombs => throw new NotImplementedException();

        public MoveResult diffuseTile(Point coord)
        {
            Tiles![coord].is_revealed = true;

            if (Tiles[coord].bomb_count == -1)
            {
                m_current_bombs.Remove(coord);
            }

            return MoveResult.Success;
        }

        public MoveResult flagTile(Point coord)
        {
            Tiles![coord].is_flagged = true;
            return MoveResult.Success;
        }

        public void generate(Size size, int bombs)
        {
            m_grid = new(size, new(0));
        }

        public void place(Point coord, MineSweeperTile tile)
        {
            m_grid[coord] = tile.Copy();
        }

        public void setInitialBombs(HashSet<Point> bombs)
        {
            m_initial_bombs = bombs;
        }

        public void setCurrentBombs(HashSet<Point> bombs)
        {
            m_current_bombs = bombs;
        }

        public MoveResult testTile(Point coord)
        {
            Tiles![coord].is_revealed = true;
            return MoveResult.Success;
        }

        protected Grid<MineSweeperTile>? m_grid;
        protected HashSet<Point> m_initial_bombs = new();
        protected HashSet<Point> m_current_bombs = new();
    }
}
