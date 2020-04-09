using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Models;
using System;
using System.Collections.Generic;

namespace RPG.Butler.BLL.Services
{
    public class RollService : IRollService
    {
        public Random Randomizer;
        public RollService()
        {
            Randomizer = new Random();
        }

        public string Roll(int diceCount, int? diceType, MarkType mark, int? modifier)
        {
            int modifiedTotal = 0;
            var rolledDice = RollDice(diceCount, diceType);
            if (modifier.HasValue && mark != MarkType.None)
                modifiedTotal = AddModifier(rolledDice.Key, modifier.Value, mark);

            return MakeMessage(rolledDice, modifiedTotal, mark, modifier);
        }

        private string MakeMessage(KeyValuePair<int, int[]> rolledDice, int modifiedTotal, MarkType mark, int? modifier)
        {
            var msg = "";
            var total = modifiedTotal != 0 ? modifiedTotal : rolledDice.Key;
            var totalRolls = "";
            var modifString = $" modyfikator dodany do rzutu: [{(char)mark}{modifier}]";
            foreach (var roll in rolledDice.Value)
            {
                totalRolls += $" [{roll}] ";
            }
            msg += $"[{total}]";
            if (rolledDice.Value.Length > 1)
                msg += $"składał się z: {totalRolls}";
            if (modifier.HasValue && mark != MarkType.None)
                msg += modifString;
            return msg;
        }

        public int? Splitter(char splitter, string input, int index)
        {
            var splitted = input.Split(splitter)[index];
            if (!string.IsNullOrEmpty(splitted))
            {
                return int.Parse(splitted);
            }
            return null;
        }

        private int AddModifier(int total, int modifier, MarkType mark)
        {
            switch (mark)
            {
                case MarkType.Plus:
                    return total += modifier;
                case MarkType.Minus:
                    return total -= modifier;
                case MarkType.Division:
                    return total /= modifier;
                case MarkType.Multiplication:
                    return total *= modifier;
            }
            return 0;
        }

        private KeyValuePair<int, int[]> RollDice(int diceCount, int? diceType)
        {
            int total = 0;
            List<int> totalTab = new List<int>();
            for (int i = 0; i < diceCount; i++)
            {
                var roll = Randomizer.Next(1, diceType.Value + 1);
                total += roll;
                totalTab.Add(roll);
            }
            return new KeyValuePair<int, int[]>(total, totalTab.ToArray());
        }
    }
}
