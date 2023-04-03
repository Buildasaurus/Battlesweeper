using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using BattleSweeper.ViewModels;
using DynamicData;
using Games.Battleships;
using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Controls;
using ColumnDefinition = Avalonia.Controls.ColumnDefinition;
using ContentControl = Avalonia.Controls.ContentControl;
using Grid = Avalonia.Controls.Grid;
using Image = Avalonia.Controls.Image;
using Orientation = Avalonia.Layout.Orientation;
using RowDefinition = Avalonia.Controls.RowDefinition;
using StackPanel = Avalonia.Controls.StackPanel;
using UserControl = Avalonia.Controls.UserControl;

namespace BattleSweeper.Views
{
    public partial class BattleShipsView : UserControl
    {
        public BattleShipsView()
        {
            InitializeComponent();
            DataContextChanged += onViewModelAdd;
        }

        private void onViewModelAdd(object? sender, System.EventArgs e)
        {
            if (DataContext is BattleShipsViewModel vm)
            {
				setupShipsDisplay(vm);

				Grid player_1_grid = this.FindControl<Grid>("Player1Grid");

                var bounds_1_binding = new Binding
                {
                    Source = vm,
                    Path = nameof(vm.Bs1TransformedBounds),
                    Mode = BindingMode.OneWayToSource
                };

                player_1_grid.Bind(TransformedBoundsProperty, bounds_1_binding);

                setupBattleshipsGrid(player_1_grid, vm.bs_player_1, vm, Player.Player1, ref vm.bs1_tile_vm);

                Grid player_2_grid = this.FindControl<Grid>("Player2Grid");

                var bounds_2_binding = new Binding
                {
                    Source = vm,
                    Path = nameof(vm.Bs2TransformedBounds),
                    Mode = BindingMode.OneWayToSource
                };

                player_2_grid.Bind(TransformedBoundsProperty, bounds_2_binding);

                setupBattleshipsGrid(player_2_grid, vm.bs_player_2, vm, Player.Player2, ref vm.bs2_tile_vm);
            }
        }

        /// <summary>
        /// constructs a viewmodel grid, for the passed battleships model, and binds it to an avalonia grid.
        /// </summary>
        private void setupBattleshipsGrid(Grid grid, IBattleships battle_ships, BattleShipsViewModel vm, Player player, ref Games.Grid<BSTileVM> tiles_viewmodel)
        {
            for (int x = 0; x < battle_ships.Tiles.Size.Width; x++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                for (int y = 0; y < battle_ships.Tiles.Size.Height; y++)
                {
                    if(y == 0)
                        grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

                    tiles_viewmodel[x, y] = new(battle_ships.Tiles[x, y], vm, player);

                    ContentControl presenter = new()
                    {
                        Content = tiles_viewmodel[x, y],
                    };

                    presenter[Grid.RowProperty] = y;
                    presenter[Grid.ColumnProperty] = x;

                    grid.Children.Add(presenter);
                }
            }
        }

        private void setupShipsDisplay(BattleShipsViewModel vm)
        {
            List<int> shiplenghts = vm.bs_player_1.shipLengths;
            int counter = 0;
            foreach (int shiplength in shiplenghts)
            {
                StackPanel stk = new StackPanel();
                stk.Orientation = Orientation.Horizontal;
                string binding = "ShipHighlights[" + counter + "]";
				stk.Bind(StackPanel.BackgroundProperty, new Binding(binding));
                counter++;
				for (int i = 0; i < shiplength; i++) //add middle parts
                {
					double? width = this.Resources["width"] as double?;
					Image img = new Image();
					img.Width = width ?? default(double);
					if (i == shiplength - 1) // if placing last ship, rotation is different
                    {
						img.RenderTransform = new RotateTransform(270);
                        img.Source = (this.Resources["end"] as Image).Source;
					}
					else
                    {
						img.RenderTransform = new RotateTransform(90);
						img.Source = (this.Resources["mid"] as Image).Source;
					}
					if (i == 0)
                    {
						img.Source = (this.Resources["end"] as Image).Source;
					}
					stk.Children.Add(img);
				}
				shipDisplay.Children.Add(stk);

			}
		}

    }
}
