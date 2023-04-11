using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BattleSweeper.ViewModels
{
    /// <summary>
    /// viewmodel for transitioning between minesweeper games.
    /// </summary>
    public class MineSweeperTransitionViewModel : ViewModelBase
    {
        public MineSweeperTransitionViewModel(int bombs_found, int total_bombs)
        {
            TransitionFinished = ReactiveCommand.Create( () => { }) ;
            this.m_bombs_found = bombs_found;
            this.m_total_bombs = total_bombs;
        }

        /// <summary>
        /// command called when the transition has finished.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TransitionFinished { get; set; }

        /// <summary>
        /// says mine sweeper has finished, and informs about number of bombs found.
        /// </summary>
        public string transitionText { get => $"Minesweeper finished!\nFound {m_bombs_found} out of {m_total_bombs} bombs!"; }

        protected int m_bombs_found;
        protected int m_total_bombs;
    }
}
