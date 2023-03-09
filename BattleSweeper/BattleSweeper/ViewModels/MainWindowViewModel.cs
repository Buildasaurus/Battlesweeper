using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using Games.MineSweeper;

namespace BattleSweeper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() {
        
            View = game;
        }
        protected ViewModelBase? m_view;

        public ViewModelBase? View { get => m_view; set => this.RaiseAndSetIfChanged(ref m_view, value); }
        protected GameViewModel game = new GameViewModel();

    }


}
