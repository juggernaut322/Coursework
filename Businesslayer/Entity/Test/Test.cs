using BusinessLogicLayer.Entity.Stats;
using System.Globalization;

namespace BusinessLogicLayer.Entity.Test;

[Serializable]
public class Test : IFormattable
{
    #region Questions
    private List<Question> _questions = new();

    public void Add(Question question)
    {
        _questions.Add(question);
    }
    public void RemoveAt(int index)
    {
        _questions.RemoveAt(index);
    }
    public void AddRange(IEnumerable<Question> questions)
    {
        _questions.AddRange(questions);
    }

    public int Count
    {
        get => Questions.Count;
    }
    public Question this[int index]
    {
        get => _questions[index];
        set => _questions[index] = value;
    }
    public List<Question> Questions
    {
        get => _questions;
        set => _questions = value;
    }

    public void ResetUserAnswers()
    {
        foreach(var question in _questions)
            question.ResetUserAnswer();
    }
    #endregion

    #region Statistic
    private Statistic _statistic = new();
    public Statistic Statistic
    {
        get => _statistic;
        set => _statistic = value;
    }
    public void AddStatistic(User user, Mark mark)
    {
        _statistic.Add(user, mark);
    }

    public string GetLastStatistic()
    {
        return _statistic[^1];
    }
    public void ClearStatistic()
    {
        _statistic.Clear();
    }
    public Statistic GetStatisticByUser(User user)
    {
        return Statistic.GetStatsByUser(user);
    }
    public Statistic GetStatisticByDate(DateTime time)
    {
        return Statistic.GetStatsByDate(time);
    }
    #endregion

    #region Object
    public override string ToString()
    {
        return ToString("D", CultureInfo.CurrentCulture);
    }
    public override bool Equals(object? obj)
    {
        Test? test = obj as Test;
        if(test is not null)
        {
            if(Questions.Count == test.Questions.Count)
            {
                for(int i = 0; i < Questions.Count; i++)
                {
                    if(!Questions[i].Equals(test.Questions[i]))
                        return false;
                }
                return true;
            }
        }
        return false;
    }
    public override int GetHashCode()
    {
        int res = 1;
        foreach(var questioin in _questions)
        {
            res *= questioin.GetHashCode();
        }
        return res;
    }
    #endregion

    #region IFormattable
    public string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if(string.IsNullOrEmpty(format))
            format = Answers.defaultFormat;

        formatProvider ??= CultureInfo.CurrentCulture;

        int index = 1;
        string res = "";
        foreach(var question in _questions)
        {
            res += $"{index++}.{question}";
            res += "\n\t";
            switch(format.ToUpperInvariant())
            {
                case Answers.answerFormat:
                res += question.Answers.ToString(Answers.answerFormat);
                break;

                case Answers.testFormat:
                res += question.Answers.ToString(Answers.testFormat);
                break;

                case Answers.compareFormat:
                res += question.Answers.ToString(Answers.compareFormat);
                break;

                case Answers.defaultFormat:
                default:
                res += question.Answers.ToString(Answers.defaultFormat);
                break;
            }
            res += "\n\n";
        }
        return res;
    }
    #endregion
}
