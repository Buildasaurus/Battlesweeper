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
using Point = System.Drawing.Point;
using Avalonia.VisualTree;
using ReactiveUI;
using Avalonia.Metadata;
using System.Reactive;
using Games;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


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

            IBattleships? bs = null;

            switch (player)
            {
                case Player.Player1:
                    bs = vm.bs_player_1;
                    break;
                case Player.Player2:
                    bs = vm.bs_player_2;
                    break;
            }

            if(bs != null)
                bs.ShipSunk += (s, ship) =>
                {
                    if (tile.ship == ship)
                        this.RaisePropertyChanged(nameof(isDestroyed));
                };

            vm.AllTilesChanged += tileChanged;
        }

        public bool isHit { get => tile.hasBeenShot && tile.ship != -1; }
        public bool isMissed { get => tile.hasBeenShot && !isHit; }

        public bool isBombHit { get => tile.hasBeenShot && tile.hasBomb; }

        public bool isMiddle { get =>
                //tile must be a ship
                tile.ship != -1
                // the ship must be in the middle
                && !tile.atEnd
                && !tile.atStart;
        }

        public bool showShips => vm.ActivePlayer == player;

        public bool isEnd { get =>
                // must be ship
                tile.ship != -1
                // must not be a middle piece
                && (tile.atEnd || tile.atStart);
                }
        public bool isDestroyed { get  
            {
                // must be a ship
                if (tile.ship == -1)
                    return false;

                // retrieve the relevant battleship model, depending on the tiles player

                IBattleships? bs = null;

                switch(player)
                {
                    case Player.Player1:
                        bs = vm.bs_player_1;
                        break;
                    case Player.Player2:
                        bs = vm.bs_player_2;
                        break;
                }

                // check if there exist any remaining pieces of the current ship.
                return (bs?.remainingPieces[tile.ship] ?? 0) == 0;
            }
        }
        public bool isBrokenEnd { get => tile.ship != -1 && (tile.atEnd || tile.atStart) && vm.ActivePlayer == player; }

        public RotateTransform shipTransform { get => new((tile.horizontal ? -90 : 0) + (tile.atEnd ? 0 : 180)); }

        public void tileChanged()
        {
            this.RaisePropertyChanged(nameof(isHit));
            this.RaisePropertyChanged(nameof(isMissed));
            this.RaisePropertyChanged(nameof(isBombHit));
            this.RaisePropertyChanged(nameof(isMiddle));
            this.RaisePropertyChanged(nameof(isEnd));
            this.RaisePropertyChanged(nameof(shipTransform));
            this.RaisePropertyChanged(nameof(isDestroyed));
            this.RaisePropertyChanged(nameof(showShips));
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

            ShipHighlights = new();

            for(int i = 0; i < bs_player_1.shipLengths.Count; i++)
                ShipHighlights.Add(new(Color.FromArgb(0, 0, 0, 0)));

            // first ship should be highlighted, as it is the first to be placed.
            ShipHighlights[0] = PlacingShipHighlight;
        }

        int placedShips = 0;
        public bool isPlacingShips = true;
        protected bool isVertical = true;

        /// <summary>
        /// the background color of the ships in the top border, when they should not be highlighted in any way.
        /// </summary>
        public static readonly SolidColorBrush NoShipHighlight = new(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// ship highlight color when they are about to be placed
        /// </summary>
        public static readonly SolidColorBrush PlacingShipHighlight = new(Color.FromArgb(155, 0, 255, 0));
        /// <summary>
        /// ship highlight color when they have been destroyed.
        /// </summary>
        public static readonly SolidColorBrush DestroyedShipHighlight = new(Color.FromArgb(155, 255, 0, 0));

        /// <summary>
        /// gets invoked when all the tiles needs to be updated.
        /// happens primarily on a player change.
        /// </summary>
        public Action? AllTilesChanged { get; set; }

        public Player ActivePlayer { get => m_active_player; set => this.RaiseAndSetIfChanged(ref m_active_player, value); }
        public bool PlayerChanging { get => m_player_changing; set => this.RaiseAndSetIfChanged(ref m_player_changing, value); }

        /// <summary>
        /// controls whether the placing ship arrow should be visible for player 1 (or player 2 [Player2ArrowVisible])
        /// 
        /// this should be true, if the mouse is inside the ActiveGrid, and we are placing ships AND it is player 1's turn to place a ship.
        /// 
        /// </summary>
        public bool Player1ArrowVisible { get => m_active_player == Player.Player1 && bs_player_1.Tiles.inBounds(m_arrow_coords) && isPlacingShips; }
        public bool Player2ArrowVisible { get => m_active_player == Player.Player2 && bs_player_2.Tiles.inBounds(m_arrow_coords) && isPlacingShips; }

        /// <summary>
        /// the column that the arrow should be visible in, in the active grid.
        /// </summary>
        public double ArrowX => m_arrow_coords.X;

        /// <summary>
        /// the row that the arrow should be visible in, in the active grid.
        /// </summary>
        public double ArrowY => m_arrow_coords.Y;

        /// <summary>
        /// rotation of the angle.
        /// changes depending on the isVertical property.
        /// </summary>
        public RotateTransform ArrowAngle => new(isVertical ? 0 : -90);

        /// <summary>
        /// list of background colors for the ships displayed in the top border.
        /// 
        /// ObservableCollection = RaisePropertyChanged is automatically called, when the indexer is used.
        /// 
        /// </summary>
        public ObservableCollection<SolidColorBrush> ShipHighlights { get; set; }

        public IBattleships bs_player_1;
        public IBattleships bs_player_2;

        public Grid<BSTileVM> bs1_tile_vm = new(new(10, 10));
        public Grid<BSTileVM> bs2_tile_vm = new(new(10, 10));

        /// <summary>
        /// returns the transform bounds of the active players battleships grid.
        /// returns null, if there is no active player.
        /// </summary>
        public Rect? ActiveGridBounds {
            get
            {
                if (!isPlacingShips)
                {
					if (ActivePlayer == Player.Player2)
						return Bs1TransformedBounds.Clip;

					else if (ActivePlayer == Player.Player1)
						return Bs2TransformedBounds.Clip;
				}
                else
                {
					if (ActivePlayer == Player.Player1)
						return Bs1TransformedBounds.Clip;

					else if (ActivePlayer == Player.Player2)
						return Bs2TransformedBounds.Clip;

				}
                
				return null;
            }
        }

        /// <summary>
        /// binding to battleshipviews Player1Grid transform bounds
        /// 
        /// is used to get the absolute pixel position and size of the grid, relative to the window top left corner.
        /// 
        /// </summary>
        public TransformedBounds Bs1TransformedBounds { get; set; }

        /// <summary>
        /// see Bs1TransformedBounds, but now for Player2Grid.
        /// </summary>
        public TransformedBounds Bs2TransformedBounds { get; set; }

        public void changeDirection()
        {
            isVertical = !isVertical;
            this.RaisePropertyChanged(nameof(ArrowAngle));
        }

        private int mod(int a, int b)//modulus function, because % is not modulus in c#, and c# doesn't have modulus???
        {
            int remainder = a % b;
			return remainder < 0 ? remainder + b : remainder;
		}

		public void leftClick(Point coord)
        {
            if (isPlacingShips)
            {
                if (ActivePlayer == Player.Player1)
                {
                    // placed ships should only be incremented if the placeShip call was successful
                    if(bs_player_1.placeShip(coord, isVertical) == MoveResult.Success)
                        placedShips++;
                }
                if (ActivePlayer == Player.Player2)
                {
                    if(bs_player_2.placeShip(coord, isVertical) == MoveResult.Success)
                        placedShips++;
                }
                if (placedShips == 5 && ActivePlayer == Player.Player2)
                {
                    isPlacingShips = false;
                    changePlayer();
                }
                else if (placedShips == 5)
                {
                    changePlayer();
                    placedShips = 0;
                }

				// set previous ship to nohighlight - modulus not really necessary: if statement would do for case placedShips = 0
				ShipHighlights[mod((placedShips-1),ShipHighlights.Count)] = NoShipHighlight; 
				if (isPlacingShips)
                    ShipHighlights[placedShips] = PlacingShipHighlight;

                this.RaisePropertyChanged(nameof(Player1ArrowVisible));
                this.RaisePropertyChanged(nameof(Player2ArrowVisible));
                this.RaisePropertyChanged(nameof(ShipHighlights));
            }
            else //if shooting
            {
                
                if (ActivePlayer == Player.Player1) //player 1 is shooting at player 2
                {
                    Games.MoveResult move = bs_player_2.shoot(coord);
                    if (move == MoveResult.Failure)
                    {
                        changePlayer();
					}
				}

				if (ActivePlayer == Player.Player2) //player 2 is shooting at player 1
				{
                    Games.MoveResult move = bs_player_1.shoot(coord);
                    if (move == MoveResult.Failure)
					{
                        changePlayer();
					}
				}


				highlightShips(ActivePlayer);

			}
		}


        /// <summary>
        /// begins a player change.
        /// takes away shooting capability from the active player, hides both grids ships,
        /// and reveals a button, that the next player should click, to continue the game.
        /// </summary>
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
        /// <summary>
        /// Highlights the ships that the given player shot and sunk.
        /// Eg. if player 1 is given, then all the dead ships of player 2 is highlighted.
        /// 
        /// if player given is none, then doesn't change highlighten -> outcommented code will set all to no highlight.
        /// </summary>
        /// <param name="player"></param>
        public void highlightShips(Player player)
        {
			if (player == Player.Player1)
			{
				for (int i = 0; i < bs_player_2.remainingPieces.Count; i++)
				{
					if (bs_player_2.remainingPieces[i] == 0)
						ShipHighlights[i] = DestroyedShipHighlight;
					else
						ShipHighlights[i] = NoShipHighlight;
				}
			}
			else if (player == Player.Player2)
			{
				for (int i = 0; i < bs_player_1.remainingPieces.Count; i++)
				{
					if (bs_player_1.remainingPieces[i] == 0)
						ShipHighlights[i] = DestroyedShipHighlight;
					else
						ShipHighlights[i] = NoShipHighlight;
				}
			}
            /* code for having no highlights between player, if deemed necessary.
            else
            {
				for (int i = 0; i < bs_player_1.remainingPieces.Count; i++)
				{
						ShipHighlights[i] = NoShipHighlight;
				}
			}*/
		}

        /// <summary>
        /// gets called when the next player has confirmed the player change,
        /// and is ready to see their version of the grids.
        /// 
        /// updates the active player, as well as the visible tiles on the grid.
        /// 
        /// </summary>
        public void confirmPlayerChange()
        {
            // highlight next players sunken ships.
            if (!isPlacingShips)
            {
				highlightShips(m_next_player);
			}

			PlayerChanging = false;

            ActivePlayer = m_next_player;

            AllTilesChanged?.Invoke();
        }

        /// <summary>
        /// fires every time the mouse has hovered above the main window.
        /// the coordinate is a pont coordinate, that specifies the tile that the mouse is currently hovering over.
        /// the tile is relative to ActiveGrid.
        /// 
        /// updates the arrow position and visibility.
        /// 
        /// </summary>
        /// <param name="grid_coord"></param>
        public void mouseMoved(Point grid_coord)
        {
            m_arrow_coords = grid_coord;

            switch(ActivePlayer)
            {
                case Player.Player1:
                    this.RaisePropertyChanged(nameof(Player1ArrowVisible));
                    break;
                case Player.Player2:
                    this.RaisePropertyChanged(nameof(Player2ArrowVisible));
                    break;
            }

            this.RaisePropertyChanged(nameof(ArrowX));
            this.RaisePropertyChanged(nameof(ArrowY));
        }

        protected bool m_player_changing = false;
        protected Player m_next_player = Player.None;
        protected Player m_active_player;

        protected Point m_arrow_coords;
    }
}
