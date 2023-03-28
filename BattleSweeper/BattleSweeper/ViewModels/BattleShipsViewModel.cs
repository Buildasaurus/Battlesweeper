using Avalonia.Platform;
using Avalonia;
using Games;
using Games.Battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace BattleSweeper.ViewModels
{
    public class BSTileVM : ICopyable<BSTileVM>
    {
        public BSTileVM(BattleshipTile tile)
        {
            this.tile = tile;
        }

        public BSTileVM Copy() => new BSTileVM(tile);

        public BattleshipTile tile;
    }

    public class BattleShipsViewModel : ViewModelBase
    {
        public BattleShipsViewModel(IBattleships bs_player_1, IBattleships bs_player_2)
        {
            this.bs_player_1 = bs_player_1;
            this.bs_player_2 = bs_player_2;
        }

        public bool Player1Playing { get; }
        public bool Player2Playing { get; }

        public IBattleships bs_player_1;
        public IBattleships bs_player_2;

        public Grid<BSTileVM> bs1_tile_vm = new(new(10, 10));
        public Grid<BSTileVM> bs2_tile_vm = new(new(10, 10));
    }
}
