using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RPG.Butler.Al.Modules
{
	public class Roll : ModuleBase<SocketCommandContext>
    {
		[Command("roll")]
		public async Task RollAsync(string equation)
		{
			var total = 0;
			var totalRolls = "";
			var diceNumber = Splitter('k', equation, 0);
			var dice = 0;
			var add = true;
			if(equation.Contains('+'))
			{
				dice = Splitter('+', equation.Split('k')[1], 0);
			} else if(equation.Contains('-'))
			{
				add = false;
				dice = Splitter('-', equation.Split('k')[1], 0);
			} else
			{
				dice = Splitter('k', equation, 1);
			}
			var modifier = 0;
			if (equation.Contains('+'))
			{
				modifier = Splitter('+', equation.Split('k')[1], 1);
			}
			else if (equation.Contains('-'))
			{
				modifier = Splitter('-', equation.Split('k')[1], 1);
			}
			else
			{
				modifier = 0;
			}
			Random r = new Random();
			for (int i = 0; i < diceNumber; i++)
			{
				var roll = r.Next(1, dice + 1);
				total += roll;
				totalRolls += $" [{roll}] ";
			}
			var modifString = "";
			if(modifier > 0)
			{
				var decision = "";
				if(add)
				{
					decision = "+";
					total += modifier;
				} else
				{
					decision = "-";
					total -= modifier;
				}
				modifString += $" modyfikator dodany do rzutu: [{decision}{modifier}]";
			}
			totalRolls = (diceNumber > 1) ? $" składał się z :{totalRolls}" : "";

			Color color = new Color(r.Next(256), r.Next(256), r.Next(256));

			EmbedBuilder builder = new EmbedBuilder();
			builder.WithDescription($"Wynik dla ciebie {Context.User.Mention}! : [{total}]{totalRolls}{modifString}!")
				.WithColor(color);

			await ReplyAsync("", false, builder.Build());
		}

		private int Splitter(char splitter, string input, int index)
		{
			return int.Parse(input.Split(splitter)[index]);
		}
	}
}
