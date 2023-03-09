using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Games.MineSweeper;

namespace BattleSweeper.ViewModels
{
    public class MineSweeperViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public Dictionary<string, Bitmap> Sprites { get; set; }
        
        public MineSweeperViewModel() 
        {
            
        }
        public void constructGame(IMineSweeper mineGame)
        {
            for(int x = 0; x < 10; x++)
                for(int y = 0; y < 10; y++)
                {
                    MSTileViewModel tile = new(mineGame.Tiles[x, y], new Point(x, y));

                    Avalonia.Controls.Image img = new();
                    img[Grid.RowProperty] = y;
                    img[Grid.ColumnProperty] = x;

                    img.Source = tile.Sprite;

                    grid.Children.Add(img); // adds a lot of the tile sprites to the grid
                }
        }

        
        public Grid? grid;
    }
}
