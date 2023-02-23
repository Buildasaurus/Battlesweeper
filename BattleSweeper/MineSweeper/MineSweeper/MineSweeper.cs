using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Games.MineSweeper
{

    public class MineSweeper : IMineSweeper
    {
        public Grid<MineSweeperTile>? Tiles => m_grid;

        public HashSet<Point> InitialBombs => throw new NotImplementedException();

        public HashSet<Point> CurrentBombs => throw new NotImplementedException();

        public MoveResult diffuseTile(Point coord)
        {
            throw new NotImplementedException();
        }

        public MoveResult flagTile(Point coord)
        {
            throw new NotImplementedException();
        }

        public void generate(Size size, int bombs, int? seed = null)
        {
            // setup random number generator.

            Random rng;

            if (seed != null)
                rng = new((int)seed);
            else
                rng = new();

            m_grid = new Grid<MineSweeperTile>(size, new(0));

            // place bombs, by generating a random number between 0 and maximum size of grid, and placing a bomb there.
            // if there already exists a bomb, retry a new random number.

            int bombs_left = bombs;

            while(bombs_left > 0)
            {
                int rindx_x = rng.Next(m_grid.Size.Width);
                int rindx_y = rng.Next(m_grid.Size.Height);

                if(m_grid[rindx_x, rindx_y].bomb_count != -1)
                {
                    m_initial_bombs.Add(new Point(rindx_x, rindx_y));
                    m_grid[rindx_x, rindx_y].bomb_count = -1;
                    bombs_left--;
                }
            }

            m_current_bombs = new(m_initial_bombs);

            // generate bomb count of the non bomb tiles.

            for(int x = 0; x < m_grid.Size.Width; x++)
            {
                for(int y = 0; y < m_grid.Size.Height; y++)
                {
                    // skip if the current tile is a bomb.
                    if (m_grid[x, y].bomb_count == -1)
                        continue;

                    // check all tiles in a 3x3 square, around (x, y)
                    
                    for (int dx = x - 1; dx <= x + 1; dx++)
                    {
                        for (int dy = y - 1; dy <= y + 1; dy++)
                        {
                            if(m_grid.inBounds(new Point(dx, dy)) && m_grid[dx, dy].bomb_count == -1)
                            {
                                m_grid[x, y].bomb_count++;
                            }
                        }
                    }

                }
            }

        }

        public MoveResult testTile(Point coord)
        {
            throw new NotImplementedException();
        }

        protected Grid<MineSweeperTile> m_grid;
        protected HashSet<Point> m_initial_bombs = new();
        protected HashSet<Point> m_current_bombs = new();
    }
}
