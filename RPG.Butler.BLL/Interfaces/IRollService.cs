using RPG.Butler.BLL.Models;
using System;

namespace RPG.Butler.BLL.Interfaces
{
    public interface IRollService
    {
        Random Randomizer { get; }

        string Roll(int diceCount, int? diceType, MarkType mark, int? modifier);
        int? Splitter(char splitter, string input, int index);
    }
}
