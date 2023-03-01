using Avalonia.Controls;
using BattleSweeper.ViewModels;
using Games.MineSweeper;
using System.Drawing;

namespace BattleSweeper.Views
{
    public partial class MineSweeperView : UserControl
    {
        public MineSweeperView()
        {
            InitializeComponent();

            MineSweeper mine_sweeper = new();
            mine_sweeper.generate(new Size(10, 10), 10);
            

            (DataContext as MineSweeperViewModel).grid = this.FindControl<Grid>("MineSweeperGrid");

            (DataContext as MineSweeperViewModel).constructGame(mine_sweeper);

        }
    }
}
