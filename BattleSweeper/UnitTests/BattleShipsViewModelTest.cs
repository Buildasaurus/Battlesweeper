using BattleSweeper.ViewModels;
using Games.Battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class BattleShipsViewModelTest
    {
        [Fact]
        public void TileVMTest()
        {
            BattleshipsTemplate player1_template = new();
            player1_template.constructBoard(new(), new() { 1 });

            BattleshipsTemplate player2_template = new();
            player1_template.constructBoard(new(), new() { 1 });
            BattleShipsViewModel vm = new(player1_template, player2_template);

            {
                // test empty tile.
                BattleshipTile bs_tile = new(-1, false, false, false, false, false);
                BSTileVM tile = new(bs_tile, vm, Player.Player1);

                Assert.False(tile.isEnd || tile.isMiddle || tile.isHit || tile.isDestroyed || tile.isBombHit);
            }

            {
                // test horizontal middle ship tile

                BattleshipTile bs_tile = new(0, false, false, true, false, false);
                BSTileVM tile = new(bs_tile, vm, Player.Player1);

                Assert.True(!tile.isEnd && tile.isMiddle && !tile.isHit && !tile.isDestroyed && !tile.isBombHit && tile.showShips);
                Assert.True(tile.shipTransform.Angle == 90);
            }

            {
                // test vertical endship which is destroyed.
                player1_template.remainingPieces[0] = 0;
                BattleshipTile bs_tile = new(0, true, false, false, true, false);
                BSTileVM tile = new(bs_tile, vm, Player.Player1);

                Assert.True(tile.isEnd && !tile.isMiddle && tile.isHit && tile.isDestroyed && !tile.isBombHit && tile.showShips);
            }

            {
                // test player 2 tile which is hit, but has a ship.
                BattleshipTile bs_tile = new(0, true, false, false, false, false);
                BSTileVM tile = new(bs_tile, vm, Player.Player2);

                Assert.True(!tile.isEnd && tile.isMiddle && tile.isHit && !tile.showShips);
            }

        }
    }
}
