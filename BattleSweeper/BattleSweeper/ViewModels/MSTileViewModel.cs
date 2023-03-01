using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData.Diagnostics;
using Games.MineSweeper;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSweeper.ViewModels
{
    public class MSTileViewModel : ReactiveObject
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

        public MSTileViewModel(MineSweeperTile tile, System.Drawing.Point coord)
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
}
