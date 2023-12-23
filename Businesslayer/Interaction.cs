using BusinessLogicLayer.Entity.Stats;
using BusinessLogicLayer.Entity.Test;
using BusinessLogicLayer.Exceptions;
using DataAccess;
using System.Collections;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer;
public class Interaction
{
    #region Fields 
    string? _filePath;
    string? _fileExtension;

    public static readonly ArgumentException wrongFile = new("File or file extension is not valid!");
    public static readonly QuestionException wrongQuestion = new();
    public static readonly AnswerException wrongAnswer = new();

    static readonly Dictionary<string, Func<string?, object?>> deser = new()
    {
        { ".xml", (filePath) => new XMLProvider(typeof(Test), new Type[]
        {
            typeof(Question), typeof(Answers), typeof(Statistic), typeof(User), typeof(Mark)
        }).Deserialize(filePath) },

        { ".dat", (filePath) => new BinaryProvider(typeof(Test)).Deserialize(filePath) },
        { ".json", (filePath) => new JSONProvider(typeof(Test)).Deserialize(filePath) }
    };
    static readonly Dictionary<string, Action<object, string?>> ser = new()
    {
        {".xml", (graph, filePath) => new XMLProvider(typeof(Test), new Type[]
        {
            typeof(Question), typeof(Answers), typeof(Statistic),typeof(User), typeof(Mark)
        }).Serialize(graph, filePath) },

        {".dat", (graph, filePath) => new BinaryProvider(typeof(Test)).Serialize(graph, filePath) },
        {".json", (graph, filePath) => new JSONProvider(typeof(Test)).Serialize(graph, filePath) }
    };

    static readonly Regex validQuestion = new(@"^[A-Z]{1}[ \w\W +\-:*\\|/^]+\?$");
    #endregion

    #region Ctors
    public Interaction()
    {

    }
    public Interaction(string filePath)
    {
        FilePath = filePath;
    }
    #endregion

    #region Auxiliary Methods
    public string? FilePath
    {
        get => _filePath;
        set
        {
            string? extension = Path.GetExtension(value);
            if(extension == ".xml" || extension == ".dat" || extension == ".json")
            {
                _filePath = value;
                _fileExtension = extension;
            }
            else
                throw wrongFile;
        }
    }
    public static Test DefTest
    {
        get
        {
            Test test = new();
            test.Add(new Question("What is 2+2?") { Answers = { "1", "2", "4", "5" }, RightAnswer = 2 });

            test.Add(new Question("What is capital of Ukraine?") { Answers = { "Kyiv", "Odesa", "London", "New-York" }, RightAnswer = 0 });

            test.Add(new Question("What is the second letter of alphabet?") { Answers = { "A", "B", "C", "D" }, RightAnswer = 1 });
            return test;
        }
    }
    public static bool IsQuestionValid(string question)
    {
        return validQuestion.IsMatch(question);
    }
    public static bool IsIndexValid(int index, IList list)
    {
        return index >= 0 && index < list.Count;
    }
    public void ClearFile() => DataProvider.ClearFile(_filePath);
    public int Count
    {
        get
        {
            try
            {
                return GetTest().Count;
            }
            catch
            {
                return -1;
            }
        }
    }
    #endregion

    #region Test
    public void AddTest(Test test)
    {
        if(File.Exists(_filePath))
            ClearFile();

        ser[_fileExtension](test, _filePath);
    }
    public Test? GetTest()
    {
        try
        {
            var test = deser[_fileExtension](_filePath) as Test;
            return test is not null ? test : throw new InvalidOperationException("File contains another data!");
        }
        catch(FileNotFoundException)
        {
            throw new FileNotFoundException("File not found!");
        }
    }
    public void CalculatePersentOfRightAnswers(DateTime time, User user)
    {
        int count = 0;
        Test test = GetTest();

        foreach(Question question in test.Questions)
        {
            if(question.RightAnswer == question.UserAnswer)
                count++;
        }

        double value = ((double)count / test.Count) * 100.0;

        Mark mark = new(value, time);

        test.AddStatistic(user, mark);

        AddTest(test);
    }
    public void ResetUserAnswers()
    {
        var test = deser[_fileExtension](_filePath) as Test;
        test.ResetUserAnswers();
        AddTest(test);
    }
    #endregion

    #region Question 
    public void AddQuestion(Question question)
    {
        Test? test;
        if(File.Exists(_filePath))
        {
            test = deser[_fileExtension](_filePath) as Test;

            test ??= new();
        }
        else
            test = new();

        test.Add(question);
        AddTest(test);
    }
    public void DeleteQuestion(int index)
    {
        var test = deser[_fileExtension](_filePath) as Test;

        if(IsIndexValid(index, test.Questions))
        {
            test.RemoveAt(index);
            AddTest(test);
        }
        else
            throw new QuestionException(index);
    }
    public void ChangeQuestion(int index, Question newQuestion)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(index, test.Questions) && IsQuestionValid(newQuestion))
        {
            test[index].Value = newQuestion;
            AddTest(test);
        }
        else if(!IsIndexValid(index, test.Questions))
            throw new QuestionException(index);
        else if(!IsQuestionValid(newQuestion))
            throw wrongQuestion;
    }
    public static Question CreateQuestion(string question) => IsQuestionValid(question) ? new Question(question) : throw wrongQuestion;
    #endregion

    #region Answers

    #region AddAnswer 
    public void AddAnswer(int questionIndex, string answer)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            Question question = test[questionIndex];

            AddAnswer(ref question, answer);

            test[questionIndex] = question;
            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    static void AddAnswer(ref Question question, string answer)
    {
        if(question.Answers.Count < Answers.maxCapacity)
            question.Add(answer);
        else
            throw wrongAnswer;
    }
    #endregion

    #region DeleteAnswer
    public void DeleteAnswer(int questionIndex, int answerIndex)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            Question question = test[questionIndex];

            DeleteAnswer(ref question, answerIndex);

            test[questionIndex] = question;
            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    static void DeleteAnswer(ref Question question, int index)
    {
        if(IsIndexValid(index, question.Answers))
            question.Answers.RemoveAt(index);
        else
            throw new AnswerException(index);
    }
    #endregion

    #region SetRightAnswer
    public void SetRightAnswer(int questionIndex, int answerIndex)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            Question question = test[questionIndex];

            SetRightAnswer(ref question, answerIndex);

            test[questionIndex] = question;

            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    static void SetRightAnswer(ref Question question, int index)
    {
        if(IsIndexValid(index, question.Answers))
            question.Answers.RightAnswer = index;
        else
            throw new AnswerException(index);
    }
    #endregion

    #region SetUserAnswer
    public void SetUserAnswer(int questionIndex, int answerIndex)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            Question question = test[questionIndex];

            SetUserAnswer(ref question, answerIndex);

            test[questionIndex] = question;

            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    static void SetUserAnswer(ref Question question, int index)
    {
        if(IsIndexValid(index, question.Answers))
            question.Answers.UserAnswer = index;
        else
            throw new AnswerException(index);
    }
    #endregion

    #region ChangeAnswer
    public void ChangeAnswer(int questionIndex, int answerIndex, string answer)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            Question question = test[questionIndex];

            ChangeAnswer(ref question, answerIndex, answer);
            test[questionIndex] = question;

            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    static void ChangeAnswer(ref Question question, int index, string answer)
    {
        if(IsIndexValid(index, question.Answers))
            question.Answers[index] = answer;
        else
            throw new AnswerException(index);
    }
    #endregion

    public void ResetRightAnswer(int questionIndex)
    {
        var test = deser[_fileExtension](_filePath) as Test; // never null because before this method always GetTest() being called

        if(IsIndexValid(questionIndex, test.Questions))
        {
            test[questionIndex].RightAnswer = null;
            AddTest(test);
        }
        else
            throw new QuestionException(questionIndex);
    }
    #endregion

    public static User CreateUser(string firstName, string lastName)
    {
        Regex validName = new(@"^[A-Z]{1}[a-z]+$");
        if(!validName.IsMatch(firstName) && !validName.IsMatch(lastName))
            throw new UserException(nameof(firstName), nameof(lastName));

        if(!validName.IsMatch(firstName))
            throw new UserException(nameof(firstName));

        if(!validName.IsMatch(lastName))
            throw new UserException(nameof(lastName));

        return new User(firstName, lastName);
    }

    /// <summary>
    /// if test has question without right answers returns their indexes 
    /// </summary>
    /// <returns></returns>
    public string? CheckForRightAnswers()
    {
        List<int> answers = GetNotExistingRightAnswers();

        string res = null;

        if(answers.Count != 0)
        {
            res = "Questions: ";
            foreach(int i in answers)
                res += $"{i + 1}, ";

            res += "don't have right answers!";
        }
        return res;
    }
    public List<int> GetNotExistingRightAnswers()
    {
        List<int> answers = new();
        var test = deser[_fileExtension](_filePath) as Test;

        for(int i = 0; i < test.Questions.Count; i++)
        {
            if(!test.Questions[i].CheckForRightAnswer())
            {
                answers.Add(i);
            }
        }
        return answers;
    }
}
