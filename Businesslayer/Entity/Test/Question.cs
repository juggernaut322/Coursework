namespace BusinessLogicLayer.Entity.Test;

[Serializable]
public class Question
{
    #region Fields
    string value = "";
    Answers _answers = new();
    #endregion

    #region Ctors
    public Question()
    {

    }
    public Question(string question)
    {
        value = question;
    }
    public Question(string question, Answers answers)
        : this(question)
    {
        _answers = answers;
    }
    #endregion

    #region Properties
    public string Value
    {
        get => value;
        set => this.value = value;
    }
    public string this[int index]
    {
        get => Answers[index];
        set => Answers[index] = value;
    }

    public Answers Answers
    {
        get => _answers;
        set => _answers = value;
    }
    public int? RightAnswer
    {
        get => _answers.RightAnswer;
        set => _answers.RightAnswer = value;
    }
    public int? UserAnswer
    {
        get => _answers.UserAnswer;
        set => _answers.UserAnswer = value;
    }
    #endregion

    #region Methods
    public void Add(string answer)
    {
        _answers.Add(answer);
    }
    public void ResetUserAnswer()
    {
        UserAnswer = null;
    }

    //returns true if right answer is set
    public bool CheckForRightAnswer()
    {
        return RightAnswer is not null;
    }
    #endregion

    #region Object
    public override string ToString() => value;
    public override bool Equals(object? obj) => Value.Equals((obj as Question).Value);
    public override int GetHashCode() => Value.GetHashCode();
    #endregion

    #region Cast
    public static implicit operator Question(string question)
    {
        return new Question(question);
    }
    public static implicit operator string(Question question)
    {
        return question.Value;
    }
    #endregion
}
