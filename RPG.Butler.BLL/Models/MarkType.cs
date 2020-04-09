namespace RPG.Butler.BLL.Models
{
    public static class Mark
    {
        public static MarkType Get(char mark)
        {
            switch (mark)
            {
                case '+':
                    return MarkType.Plus;
                case '-':
                    return MarkType.Minus;
                case '/':
                    return MarkType.Division;
                case '*':
                    return MarkType.Multiplication;
                default:
                    return MarkType.None;
            }
        }

        public static char? CheckForMark(string input)
        {
            if (input.IndexOf((char)MarkType.Plus) > 0)
            {
                return (char)MarkType.Plus;
            }
            if (input.IndexOf((char)MarkType.Minus) > 0)
            {
                return (char)MarkType.Minus;
            }
            if (input.IndexOf((char)MarkType.Division) > 0)
            {
                return (char)MarkType.Division;
            }
            if (input.IndexOf((char)MarkType.Multiplication) > 0)
            {
                return (char)MarkType.Multiplication;
            }
            return null;
        }
    }

    public enum MarkType
    {
        None,
        Plus = '+',
        Minus = '-',
        Division = '/',
        Multiplication = '*'
    }

}
