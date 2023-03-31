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
using Avalonia.Media;

namespace BattleSweeper.ViewModels
{
    public class BSTileVM : ICopyable<BSTileVM>
    {
        public BSTileVM(BattleshipTile tile)
        {
            this.tile = tile;
        }

        public bool isHit { get => tile.hasBeenShot; }

        public bool isBombHit { get => tile.hasBeenShot && tile.hasBomb; }

        public bool isMiddle { get => tile.ship != -1 && !tile.atEnd && !tile.atStart; }

        public bool isEnd { get => tile.ship != -1 && (tile.atEnd || tile.atStart); }

        public RotateTransform shipTransform { get => new((tile.horizontal ? -90 : 0) + (tile.atEnd ? 180 : 0)); }

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

        public BSTileVM Tile { get => bs1_tile_vm[0, 0]; }

        public bool Player1Playing { get; }
        public bool Player2Playing { get; }

        public IBattleships bs_player_1;
        public IBattleships bs_player_2;

        public Grid<BSTileVM> bs1_tile_vm = new(new(10, 10));
        public Grid<BSTileVM> bs2_tile_vm = new(new(10, 10));
    }
}
