using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.MineSweeper
{
    /// <summary>
    /// use place, setInitialBombs and setCurrentBombs to configure a simple IMineSweeper instance,
    /// that can be used to develop the UI.
    /// </summary>
    public class MineSweeperTemplate : IMineSweeper
    {
        public Grid<MineSweeperTile>? Tiles => m_grid;

        public HashSet<Point> InitialBombs => m_initial_bombs;

        public HashSet<Point> CurrentBombs => m_current_bombs;

        /// <summary>
        /// sets the MineSweeperTile.is_revealed to true, at coord.
        /// </summary>
        /// <param name="coord"> MoveResult.Success</param>
        public MoveResult diffuseTile(Point coord)
        {
            Tiles![coord].is_revealed = true;

            return MoveResult.Success;
        }


        /// <summary>
        /// sets the MineSweeperTile.is_flagged to true, at coord.
        /// </summary>
        /// <param name="coord"> MoveResult.Success </param>
        public MoveResult flagTile(Point coord)
        {
            Tiles![coord].is_flagged = true;
            return MoveResult.Success;
        }

        /// <summary>
        /// sets the MineSweeperTile.is_revealed to true, at coord.
        /// </summary>
        /// <param name="coord"> MoveResult.Success </param>
        public MoveResult testTile(Point coord)
        {
            Tiles![coord].is_revealed = true;
            return MoveResult.Success;
        }

        /// <summary>
        /// bombs are ignored here, use place() in order to place bombs.
        /// seed is also ignored, as there is no random generator used, in this implementation.
        /// </summary>
        public void generate(Size size, int bombs, int? seed)
        {
            m_grid = new(size, new(0));
        }

        /// <summary>
        /// sets the MineSweeperTile at coord, to tile
        /// </summary>
        public void place(Point coord, MineSweeperTile tile)
        {
            m_grid[coord] = tile.Copy();
        }

        /// <summary>
        /// set the initial bomb hash set data.
        /// </summary>
        public void setInitialBombs(HashSet<Point> bombs)
        {
            m_initial_bombs = bombs;
        }

        /// <summary>
        /// set the current bomb hash set data.
        /// </summary>
        public void setCurrentBombs(HashSet<Point> bombs)
        {
            m_current_bombs = bombs;
        }

        protected Grid<MineSweeperTile>? m_grid;
        protected HashSet<Point> m_initial_bombs = new();
        protected HashSet<Point> m_current_bombs = new();
    }
}
