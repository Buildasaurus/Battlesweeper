﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BattleSweeper.ViewModels
{
    public class StartScreenViewModel : ViewModelBase
    {
        public StartScreenViewModel()
        {
            TransitionFinished = ReactiveCommand.Create(() => { });
        }
        public ReactiveCommand<Unit, Unit> TransitionFinished { get; set; }
    }
}
