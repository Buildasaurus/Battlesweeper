using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Games.MineSweeper;
using ReactiveUI;
using Games;
using Point = System.Drawing.Point;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using System.Threading;

namespace BattleSweeper.ViewModels
{
    public class MSTileVM : ReactiveObject, ICopyable<MSTileVM>
    {
        public static Dictionary<string, Bitmap> Sprites { get; set; } = new();

        public static void loadSprites()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            if (assets != null)
            {
                string[] sprites = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "Bomb", "Empty", "Tile", "Flag" };

                foreach (string sprite in sprites)
                {
                    Sprites[sprite] = new Bitmap(assets.Open(new($"avares://BattleSweeper/Assets/MineSweeper{sprite}.png")));
                }
            }
        }

        public MSTileVM Copy() => new(Tile, Coord);

        public MSTileVM(MineSweeperTile tile, System.Drawing.Point coord)
        {
            if (Sprites.Count == 0)
                loadSprites();

            Tile = tile;

            Coord = coord;
        }

        public Bitmap Sprite
        {
            get
            {
                if (Tile.is_flagged)
                    return Sprites["Flag"];

                if (!Tile.is_revealed)
                    return Sprites["Tile"];

                if (Tile.bomb_count == 0)
                    return Sprites["Empty"];

                if (Tile.bomb_count == IMineSweeper.BOMB)
                    return Sprites["Bomb"];

                return Sprites[Tile.bomb_count.ToString()];
            }
        }

        public System.Drawing.Point Coord { get; set; }

        public MineSweeperTile Tile { get; set; }
    }

    /// <summary>
    /// class responsible for handling user interaction with a IMineSweeper instance,
    /// as well as managing a grid of MSTileVM, and making sure their properties are synchronized with the given model.
    /// </summary>
    public class MineSweeperViewModel : ViewModelBase
    {
        /// <summary>
        /// called when the user has run out of time.
        /// 
        /// gives true if all bombs were diffused, otherwise false.
        /// 
        /// </summary>
        public EventHandler<bool>? GameOver;

        /// <summary>
        /// grid of MineSweeper Tile ViewModels, that represent the current mine_sweeper game.
        /// </summary>
        public Grid<MSTileVM> grid;
        /// <summary>
        /// the minesweeper instance, the current viewmodel will display.
        /// </summary>
        public IMineSweeper mine_sweeper;

        public int TimeLeft { get => m_time_left; protected set => this.RaiseAndSetIfChanged(ref m_time_left, value); }

        /// <summary>
        /// construct a MineSweeperViewModel, that will display the passed IMineSweeper model.
        /// </summary>
        public MineSweeperViewModel(IMineSweeper _mine_sweeper, int time_span)
        {
            TimeLeft = time_span;
            mine_sweeper = _mine_sweeper;
            timer_thread = new(timerThread);

            grid = new(mine_sweeper.Tiles.Size);

            for (int x = 0; x < mine_sweeper.Tiles.Size.Width; x++)
            {
                for (int y = 0; y < mine_sweeper.Tiles.Size.Height; y++)
                {
                    grid[x, y] = new(mine_sweeper.Tiles[x, y], new Point(x, y));
                }
            }

            mine_sweeper.TileChanged += (coord) => grid[coord].RaisePropertyChanged(nameof(MSTileVM.Sprite));
        }

        /// <summary>
        /// should be called when the game is started.
        /// initializes a timer thread, that keeps track of how much time the user has left.
        /// </summary>
        public void start() => timer_thread.Start();


        public void leftClickTile(Point tile)
        {
            if (mine_sweeper.testTile(tile) == MoveResult.Failure)
            {
                GameOver?.Invoke(this, false);
            }
        }


        public void leftShiftClickTile(Point tile)
        {
            var result = mine_sweeper.diffuseTile(tile);

            switch(result)
            {
                case MoveResult.Failure:
                    TimeLeft -= 10;
                    break;
                case MoveResult.Success:
                    if (mine_sweeper.CurrentBombs.Count <= 0)
                        GameOver?.Invoke(this, true);
                    break;
            }
        }


        public void rightClickTile(Point tile)
        {
            mine_sweeper.flagTile(tile);
        }

        protected int m_time_left;
        protected Thread timer_thread;

        protected void timerThread()
        {
            while(TimeLeft > 0)
            {
                Thread.Sleep(1000);
                TimeLeft--;
            }

            GameOver?.Invoke(this, false);
        }

    }
}
