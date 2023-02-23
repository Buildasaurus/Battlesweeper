using System.Drawing;

namespace Games.MineSweeper
{
    public class MineSweeperTile : ICopyable<MineSweeperTile>
    {
        public MineSweeperTile(int bomb_count)
        {
            this.bomb_count = bomb_count;
        }


        // -1 = bomb
        public int bomb_count { get; init; }
        public bool is_revealed;
        public bool is_flagged;

        public MineSweeperTile Copy()
        {
            MineSweeperTile tile = new(bomb_count);
            tile.is_revealed = is_revealed;
            tile.is_flagged = is_flagged;
            
            return tile;
        }
    }
    public interface IMineSweeper
    {
        public Grid<MineSweeperTile>? Tiles { get; }
        public List<Point> InitialBombs { get; }
        public List<Point> CurrentBombs { get; }


        public void generate(Size size, int bombs);
        public MoveResult testTile(Point coord);
        public MoveResult flagTile(Point coord);
        public MoveResult diffuseTile(Point coord);
    }
}