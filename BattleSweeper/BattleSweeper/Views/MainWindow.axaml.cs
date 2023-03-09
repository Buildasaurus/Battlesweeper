using Avalonia.Controls;
using BattleSweeper.ViewModels;

namespace BattleSweeper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += (s, e) =>
            {
                //GameViewModel.stop();
            };
        }

        
    }
}
