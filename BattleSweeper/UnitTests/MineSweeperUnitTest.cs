using Games;
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
        /// <summary>
        /// test if the MineSweeper object is able to generate a valid minesweeper field, with the specified number of bombs.
        /// </summary>
        [Fact]
        public void constructWithBombs()
        {
            // keep track of the number of bombs on the generated grid (should be three).
            int bombs = 0;
            MineSweeper mine_sweeper = new();

            mine_sweeper.generate(new Size(10, 10), 3, 10);

            for(int y = 0; y < mine_sweeper.Tiles.Size.Height; y++)
            {
                for(int x = 0; x < mine_sweeper.Tiles.Size.Height; x++)
                {
                    Trace.Write(mine_sweeper.Tiles[x, y].bomb_count);
                    if (mine_sweeper.Tiles[x, y].bomb_count == IMineSweeper.BOMB)
                        bombs++;
                }

                Trace.WriteLine("");
            }

            // check if the bomb count matches up.
            Assert.True(bombs == 3);
            return;
        }

        [Fact]
        public void testTiles()
        {
            MineSweeper mine_sweeper = new();

            mine_sweeper.generate(new Size(10, 10), 10, 10);

            // there is no bomb here, with the seed 10
            Assert.True(mine_sweeper.testTile(new Point(4, 0)) == MoveResult.Success);

            // the tile will already have been cleared, therefore it should return illegal.
            Assert.True(mine_sweeper.testTile(new Point(4, 0)) == MoveResult.Illegal);
        }

        [Fact]
        public void flagTile()
        {
            MineSweeper mine_sweeper = new();

            mine_sweeper.generate(new Size(10, 10), 10, 10);

            Assert.True(mine_sweeper.flagTile(new Point(4, 0)) == MoveResult.Success);

            // the tile will have been flagged, therefore it should return illegal.
            Assert.True(mine_sweeper.testTile(new Point(4, 0)) == MoveResult.Illegal);
            
            Assert.True(mine_sweeper.flagTile(new Point(4, 0)) == MoveResult.Success);
            
            Assert.True(mine_sweeper.testTile(new Point(4, 0)) == MoveResult.Success);
         
            Assert.True(mine_sweeper.flagTile(new Point(4, 0)) == MoveResult.Failure);
        }
    }
}
