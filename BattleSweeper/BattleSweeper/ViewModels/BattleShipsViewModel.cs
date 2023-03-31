﻿using Avalonia.Platform;
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
using Point = System.Drawing.Point;
using Avalonia.VisualTree;
using ReactiveUI;
using Avalonia.Metadata;
using System.Reactive;

namespace BattleSweeper.ViewModels
{
    public enum Player
    {
        None,
        Player1,
        Player2,
    }
    public class BSTileVM : ReactiveObject, ICopyable<BSTileVM>
    {
        public BSTileVM(BattleshipTile tile, BattleShipsViewModel viewmodel, Player player)
        {
            this.tile = tile;
            vm = viewmodel;
            this.player = player;

            vm.AllTilesChanged += tileChanged;
        }

        public bool isHit { get => tile.hasBeenShot && tile.ship != -1; }
        public bool isMissed { get => tile.hasBeenShot && !isHit; }

        public bool isBombHit { get => tile.hasBeenShot && tile.hasBomb; }

        public bool isMiddle { get => tile.ship != -1 && !tile.atEnd && !tile.atStart && vm.ActivePlayer == player; }

        public bool isEnd { get => tile.ship != -1 && (tile.atEnd || tile.atStart) && vm.ActivePlayer == player; }

        public RotateTransform shipTransform { get => new((tile.horizontal ? -90 : 0) + (tile.atEnd ? 180 : 0)); }

        public void tileChanged()
        {
            this.RaisePropertyChanged(nameof(isHit));
            this.RaisePropertyChanged(nameof(isMissed));
            this.RaisePropertyChanged(nameof(isBombHit));
            this.RaisePropertyChanged(nameof(isMiddle));
            this.RaisePropertyChanged(nameof(isEnd));
            this.RaisePropertyChanged(nameof(shipTransform));
        }

        public BSTileVM Copy() => new BSTileVM(tile, vm, player);

        public BattleshipTile tile;
        public BattleShipsViewModel vm;
        public Player player;
    }

    public class BattleShipsViewModel : ViewModelBase
    {
        public BattleShipsViewModel(IBattleships bs_player_1, IBattleships bs_player_2)
        {
            this.bs_player_1 = bs_player_1;
            this.bs_player_2 = bs_player_2;

            this.bs_player_1.TileChanged += (coord) => bs1_tile_vm[coord].tileChanged();
            this.bs_player_2.TileChanged += (coord) => bs2_tile_vm[coord].tileChanged();

            ActivePlayer = Player.Player1;
            changePlayer();
        }
        public bool isPlacingShips = true;
        protected bool isVertical = true;

        public Action? AllTilesChanged { get; set; }

        public BSTileVM Tile { get => bs1_tile_vm[0, 0]; }

        public Player ActivePlayer { get => m_active_player; set => this.RaiseAndSetIfChanged(ref m_active_player, value); }
        public bool PlayerChanging { get => m_player_changing; set => this.RaiseAndSetIfChanged(ref m_player_changing, value); }

        public IBattleships bs_player_1;
        public IBattleships bs_player_2;

        public Grid<BSTileVM> bs1_tile_vm = new(new(10, 10));
        public Grid<BSTileVM> bs2_tile_vm = new(new(10, 10));

        public Rect? ActiveGridBounds {
            get
            {
                if (ActivePlayer == Player.Player1)
                    return Bs1TransformedBounds.Clip;

                else if(ActivePlayer == Player.Player2)
                    return Bs2TransformedBounds.Clip;

                return null;
            }
        }

        public TransformedBounds Bs1TransformedBounds { get; set; }
        public TransformedBounds Bs2TransformedBounds { get; set; }

        public void changeDirection()
        {
            isVertical = !isVertical;
        }

        public void leftClick(Point coord)
        {
            if (isPlacingShips)
            {
                if (ActivePlayer == Player.Player1)
                {
                    bs_player_1.placeShip(coord, isVertical);
                }
                if (ActivePlayer == Player.Player2)
                {
                    bs_player_2.placeShip(coord, isVertical);
                }
            }
            else
            {
                if (ActivePlayer == Player.Player1)
                {
                    bs_player_1.shoot(coord);
                }
                if (ActivePlayer == Player.Player2)
                {
                    bs_player_2.shoot(coord);
                }
            }
            
        }

        public void changePlayer()
        {
            PlayerChanging = true;


            if (ActivePlayer == Player.Player1)
                m_next_player = Player.Player2;
            else if (ActivePlayer == Player.Player2)
                m_next_player = Player.Player1;

            ActivePlayer = Player.None;

            AllTilesChanged?.Invoke();
        }

        public void confirmPlayerChange()
        {
            PlayerChanging = false;

            ActivePlayer = m_next_player;

            AllTilesChanged?.Invoke();
        }

        protected bool m_player_changing = false;
        protected Player m_next_player = Player.None;
        protected Player m_active_player;
    }
}
