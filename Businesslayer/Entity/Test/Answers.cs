using System.Globalization;
namespace BusinessLogicLayer.Entity.Test;

[Serializable]
public class Answers : List<string>, IFormattable
{
    #region Fields
    public const int maxCapacity = 4;
    public const string testFormat = "T";
    public const string answerFormat = "A";
    public const string defaultFormat = "D";
    public const string compareFormat = "C";
    #endregion

    #region Properties
    public int? RightAnswer { get; set; } = null;
    public int? UserAnswer { get; set; } = null;
    #endregion

    #region IFormattable
    public override string ToString()
    {
        return ToString(testFormat, CultureInfo.CurrentCulture);
    }
    public string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if(string.IsNullOrEmpty(format))
            format = defaultFormat;

        formatProvider ??= CultureInfo.CurrentCulture;

        string res = "";
        int index = 1;
        foreach(string answer in this)
        {
            res += $"{index})" + $" {answer};";
            switch(format.ToUpperInvariant())
            {
                case answerFormat:
                if(index - 1 == RightAnswer)
                    res += " +";
                break;
                case testFormat:
                if(index - 1 == UserAnswer)
                    res += " <--";
                break;
                case compareFormat:
                if(index - 1 == UserAnswer)
                    res += " <--";

                if(index - 1 == RightAnswer)
                    res += " +";
                break;
                case defaultFormat:
                default:
                break;

            }
            res += "  ";
            index++;
        }
        return res;
    }
    #endregion
}
