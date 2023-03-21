using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Data;
using BattleSweeper.ViewModels;
using Games.MineSweeper;
using ReactiveUI;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Image = Avalonia.Controls.Image;

namespace BattleSweeper.Views
{
    public partial class MineSweeperView : UserControl
    {
        public MineSweeperView()
        {
            InitializeComponent();

            DataContextChanged += onViewModelAdd;
        }

        private void onViewModelAdd(object? sender, System.EventArgs e)
        {
            if (DataContext is MineSweeperViewModel vm)
            {
                Grid grid = this.FindControl<Grid>("MineSweeperGrid");

                var bounds_binding = new Binding
                {
                    Source = vm,
                    Path = nameof(vm.Bounds),
                    Mode = BindingMode.OneWayToSource
                };

                grid.Bind(TransformedBoundsProperty, bounds_binding);

                for (int x = 0; x < vm.grid.Size.Width; x++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                    for (int y = 0; y < vm.grid.Size.Height; y++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
                        MSTileVM tile = vm.grid[x, y];

                        Image img = new();
                        img[Grid.RowProperty] = y;
                        img[Grid.ColumnProperty] = x;


                        INotifyPropertyChanged test = tile; 
                        var binding = new Binding
                        {
                            Source = tile,
                            Path = nameof(tile.Sprite)
                        };

                        img.Bind(Image.SourceProperty, binding);

                        tile.RaisePropertyChanged(nameof(tile.Sprite));

                        grid.Children.Add(img);
                    }
                }
            }
        }
    }
}
