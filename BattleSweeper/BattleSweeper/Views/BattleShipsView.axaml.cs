using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using BattleSweeper.ViewModels;
using DynamicData;
using Games.Battleships;
using ReactiveUI;

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
                Grid player_1_grid = this.FindControl<Grid>("Player1Grid");

                setupBattleshipsGrid(player_1_grid, vm.bs_player_1, ref vm.bs1_tile_vm);

                Grid player_2_grid = this.FindControl<Grid>("Player2Grid");

                setupBattleshipsGrid(player_2_grid, vm.bs_player_2, ref vm.bs2_tile_vm);
            }
        }

        /// <summary>
        /// constructs a viewmodel grid, for the passed battleships model, and binds it to an avalonia grid.
        /// </summary>
        private void setupBattleshipsGrid(Grid grid, IBattleships battle_ships, ref Games.Grid<BSTileVM> tiles_viewmodel)
        {
            for (int x = 0; x < battle_ships.Tiles.Size.Width; x++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                for (int y = 0; y < battle_ships.Tiles.Size.Height; y++)
                {
                    if(y == 0)
                        grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

                    tiles_viewmodel[x, y] = new(battle_ships.Tiles[x, y]);

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

    }
}
