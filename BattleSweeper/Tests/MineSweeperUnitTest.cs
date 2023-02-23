using Games.MineSweeper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MineSweeperUnitTest
    {
        [Fact]
        public void constructWithBombs()
        {
            MineSweeper mine_sweeper = new();

            mine_sweeper.generate(new Size(10, 10), 3, 10);

            for(int y = 0; y < mine_sweeper.Tiles.Size.Height; y++)
            {
                for(int x = 0; x < mine_sweeper.Tiles.Size.Height; x++)
                {
                    Trace.Write(mine_sweeper.Tiles[x, y].bomb_count);
                }

                Trace.WriteLine("");
            }

            return;
        }
    }
}
