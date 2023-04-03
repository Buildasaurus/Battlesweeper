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
        
            View = game;
        }
        // underlying field for View.
        protected ViewModelBase? m_view;

        /// <summary>
        /// binds to the main contentcontrol of the maindindow view.
        /// </summary>
        public ViewModelBase? View { get => m_view; set => this.RaiseAndSetIfChanged(ref m_view, value); }
        /// <summary>
        /// the game ivew model, that controls the app.
        /// </summary>
        protected GameViewModel game = new GameViewModel();

    }


}
