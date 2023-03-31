using BattleSweeper.ViewModels;
using Games.Battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class BattleShipsViewModelTest
    {
        [Fact]
        public void TileVMTest()
        {
            {
                // test empty tile.
                BattleshipTile bs_tile = new(-1, false, false, false, false, false);
                BSTileVM tile = new(bs_tile, null, Player.None);

                Assert.False(tile.isEnd || tile.isMiddle || tile.isHit || tile.isBombHit);
            }

            {
                // test horizontal middle ship tile

                BattleshipTile bs_tile = new(0, false, false, true, false, false);
                BSTileVM tile = new(bs_tile, null, Player.None);

                Assert.True(!tile.isEnd && tile.isMiddle && !tile.isHit && !tile.isBombHit);
                Assert.True(tile.shipTransform.Angle == -90);
            }


        }
    }
}
