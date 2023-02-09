using System.Drawing;

namespace Games.MineSweeper
{
    public class MineSweeperTile
    {
        public MineSweeperTile(int bomb_count)
        {
            this.bomb_count = bomb_count;
        }


        // -1 = bomb
        public int bomb_count { get; init; }
        public bool is_revealed;
        public bool is_flagged;
    };

    public enum MineSweeperResult
    {
        Success,
        Failure,
        Illegal,
    }

    public interface IMineSweeper
    {
        public Grid<MineSweeperTile> Tiles { get; }
        public List<Point> InitialBombs { get; }
        public List<Point> CurrentBombs { get; }


        public void generate(Size size, int bombs);
        public MineSweeperResult testTile(Point coord);
        public MineSweeperResult flagTile(Point coord);
        public MineSweeperResult diffuseTile(Point coord);
    }
}