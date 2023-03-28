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
        public BattleShipsViewModel(IBattleships bs_player_1, IBattleships bs_player_2)
        {
            this.bs_player_1 = bs_player_1;
            this.bs_player_2 = bs_player_2;
        }

        public bool Player1Playing { get; }
        public bool Player2Playing { get; }

        public IBattleships bs_player_1;
        public IBattleships bs_player_2;
    }
}
