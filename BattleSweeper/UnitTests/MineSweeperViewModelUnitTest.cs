using BattleSweeper.ViewModels;
using Games;
using Games.MineSweeper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MineSweeperViewModelUnitTest
    {
        [Fact]
        void winGame()
        {
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10, 13);

            MineSweeperViewModel mine_sweeper_vm = new(mine_sweeper_model, int.MaxValue);
            
            bool completed_game = false;

            mine_sweeper_vm.GameOver += (s, won) =>
            {
                completed_game = true;
                Assert.True(won);
            };

            // clear the board

            mine_sweeper_vm.leftClickTile(new Point(3, 3));
            mine_sweeper_vm.leftClickTile(new Point(6, 6));
            mine_sweeper_vm.leftClickTile(new Point(0, 2));
            mine_sweeper_vm.leftClickTile(new Point(0, 0));
            mine_sweeper_vm.leftClickTile(new Point(1, 0));
            mine_sweeper_vm.leftClickTile(new Point(2, 0));
            mine_sweeper_vm.leftClickTile(new Point(4, 0));
            mine_sweeper_vm.leftClickTile(new Point(5, 1));
            mine_sweeper_vm.leftClickTile(new Point(6, 0));
            mine_sweeper_vm.leftClickTile(new Point(6, 1));
            mine_sweeper_vm.leftClickTile(new Point(6, 2));
            mine_sweeper_vm.leftClickTile(new Point(7, 1));
            mine_sweeper_vm.leftClickTile(new Point(7, 2));
            mine_sweeper_vm.leftClickTile(new Point(9, 3));
            mine_sweeper_vm.leftClickTile(new Point(9, 4));
            mine_sweeper_vm.leftClickTile(new Point(8, 4));
            mine_sweeper_vm.leftClickTile(new Point(9, 5));
            mine_sweeper_vm.leftClickTile(new Point(8, 5));
            mine_sweeper_vm.leftClickTile(new Point(7, 6));
            mine_sweeper_vm.leftClickTile(new Point(9, 6));
            mine_sweeper_vm.leftClickTile(new Point(7, 7));
            mine_sweeper_vm.leftClickTile(new Point(8, 7));
            mine_sweeper_vm.leftClickTile(new Point(9, 7));
            mine_sweeper_vm.leftClickTile(new Point(7, 8));
            mine_sweeper_vm.leftClickTile(new Point(9, 8));

            // flag the tiles
            mine_sweeper_vm.rightClickTile(new Point(0, 9));
            mine_sweeper_vm.rightClickTile(new Point(0, 1));
            mine_sweeper_vm.rightClickTile(new Point(0, 3));
            mine_sweeper_vm.rightClickTile(new Point(3, 0));
            mine_sweeper_vm.rightClickTile(new Point(5, 0));
            mine_sweeper_vm.rightClickTile(new Point(5, 2));
            mine_sweeper_vm.rightClickTile(new Point(5, 6));
            mine_sweeper_vm.rightClickTile(new Point(7, 9));
            mine_sweeper_vm.rightClickTile(new Point(8, 3));
            mine_sweeper_vm.rightClickTile(new Point(8, 6));

            // diffuse the bombs
            mine_sweeper_vm.leftShiftClickTile(new Point(0, 9));
            mine_sweeper_vm.leftShiftClickTile(new Point(0, 1));
            mine_sweeper_vm.leftShiftClickTile(new Point(0, 3));
            mine_sweeper_vm.leftShiftClickTile(new Point(3, 0));
            mine_sweeper_vm.leftShiftClickTile(new Point(5, 0));
            mine_sweeper_vm.leftShiftClickTile(new Point(5, 2));
            mine_sweeper_vm.leftShiftClickTile(new Point(5, 6));
            mine_sweeper_vm.leftShiftClickTile(new Point(7, 9));
            mine_sweeper_vm.leftShiftClickTile(new Point(8, 3));
            mine_sweeper_vm.leftShiftClickTile(new Point(8, 6));

            // should have won by this point.
            Assert.True(completed_game);
        }

        [Fact]
        void loseGame()
        {
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10, 13);

            MineSweeperViewModel mine_sweeper_vm = new(mine_sweeper_model, int.MaxValue);

            bool completed_game = false;

            mine_sweeper_vm.GameOver += (s, won) =>
            {
                completed_game = true;
                Assert.True(!won);
            };

            // hit bomb
            mine_sweeper_vm.leftClickTile(new Point(0, 1));

            Assert.True(completed_game);
        }

        [Fact]
        void timeoutGame()
        {
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10, 13);

            MineSweeperViewModel mine_sweeper_vm = new(mine_sweeper_model, 1);

            bool completed_game = false;

            mine_sweeper_vm.GameOver += (s, won) =>
            {
                completed_game = true;
                Assert.True(!won);
            };

            mine_sweeper_vm.start();

            Thread.Sleep(3000);

            Assert.True(completed_game);
        }
    }
}
