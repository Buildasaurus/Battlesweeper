using Games.Battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSweeper.ViewModels
{
    public class BattleShipsViewModel : ViewModelBase
    {
        public BattleShipsViewModel(IBattleships battleships)
        {
            this.battleships = battleships;
        }

        public bool Player1Playing { get; }
        public bool Player2Playing { get; }

        public IBattleships battleships;
    }
}
