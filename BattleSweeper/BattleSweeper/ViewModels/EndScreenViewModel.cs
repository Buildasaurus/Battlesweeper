using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BattleSweeper.ViewModels
{
	public class EndScreenViewModel : ViewModelBase
	{
		public EndScreenViewModel(string textmessage)
		{
			NewGame = ReactiveCommand.Create(() => { });
			Text = textmessage;
		}
		public string Text { get; set; }
		public string Button { get; set; }
		public ReactiveCommand<Unit, Unit> NewGame { get; set; }
	}

}
