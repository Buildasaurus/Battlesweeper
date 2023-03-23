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
using System.Diagnostics;
using Avalonia.VisualTree;

namespace BattleSweeper.ViewModels
{
    /// <summary>
    /// Mine Sweeper Tile view model
    /// handles loading of tile sprites, as well as selecting the correct sprite, depending on the tile state.
    /// </summary>
    public class MSTileVM : ReactiveObject, ICopyable<MSTileVM>
    {
        /// <summary>
        /// dictionary mapping the tile state name, to a specific sprite.
        /// possible state names can be seen in the loadSprites function.
        /// </summary>
        public static Dictionary<string, Bitmap> Sprites { get; set; } = new();

        /// <summary>
        /// loads all the tiles sprites into memory.
        /// should only be called once per program run, as this loads the sprites into a static property, which all instances of this class can access.
        /// </summary>
        public static void loadSprites()
        {
            // retrieve the service, which will allow us to load avalonia resource files, which the sprite files are stored as.
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            if (assets != null)
            {
                // The states are mapped to the suffix of the file name (MineSweeper[STATE].png)
                string[] sprites = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "Bomb", "Empty", "Tile", "Flag", "Diffused" };

                foreach (string sprite in sprites)
                {
                    Sprites[sprite] = new Bitmap(assets.Open(new($"avares://BattleSweeper/Assets/MineSweeper{sprite}.png")));
                }
            }
        }

        public MSTileVM Copy() => new(Tile);

        /// <summary>
        /// constructs a tile viewmodel from the given tile model.
        /// </summary>
        public MSTileVM(MSTile tile)
        {
            if (Sprites.Count == 0)
                loadSprites();

            Tile = tile;
        }

        /// <summary>
        /// Property representing the sprite that should be displayed, at the position of the tile.
        /// 
        /// </summary>
        public Bitmap Sprite
        {
            get
            {
                // retrieve sprite based on current state.
                
                if (Tile.is_diffused)
                    return Sprites["Diffused"];

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

        /// <summary>
        /// accessor property for the underlying tile model.
        /// </summary>
        public MSTile Tile { get; set; }
    }

    /// <summary>
    /// class responsible for handling user interaction with a IMineSweeper instance,
    /// as well as managing a grid of MSTileVM, and making sure their properties are synchronized with the given model.
    /// </summary>
    public class MineSweeperViewModel : ViewModelBase
    {
        /// <summary>
        /// called when the user has finished the game.
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

        public Bitmap MSTimeDigit
        {
            get
            {
                int indx = TimeLeft / 10;

                if (indx == 0)
                    indx = -1;

                return m_digit_sprites[indx];
            }
        }

        public Bitmap LSTimeDigit
        {
            get => m_digit_sprites[TimeLeft % 10];
        }

        public Rect Position { get => Bounds.Clip; }

        public TransformedBounds Bounds { get; set; } = new();

        /// <summary>
        /// how much time the user has left, before GameOver is invoked.
        /// </summary>
        public int TimeLeft
        {
            get => m_time_left;
            protected set
            {
                this.RaiseAndSetIfChanged(ref m_time_left, Math.Max(value, 0));
                this.RaisePropertyChanged(nameof(MSTimeDigit));
                this.RaisePropertyChanged(nameof(LSTimeDigit));
            }
        }

        /// <summary>
        /// construct a MineSweeperViewModel, that will display the passed IMineSweeper model.
        /// </summary>
        /// <param name="time_span"> how many seconds the user has to solve the minefield </param>
        public MineSweeperViewModel(IMineSweeper _mine_sweeper, int time_span)
        {
            if (m_digit_sprites.Count == 0)
                loadDigitSprites();

            TimeLeft = time_span;
            mine_sweeper = _mine_sweeper;
            // the timer method is started on a separate thread, in order to avoid blocking the main thread, whilst waiting for the time running out.
            timer_thread = new(timerThread);

            grid = new(mine_sweeper.Tiles.Size);

            for (int x = 0; x < mine_sweeper.Tiles.Size.Width; x++)
            {
                for (int y = 0; y < mine_sweeper.Tiles.Size.Height; y++)
                {
                    grid[x, y] = new(mine_sweeper.Tiles[x, y]);
                }
            }

            // subscribe to the models TileChanged event, and make sure avalonia knows the tile has been updated, when the model notifies us about a tile change.
            mine_sweeper.TileChanged += (coord) => grid[coord].RaisePropertyChanged(nameof(MSTileVM.Sprite));
        }

        /// <summary>
        /// should be called when the game is started.
        /// initializes a timer thread, that keeps track of how much time the user has left.
        /// </summary>
        public void start() => timer_thread.Start();

        /// <summary>
        /// should be called, when the user attempts to left click a tile at a given position.
        /// 
        /// attempts to test the tile, and invokes GameOver if the move failed.
        /// 
        /// </summary>
        /// <param name="tile"></param>
        public void leftClickTile(Point tile)
        {
            if (mine_sweeper.testTile(tile) == MoveResult.Failure)
            {
                GameOver?.Invoke(this, false);
            }
        }

        /// <summary>
        /// attempts to diffuse a tile at the given position.
        /// 
        /// if the diffuse failed, the timer is decremented by a set size.
        /// GameOver is optionally called, if all the bombs have been diffused
        /// 
        /// </summary>
        /// <param name="tile"></param>
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

        /// <summary>
        /// flags the tile at the given position.
        /// </summary>
        public void rightClickTile(Point tile)
        {
            mine_sweeper.flagTile(tile);
        }

        protected static Dictionary<int, Bitmap> m_digit_sprites = new();

        protected static void loadDigitSprites()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            if (assets != null)
            {
                string[] sprites = new string[] { "Off", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                for (int i = 0; i < sprites.Length; i++)
                {
                    m_digit_sprites[i - 1] = new Bitmap(assets.Open(new($"avares://BattleSweeper/Assets/Number{sprites[i]}.png")));
                }
            }
        }

        // how many seconds left, before the timer has run out.
        protected int m_time_left;
        
        protected Thread timer_thread;

        // thread responsible for keeping track of how much time the user has spent on playing the game, and invoking GameOver, if the user has run out of time.
        protected void timerThread()
        {
            // as the TimeLeft property might change, whilst we are sleeping, we need to periodically check whether TimeLeft is still greater than zero.
            // rather than waiting for the full duration of TimeLeft.
            while(TimeLeft > 0)
            {
                Thread.Sleep(1000);
                TimeLeft--;
                Trace.WriteLine(Position.ToString());
            }

            GameOver?.Invoke(this, false);
        }
    }
}
