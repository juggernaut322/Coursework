using BusinessLogicLayer;
using BusinessLogicLayer.Entity.Stats;
using BusinessLogicLayer.Entity.Test;
using BusinessLogicLayer.Exceptions;
using ConsoleMenuBase;
using System.Collections;
using System.Globalization;

namespace PresentationLayer;

public class MyConsoleMenu : ConsoleMenu
{
    #region Fields
    static readonly Dictionary<string, Action> mainMenu = new();
    static readonly Dictionary<string, Action> changeMenu = new();
    static Interaction inter = new();
    static readonly Exception unknownCommand = new("Unknown Command!");
    #endregion

    public MyConsoleMenu()
    {
        mainMenu.Add("/info", () => Info());
        mainMenu.Add("/create", () => Create(false));
        mainMenu.Add("/create def", () => Create(true));
        mainMenu.Add("/change", () => StartChanging());
        mainMenu.Add("/pass", () => PassTest());
        mainMenu.Add("/stats", () => StartStats());
        mainMenu.Add("/show", () => ShowTest());
        mainMenu.Add("/get all", () => ShowQuestions());
        mainMenu.Add("/clear", () => ClearFile());
        mainMenu.Add("/cls", () => Console.Clear());
        mainMenu.Add("/end", () =>

        { Console.Clear(); Console.WriteLine("Bye, have a good time!"); });

        changeMenu.Add("/info", () => ChangeInfo());
        changeMenu.Add("/question", () => StartChangingQuestion());
        changeMenu.Add("/answers", () => StartChangingAnswers());
    }

    #region Start
    public override void Start()
    {
        Console.SetWindowSize(90, 20);
        Console.SetBufferSize(100, 30);
        string? input = "";
        do
        {
            Console.Clear();
            mainMenu["/info"]();
            try
            {
                Console.Write("Enter the command: ");
                input = Console.ReadLine();
                if(int.TryParse(input, out int number))
                    switch(number)
                    {
                        case 1:
                        input = "/info";
                        continue;
                        case 2:
                        input = "/create";
                        break;
                        case 3:
                        input = "/change";
                        break;
                        case 4:
                        input = "/pass";
                        break;
                        case 5:
                        input = "/stats";
                        break;
                        case 6:
                        input = "/show";
                        break;
                        case 7:
                        input = "/get all";
                        break;
                        case 8:
                        input = "/clear";
                        break;
                        case 9:
                        input = "/cls";
                        continue;
                        case 10:
                        input = "/end";
                        break;
                    }

                if(mainMenu.ContainsKey(input))
                    mainMenu[input]();
                else
                    throw unknownCommand;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        } while(input != "/end");
    }
    #endregion

    #region Info
    public override void Info()
    {
        string? info = "***** Course Work *****\n" +
            "1.info;\n" +
            "2.create;\n" +
            "3.change;\n" +
            "4.pass;\n" +
            "5.stats;\n" +
            "6.show test;\n" +
            "7.get all questions;\n" +
            "8.clear;\n" +
            "9.cls;\n" +
            "10.end.";
        Console.WriteLine(info);
    }
    private void ChangeInfo()
    {
        string? changeInfo = "***** Change *****\n" +
            $"file: {FilePath}\n" +
            "\n1.question;\n" +
            "2.answers;\n" +
            "3.select file path;\n" +
            "4.return";
        Console.WriteLine(changeInfo);
    }
    private void QuestionInfo()
    {
        Console.Clear();
        string? questionInfo =
            "***** Changing Question *****\n" +
            $"file: {FilePath}\n" +
            "\n1.add;\n" +
            "2.delete;\n" +
            "3.change;\n" +
            "4.return.";
        Console.WriteLine(questionInfo);
    }
    private void AnswerInfo(int questionIndex)
    {
        Console.Clear();
        var question = Test[questionIndex];
        string? answerInfo =
            "***** Changing Answers *****\n" +
            $"{{\nfile: {FilePath}\n" +
            $"question: {question}\n" +
            $"answers: \n{question.Answers:A}\n}}" +
            "\n1.add;\n" +
            "2.delete;\n" +
            "3.change;\n" +
            "4.set right answer;\n" +
            "5.select question;\n" +
            "6.return.";
        Console.WriteLine(answerInfo);
    }
    private void PassingInfo(User user)
    {
        string? passingInfo =
            "***** Passing the Test *****\n" +
            $"file: {FilePath}\n" +
            $"user: {user};\n" +
            "\n/next to go to next answer;\n" +
            "/prev to go to previous answer;\n" +
            "/return;\n";
        Console.WriteLine(passingInfo);
    }
    private static void StatisticInfo()
    {
        string? res = "***** Statistic *****\n" +
            "1.show;\n" +
            "2.clear;\n" +
            "3.show by user;\n" +
            "4.show by date;\n" +
            "5.select file path;\n" +
            "6.return.";
        Console.WriteLine(res);
    }
    #endregion

    #region Show
    //filled == true if we have already asked about file path
    public override void Show(IEnumerable collection)
    {
        int index = 1;
        foreach(var item in collection)
        {
            Console.WriteLine($"{index++}.{item}");
        }
    }
    public void ShowTest(bool filled = false, string? format = "D")
    {
        if(!filled && !AskForFilePath(ref inter))
            return;

        Console.WriteLine(Test.ToString(format));
    }
    private void ShowQuestions(bool filled = false)
    {
        if(!filled && !AskForFilePath(ref inter))
            return;

        Show(Test.Questions);
    }
    private void ShowQuestion(Test test, int questionIndex, string format = "T")
    {
        var question = test[questionIndex];
        string res = $"{questionIndex + 1}." + question + "\n\t" + question.Answers.ToString(format);
        Console.WriteLine(res);
    }

    /// <summary>
    /// show def statistic from test, or specific stats by user/date
    /// </summary>
    /// <param name="filled"></param>
    /// <param name="stats">is not null when show stats by user/date</param>
    private void ShowStatistic(bool filled = false, Statistic? stats = null)
    {
        if(!filled && !AskForFilePath(ref inter))
            return;

        stats ??= Test.Statistic;

        Console.WriteLine(stats.ToString());
    }
    #endregion

    #region Create
    /// <summary>
    /// ask file path,
    /// then clears file,
    /// then if def adds to file Interaction.DefList
    /// else calls AddQuestion
    /// </summary>
    /// <param name="def">defines whether to add def list or not</param>
    public void Create(bool def)
    {
        if(!AskForFilePath(ref inter))
            return;
        try
        {
            ClearFile(true);
        }
        catch
        {

        }

        if(def)
        {
            inter.AddTest(Interaction.DefTest);
            ShowTest(true, "A");
        }
        else
        {
            AddQuestion();
        }
    }
    #endregion

    #region Change
    /// <summary>
    /// asks file path, 
    /// then asks what to change,
    /// then calls specific methods for changing(question/answers)
    /// </summary>
    private void StartChanging()
    {
    askForFilePath:
        if(!AskForFilePath(ref inter))
            return;

        string? input = "";
        do
        {
            Console.Clear();
            changeMenu["/info"]();
            try
            {
                Console.Write("Enter what you want to change: ");
                input = Console.ReadLine();

                if(int.TryParse(input, out int number))
                    switch(number)
                    {
                        case 0:
                        input = "/info";
                        break;
                        case 1:
                        input = "/question";
                        break;
                        case 2:
                        input = "/answers";
                        break;
                        case 3:
                        goto askForFilePath;
                        case 4:
                        return;
                        case 5:
                        input = "/cls";
                        break;
                    }


                if(changeMenu.ContainsKey(input))
                    changeMenu[input]();
                else
                    throw unknownCommand;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        } while(true);
    }

    /// <summary>
    /// asks what to do,
    /// then calls specific methods(add, delete, change question)
    /// </summary>
    private void StartChangingQuestion()
    {
        do
        {
            string input = "";
            Console.Clear();
            QuestionInfo();

            Console.Write("Enter what you want to do: ");
            input = Console.ReadLine();

            if(input == "/return" || input == "4")
                return;

            if(!int.TryParse(input, out int number))
            {
                Console.WriteLine(unknownCommand.Message);
                Console.ReadKey();
                continue;
            }

            switch(number)
            {
                case 1:
                AddQuestion();
                break;
                case 2:
                DeleteQuestion();
                break;
                case 3:
                ChangeQuestion();
                break;
                default:
                Console.WriteLine(unknownCommand.Message);
                Console.ReadKey();
                continue;
            }
            Console.ReadKey();
        } while(true);
    }

    /// <summary>
    /// asks index of question,
    /// then asks what to do,
    /// then calls specific method (add, delete, change, setRight answer)
    /// </summary>
    private void StartChangingAnswers()
    {
    //ask question index while it is not valid
    askQuestionIndex:
        int questionIndex;
        do
        {
            Console.Clear();
            ShowQuestions(true);

            Console.Write("Enter index of question: ");
            string? input = Console.ReadLine();

            if(input == "/return" || input == "/end")
                return;

            var test = Test;

            if(!int.TryParse(input, out questionIndex))
            {
                Console.WriteLine(new QuestionException(input).Message);
                Console.ReadKey();
                continue;
            }

            --questionIndex; // because in list counting starts from 0 

            if(!Interaction.IsIndexValid(questionIndex, test.Questions))
            {
                Console.WriteLine(new QuestionException(questionIndex).Message);
                Console.ReadKey();
                continue;
            }

            break;
        } while(true);

        do
        {
            AnswerInfo(questionIndex);

            int number;
            string input;

            Console.Write("Enter what you want to do: ");
            input = Console.ReadLine();
            if(input == "/select" || input == "5")
                goto askQuestionIndex;
            if(input == "/end" || input == "6")
                return;

            if(!int.TryParse(input, out number))
            {
                Console.WriteLine(unknownCommand.Message);
                Console.ReadKey();
                continue;
            }

            switch(number)
            {
                case 1:
                AddAnswer(questionIndex);
                break;
                case 2:
                DeleteAnswer(questionIndex);
                break;
                case 3:
                ChangeAnswer(questionIndex);
                break;
                case 4:
                SetRightAnswer(questionIndex);
                break;
                default:
                Console.WriteLine(unknownCommand.Message);
                Console.ReadKey();
                continue;
            }

        } while(true);
    }
    #endregion

    #region Changing Question
    /// <summary>
    /// asks for question while user do not enter /return
    /// </summary>
    private void AddQuestion()
    {
        Question question;

        do
        {
            Console.Clear();

            Console.WriteLine("***** Add Question *****" +
                $"\nfile: {FilePath}\n");

            //if we add first question in test do not show all questions
            if(inter.Count != -1)
                ShowQuestions(true);

            Console.Write("Enter your question or /return to stop" +
                "\n(your question should star with upper case letter and end with ?(question mark))\n: ");
            string questionString = Console.ReadLine();

            if(questionString == "/return" || questionString == "/end")
                return;

            try
            {
                question = Interaction.CreateQuestion(questionString);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                continue;
            }

            inter.AddQuestion(question);

            int questionIndex = Test.Questions.IndexOf(question);

            AddAnswer(questionIndex);
            ShowTest(true, "A");
            Console.ReadKey();
        } while(true);
    }

    /// <summary>
    /// deletes questions while test has more than 0 or while user do not enter /return
    /// </summary>
    private void DeleteQuestion()
    {
        do
        {
            string input;

            Console.Clear();

            Console.WriteLine("***** Delete Question *****" +
                $"\nfile: {FilePath}\n");
            ShowQuestions(true);
            Console.Write("Enter index of question which you want to delete" +
                "\nor /return to stop: ");
            input = Console.ReadLine();

            if(input == "/return" || input == "/end")
                return;

            //if user enter not digit
            if(!int.TryParse(input, out int questionIndex))
            {
                Console.WriteLine(new QuestionException(input).Message);
                Console.ReadKey();
                continue;
            }

            --questionIndex; // because in list counting starts from 0
            try
            {
                inter.DeleteQuestion(questionIndex);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                continue;
            }

            if(inter.Count > 0)
                continue;

            ShowQuestions(true);
        } while(true);
    }

    /// <summary>
    /// changes selected by user question
    /// </summary>
    private void ChangeQuestion()
    {
        // enter index of question while it is not valid
        do
        {
            string input;

            Console.Clear();

            Console.WriteLine("***** Change Question *****" +
                $"\nfile: {FilePath}\n");

            ShowQuestions(true);

            Console.Write("Enter index of question which you want to change" +
                "\nor /return to stop: ");
            input = Console.ReadLine();

            if(input == "/return" || input == "/end")
                return;

            //if user enter not digit
            if(!int.TryParse(input, out int questionIndex))
            {
                Console.WriteLine(new QuestionException(input).Message);
                Console.ReadKey();
                continue;
            }

            --questionIndex; // because in list counting starts from 0

            if(Interaction.IsIndexValid(questionIndex, Test.Questions))
            {
            // enter new question while it is not valid
            loop2:
                Console.Write("Enter new question: ");
                string question = Console.ReadLine();

                try
                {
                    inter.ChangeQuestion(questionIndex, Interaction.CreateQuestion(question));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    goto loop2;
                }
            }
            else
            {
                Console.WriteLine(new QuestionException(questionIndex).Message);
                Console.ReadKey();
                continue;
            }
            ShowQuestions(true);
        } while(true);
    }
    #endregion

    #region Changing Answers
    /// <summary>
    /// enter answer while answers.Count less then Answer.maxCapacity or user does not enter return,
    /// then asks right answer
    /// </summary>
    /// <param name="questionIndex">Index of question to which answers are added</param>
    private void AddAnswer(int questionIndex)
    {
        do
        {
            var test = Test;
            var question = test[questionIndex];

            if(question.Answers.Count < Answers.maxCapacity)
            {
                Console.Clear();
                Console.WriteLine("***** Add Answer *****" +
                $"\nfile: {FilePath}\n");

                ShowQuestion(test, questionIndex);
                Console.Write("Enter your answer" +
                    "\nor /return to stop: ");
                string input = Console.ReadLine();
                if(input == "/end" || input == "/return")
                {
                    break;
                }
                else
                {
                    inter.AddAnswer(questionIndex, input);
                    continue;
                }
            }
            //enter index of right answer while it is not valid
            SetRightAnswer(questionIndex);
            break;
        } while(true);
    }

    /// <summary>
    /// deletes answers while question has more than 0 or while user does not enter /return,
    /// then asks for right answer
    /// </summary>
    /// <param name="questionIndex"></param>
    private void DeleteAnswer(int questionIndex)
    {
    //enter index of answer while it is not valid
    deleteAnswer:
        var answers = Test[questionIndex].Answers;
        int answerIndex = 0;

        //if user enter /return stop deleting answers and ask for right answer
        if(!AskAnswerIndex("***** Delete Answer *****", ref answerIndex, questionIndex))
            goto setRightAnswer;

        int count = answers.Count;
        try
        {
            inter.DeleteAnswer(questionIndex, answerIndex);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
            goto deleteAnswer;
        }

        if(--count > 0)
            goto deleteAnswer;

        setRightAnswer:
        //enter index of right answer while it is not valid
        SetRightAnswer(questionIndex);
    }

    /// <summary>
    /// changes answer selected by user,
    /// then asks for right answer
    /// </summary>
    /// <param name="questionIndex"></param>
    private void ChangeAnswer(int questionIndex)
    {
    //enter index of answer while it is not valid
    changeAnswer:
        var answers = Test[questionIndex].Answers;
        int answerIndex = 0;

        if(!AskAnswerIndex("***** Change Answer *****", ref answerIndex, questionIndex))
            goto setRightAnswer;

        if(!Interaction.IsIndexValid(answerIndex, answers))
        {
            Console.WriteLine(new AnswerException(answerIndex).Message);
            Console.ReadKey();
            goto changeAnswer;
        }

        Console.Write("Enter new answer: ");
        inter.ChangeAnswer(questionIndex, answerIndex, Console.ReadLine());

    setRightAnswer:
        SetRightAnswer(questionIndex);
    }

    /// <summary>
    /// while index is not valid ask for right answer(1 - answers.Count),
    /// if index valid sets right answer and returns
    /// </summary>
    /// <param name="questionIndex"></param>
    private void SetRightAnswer(int questionIndex)
    {
    loop:
        var question = Test[questionIndex];
        var answers = question.Answers;

        //if user has deleted all answers set right answer to null
        if(answers.Count <= 0)
        {
            inter.ResetRightAnswer(questionIndex);
            return;
        }

        Console.Clear();
        Console.WriteLine("***** Set Right Answer *****" +
            $"\nfile: {FilePath}\n");

        ShowQuestion(Test, questionIndex, "A");
        Console.Write($"Please enter right answer (1-{answers.Count})\n" +
                $"or /return to stop: ");

        string? rightAnswer = Console.ReadLine();
        if(rightAnswer == "/return" || rightAnswer == "/end")
            return;

        if(!int.TryParse(rightAnswer, out int rightAnswerIndex))
        {
            Console.WriteLine(new AnswerException(rightAnswer).Message);
            Console.ReadKey();
            goto loop;
        }

        --rightAnswerIndex;

        try
        {
            inter.SetRightAnswer(questionIndex, rightAnswerIndex);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadKey();
            goto loop;
        }
    }

    /// <summary>
    /// while index is not valid ask for answer(1 - answers.Count)
    /// </summary>
    /// <param name="answerIndex">ref not out because if user enters /return or /end answerIndex is not set</param>
    /// <param name="answers"></param>
    /// <returns></returns>
    private bool AskAnswerIndex(string info, ref int answerIndex, int questionIndex)
    {
        string input;
        do
        {
            Console.Clear();
            Console.WriteLine(info);
            Console.WriteLine($"file: {FilePath}\n");
            ShowQuestion(Test, questionIndex);
            Console.Write("Enter index of answer" +
                "\nor /return to stop: ");

            input = Console.ReadLine();
            if(input == "/return" || input == "/end")
                return false;

            if(!int.TryParse(input, out answerIndex))
            {
                Console.WriteLine(new AnswerException(input).Message);
                Console.ReadKey();
            }
            else
                break;
        } while(true);

        answerIndex--;
        return true;
    }
    #endregion

    #region Pass
    private static User? AskUserData()
    {
        User user;
        do
        {
            Console.Write("Enter your first name: ");
            string? firstName = Console.ReadLine();

            if(firstName == "/return" || firstName == "/end")
                return null;

            Console.Write("Enter your last name: ");
            string? lastName = Console.ReadLine();
            if(lastName == "/return" || lastName == "/end")
                return null;

            try
            {
                user = Interaction.CreateUser(firstName, lastName);
                return user;
            }
            catch(UserException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("First and Last name must start with upper case letter and contain only letters!");
                Console.ReadKey();
            }
        } while(true);
    }
    private void PassTest()
    {

    //ask for file path while file does not contain test
    loop:
        if(!AskForFilePath(ref inter))
            return;

        inter.ResetUserAnswers();
        Console.Clear();

        Test test;
        try
        {
            test = Test;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadKey();
            goto loop;
        }

        string? rightAnswers = inter.CheckForRightAnswers();

        if(rightAnswers is not null)
        {
            Console.WriteLine(rightAnswers);
            Console.ReadKey();
            goto loop;
        }


        User? user = AskUserData();
        // returns null when user enters /return or /end
        if(user is null) 
            goto loop;

        int questionIndex = 0;

        //answer the questions till user enter /end or /return 
        while(true)
        {
            Console.Clear();
            PassingInfo(user);

            // if user has answered to the last question return to first question
            if(questionIndex == inter.Count)
                questionIndex = 0;

            
            string input;

            ShowQuestion(Test, questionIndex);

            Console.Write("Enter\n" + ":");
            input = Console.ReadLine();

            if(input == "/next")
            {
                if(questionIndex == inter.Count - 1)
                    questionIndex = 0;
                else
                    questionIndex++;
                continue;
            }

            if(input == "/prev")
            {
                if(questionIndex == 0)
                    questionIndex = inter.Count - 1;
                else
                    questionIndex--;
                continue;
            }

            if(input == "/end" || input == "/return")
                break;

            //int answerIndex;
            if(!int.TryParse(input, out int answerIndex))
            {
                Console.WriteLine(new AnswerException(input).Message);
                Console.ReadKey();
                continue;
            }

            --answerIndex;

            try
            {
                inter.SetUserAnswer(questionIndex, answerIndex);
                questionIndex++;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        inter.CalculatePersentOfRightAnswers(DateTime.Now, user);

        ShowTest(true, "C");

        string res = Test.GetLastStatistic();
        Console.WriteLine(res);
    }
    #endregion

    #region Stats
    private void StartStats()
    {
    askForFilePath:
        if(!AskForFilePath(ref inter))
            return;

        string? input = "";
        do
        {
            Console.Clear();
            StatisticInfo();
            try
            {

                Console.Write("Enter what you want to do: ");
                input = Console.ReadLine();

                if(int.TryParse(input, out int number))
                {
                    switch(number)
                    {
                        case 1:
                        Console.Clear();
                        ShowStatistic(true);
                        break;
                        case 2:
                        ClearStats();
                        break;
                        case 3:
                        GetStatsByUser();
                        break;
                        case 4:
                        GetStatsByDate();
                        break;
                        case 5:
                        goto askForFilePath;
                        case 6:
                        input = "/return";
                        return;
                        case 7:
                        mainMenu["/cls"]();
                        break;
                        default:
                        throw unknownCommand;
                    }
                }
                else
                {
                    throw unknownCommand;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        } while(input != "/return");

    }
    private void ClearStats()
    {
        var test = Test;
        test.ClearStatistic();

        ClearFile(true);
        inter.AddTest(test);
    }
    private void GetStatsByUser()
    {
        Statistic stats;

        Console.Clear();
        var user = AskUserData();
        if(user is null)
            return;

        stats = Test.GetStatisticByUser(user);

        ShowStatistic(true, stats);
    }
    private void GetStatsByDate()
    {
        DateTime time;
        string? date;
        do
        {
            Console.Clear();
            Console.Write("Enter date\n" +
            "(date format (date.month.year)\n:");
            date = Console.ReadLine();

        } while(!DateTime.TryParse(date, new CultureInfo("uk-UA"), DateTimeStyles.None, out time));
        //because on my PC is USA culture 

        var stats = Test.GetStatisticByDate(time);
        ShowStatistic(true, stats);
    }
    #endregion

    #region Auxiliary Methods
    private bool AskForFilePath(ref Interaction inter)
    {
    loop:
        Console.Clear();
        Console.Write("Enter file path or /return to end" +
            "\n(file can be: dat, xml or json): ");

        string? filePath = Console.ReadLine();
        if(filePath == "/return" || filePath == "/end")
            return false;
        try
        {
            inter.FilePath = filePath;
            this.FilePath = inter.FilePath;
        }
        catch
        {
            Console.WriteLine(Interaction.wrongFile.Message);
            Console.ReadKey();
            goto loop;
        }

        return true;
    }
    private void ClearFile(bool filled = false)
    {
        if(!filled && !AskForFilePath(ref inter))
            return;

        inter.ClearFile();
    }
    private string? FilePath { get; set; }
    private static Test Test
    {
        get => inter.GetTest();
    }
    #endregion
}
