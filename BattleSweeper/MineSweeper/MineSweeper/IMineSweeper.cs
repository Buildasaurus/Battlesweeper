using System.Drawing;

namespace Games.MineSweeper
{
    /// <summary>
    /// represents a tile in a minesweeper grid.
    /// 
    /// stores the current bomb count (or if the tile is a bomb)
    /// the revealed status and the flag status.
    /// 
    /// </summary>
    public class MineSweeperTile : ICopyable<MineSweeperTile>
    {
        /// <summary>
        /// construct MineSweeperTile and set the bomb_count to the passed parameter.
        /// </summary>
        /// <param name="bomb_count"></param>
        public MineSweeperTile(int bomb_count)
        {
            this.bomb_count = bomb_count;
        }


       /// <summary>
       /// the number of bombs within a 1 tile radius, of the current tile.
       /// -1 == the current tile is a bomb.
       /// </summary>
        public int bomb_count { get; set; }
        /// <summary>
        /// if true, the tile should be rendered as a revealed tile, and the bomb_count should be displayed to the user.
        /// </summary>
        public bool is_revealed;
        /// <summary>
        /// if true, the tile should not be able to be clicked.
        /// </summary>
        public bool is_flagged;

        public MineSweeperTile Copy()
        {
            MineSweeperTile tile = new(bomb_count);
            tile.is_revealed = is_revealed;
            tile.is_flagged = is_flagged;
            
            return tile;
        }
    }

    public static class MineSweeperFactory
    {
        public static T construct<T>(Size size, int bombs, int? seed = null) where T : IMineSweeper, new()
        {
            T game = new();
            game.generate(size, bombs, seed);
            return game;
        }
    }

    /// <summary>
    /// interface of a MineSweeper implementation.
    /// 
    /// exposes all the required fields, in order to implement a complete diffusable MineSweeper implementation.
    /// 
    /// </summary>
    public interface IMineSweeper
    {
        public const int BOMB = -1;
        /// <summary>
        /// grid of tiles in the current minesweeper game.
        /// </summary>
        public Grid<MineSweeperTile>? Tiles { get; }
        /// <summary>
        /// list of the initial bomb placements.
        /// </summary>
        public HashSet<Point> InitialBombs { get; }
        /// <summary>
        /// list of the current bombs, that have not yet be diffused.
        /// </summary>
        public HashSet<Point> CurrentBombs { get; }

        /// <summary>
        /// initialize a minesweeper game, of the specified size, and a specified bomb count.
        /// </summary>
        /// <param name="size"> must be greater than 0 X 0</param>
        /// <param name="bombs"> muse be greater than -1 </param>
        /// <param name="seed"> if null, a random number generator must be supplied with a random seed. </param>
        public void generate(Size size, int bombs, int? seed = null);
        /// <summary>
        /// attempts to reveal the tile at the specified coord.
        /// </summary>
        public MoveResult testTile(Point coord);
        /// <summary>
        /// attempts to flag a tile at the specified coord.
        /// </summary>
        public MoveResult flagTile(Point coord);
        /// <summary>
        /// attempts to diffuse a bomt at the specified coord.
        /// </summary>
        public MoveResult diffuseTile(Point coord);
    }
}