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

namespace BattleSweeper.ViewModels
{
    public class MSTileVM : ReactiveObject, ICopyable<MSTileVM>
    {
        public static Dictionary<string, Bitmap> Sprites { get; set; } = new();

        public static void loadSprites()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            string[] sprites = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "Bomb", "Empty", "Tile", "Flag" };

            foreach (string sprite in sprites)
            {
                Sprites[sprite] = new Bitmap(assets.Open(new($"avares://BattleSweeper/Assets/MineSweeper{sprite}.png")));
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

    public class MineSweeperViewModel : ViewModelBase
    {
        public MineSweeperViewModel(IMineSweeper _mine_sweeper) 
        {
            mine_sweeper = _mine_sweeper;

            grid = new(mine_sweeper.Tiles.Size);

            for (int x = 0; x < mine_sweeper.Tiles.Size.Width; x++)
            {
                for (int y = 0; y < mine_sweeper.Tiles.Size.Height; y++)
                {
                    grid[x, y] = new(mine_sweeper.Tiles[x, y], new Point(x, y));
                }
            }

        }

        //public void constructGame(IMineSweeper mineGame)
        //{
        //    for(int x = 0; x < 10; x++)
        //        for(int y = 0; y < 10; y++)
        //        {
        //            MSTileViewModel tile = new(mineGame.Tiles[x, y], new Point(x, y));

        //            Avalonia.Controls.Image img = new();
        //            img[Grid.RowProperty] = y;
        //            img[Grid.ColumnProperty] = x;

        //            img.Source = tile.Sprite;

        //            grid.Children.Add(img);
        //        }
        //}

        public Grid<MSTileVM> grid;
        public IMineSweeper mine_sweeper;
    }
}
