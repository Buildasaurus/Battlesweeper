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
        public MineSweeperTransitionViewModel()
        {
            TransitionFinished = ReactiveCommand.Create( () => { }) ;
        }

        /// <summary>
        /// command called when the transition has finished.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TransitionFinished { get; set; }

        

    }
}
