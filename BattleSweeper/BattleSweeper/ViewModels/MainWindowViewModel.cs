using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using Games.MineSweeper;

namespace BattleSweeper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10);

            m_mine_sweeper = new(mine_sweeper_model);

            View = m_mine_sweeper;
        }

        public string Greeting => "Welcome to Avalonia!";

        public ViewModelBase? View { get => m_view; set => this.RaiseAndSetIfChanged(ref m_view, value); }
        protected ViewModelBase? m_view;

        protected MineSweeperViewModel m_mine_sweeper;
    }


}
