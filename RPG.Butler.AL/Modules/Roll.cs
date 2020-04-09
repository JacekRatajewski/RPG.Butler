using Discord;
using Discord.Commands;
using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Models;
using RPG.Butler.BLL.Services;
using System.Threading.Tasks;

namespace RPG.Butler.Al.Modules
{
	public class Roll : ModuleBase<SocketCommandContext>
	{
		private IRollService _roller;

		public Roll()
		{
			_roller = new RollService();
		}

		[Command("roll")]
		public async Task RollAsync(string equation)
		{
			int? modifier = null;
			var mark = Mark.Get(Mark.CheckForMark(equation) ?? '~');
			var diceNumber = _roller.Splitter('k', equation, 0) ?? 1;
			var dice = mark == MarkType.None ? _roller.Splitter('k', equation, 1) : _roller.Splitter('k', equation.Split((char)mark)[0].ToString(), 1);
			if (mark != MarkType.None)
			{
				modifier = _roller.Splitter((char)mark, equation, 1);
			}
			var rolled = _roller.Roll(diceNumber, dice, mark, modifier);
			Color color = new Color(_roller.Randomizer.Next(256), _roller.Randomizer.Next(256), _roller.Randomizer.Next(256));

			EmbedBuilder builder = new EmbedBuilder();
			builder.WithDescription($"Wynik dla ciebie {Context.User.Mention} to : {rolled}!")
				.WithColor(color);

			await ReplyAsync("", false, builder.Build());
		}
	}
}
