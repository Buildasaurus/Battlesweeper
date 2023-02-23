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

        public List<Point> InitialBombs => throw new NotImplementedException();

        public List<Point> CurrentBombs => throw new NotImplementedException();

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

            for(int i = 0; i < bombs; i++)
            {
                m_grid[i * 2] = new(-1);
                m_initial_bombs.Add(new Point(i % m_grid.Size.Width, i / m_grid.Size.Width));
                m_current_bombs.Add(new Point(i % m_grid.Size.Width, i / m_grid.Size.Width));
            }

            for(int i = 0; i < m_grid.Size.Width * m_grid.Size.Height; i++)
            {
                if (m_grid[i].bomb_count != -1)
                    m_grid[i] = new(i % 10);
            }
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
