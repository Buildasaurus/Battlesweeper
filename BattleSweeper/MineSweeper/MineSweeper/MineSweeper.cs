using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Games.MineSweeper
{
    /// <summary>
    /// controls a single mine sweeper game.
    /// GameOver is invoked, when the game is lost.
    /// 
    /// a new feature has been added to this version:
    /// bombs can now be defused, which in this game, means they will be no longer be added
    /// to the battleships grid.
    /// 
    /// </summary>
    public class MineSweeper : IMineSweeper
    {
        /// <summary>
        /// the current mine sweeper grid.
        /// </summary>
        public Grid<MSTile>? Tiles => m_grid;
        /// <summary>
        /// the starting bomb positions.
        /// </summary>
        public HashSet<Point> InitialBombs => m_initial_bombs;
        /// <summary>
        /// the bomb positions that have not yet been defused.
        /// </summary>

        public HashSet<Point> CurrentBombs => m_current_bombs;
        /// <summary>
        /// is invoked every time a tile is changed in the game.
        /// the tile position is passed as well in the event.
        /// </summary>
        public Action<Point>? TileChanged { get; set; }

        /// <summary>
        /// attempts to defuse the tile at the given tile coordinate.
        /// </summary>
        public MoveResult defuseTile(Point coord)
        {
            // the tile must not be revealed, and must contain a bomb before it can be defused.
            if (Tiles[coord].is_revealed)
                return MoveResult.Illegal;

            MoveResult result;

            if (Tiles[coord].bomb_count == IMineSweeper.BOMB)
            {
                Tiles[coord].is_revealed = true;
                Tiles[coord].is_defused = true;

                m_current_bombs.Remove(coord);
                result = MoveResult.Success;
            }
            else
            {
                Tiles[coord].is_revealed = true;
                Tiles[coord].is_flagged = false;
                result = MoveResult.Failure;
            }

            TileChanged?.Invoke(coord);

            return result;
        }

        /// <summary>
        /// attempts to flag the tile at the given position.
        /// flagged tiles can not be tested.
        /// </summary>
        public MoveResult flagTile(Point coord)
        {
            // cannot flag tile, which has been revealed.
            if (Tiles[coord].is_revealed)
                return MoveResult.Failure;

            Tiles[coord].is_flagged = !Tiles[coord].is_flagged;
            TileChanged?.Invoke(coord);


            return MoveResult.Success;
        }
        
        /// <summary>
        /// attempt to test / reveal the tile at the given coord.
        /// if any tiles next to the current tile, in a 1 tile raius, contains a bomb count of 0,
        /// they should be revealed as well.
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public MoveResult testTile(Point coord)
        {
            // stop condition: If the tile has already been revealed or it has been falgged, do nothing.
            if (Tiles![coord].is_revealed || Tiles![coord].is_flagged)
                return MoveResult.Illegal;

            Tiles[coord].is_revealed = true;

            // make sure to notify a potential viewmodel, about a tile change.
            TileChanged?.Invoke(coord);

            // if the tile is a bomb, or has a bomb count greater than 0, stop the revealing process.
            if (Tiles[coord].bomb_count > 0)
                return MoveResult.Success;
            else if (Tiles[coord].bomb_count == IMineSweeper.BOMB)
                return MoveResult.Failure;

            // recursive testTile call in a 3 x 3 square around the current tile.

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    Point delta_coord = new(coord.X + dx, coord.Y + dy);

                    // make sure the testTile is not called on the samme coords,
                    // and the delta coords is not out of bounds for the tile grid..
                    if (!(dx == 0 && dy == 0) && Tiles.inBounds(delta_coord))
                        testTile(delta_coord);
                }
            }

            return MoveResult.Success;
        }
        /// <summary>
        /// Generates a MineSweeper instance, with the specified grid size, and bomb count.
        /// if no seed is passed, a random seed is used for placing the bombs.
        /// </summary>
        public void generate(Size size, int bombs, int? seed = null)
        {
            // setup random number generator.

            Random rng;

            if (seed != null)
                rng = new((int)seed);
            else
                rng = new();

            m_grid = new Grid<MSTile>(size, new(0));

            // place bombs, by generating a random number between 0 and maximum size of grid, and placing a bomb there.
            // if there already exists a bomb, retry a new random number.

            int bombs_left = bombs;

            while(bombs_left > 0)
            {
                int rindx_x = rng.Next(m_grid.Size.Width);
                int rindx_y = rng.Next(m_grid.Size.Height);

                // only place bomb, if there has not been placed a bomb already.
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

        protected Grid<MSTile> m_grid;
        protected HashSet<Point> m_initial_bombs = new();
        protected HashSet<Point> m_current_bombs = new();
    }
}
