using BusinessLogicLayer.Entity.Stats;
using BusinessLogicLayer.Entity.Test;
using BusinessLogicLayer.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace BusinessLogicLayer.Tests;

[TestClass()]
public class InteractionTests
{
    #region Fields
    static Interaction inter;

    static Answers answers;
    static Answers answers1;
    static Answers answers2;

    static Question question;
    static Question question1;
    static Question question2;

    static Test test;
    #endregion

    #region Preparation/CleanUp
    [TestInitialize]
    public void TestInit()
    {
        File.Create("file.xml").Close();
        answers = new() { "A", "B", "C" };
        answers1 = new() { "1", "0", "-10" };
        answers2 = new() { "a", "b", "c" };

        question = new Question("How are you?") { Answers = answers };
        question1 = new Question("What is number of letter A in alphabet?") { Answers = answers1 };
        question2 = new Question("What is lover case letter of D?") { Answers = answers2 };

        test = Interaction.DefTest;
        inter = new("file.xml");
        //inter.AddTest(test);
    }
    [TestCleanup]
    public void TestCleanUp()
    {
        File.Delete("file.xml");
    }
    #endregion

    [TestMethod()]
    public void InteractionTest()
    {
        inter = new();
    }

    #region IsQuestionValid
    [TestMethod()]
    public void IsQuestionValid_Success()
    {
        //arrange
        bool expected = true;

        //act
        var question = Interaction.CreateQuestion("How old are 123 you?");
        bool actual = Interaction.IsQuestionValid(question);

        //assert
        Assert.AreEqual(expected, actual, question);
    }

    [TestMethod()]
    [DataRow("How are you!")]
    [DataRow("How are you.")]
    [DataRow("How are you")]
    [DataRow("1ow are you!")]
    [DataRow("how are you!")]
    [DataRow("1How are you?")]
    public void IsQuestionValid_Fail(string questionS)
    {
        //arrange
        bool expected = false;

        //act
        bool actual = Interaction.IsQuestionValid(questionS);

        //assert
        Assert.AreEqual(expected, actual);
    }
    #endregion

    #region IsIndexValid
    [TestMethod()]
    public void IsIndexValid_2_Success()
    {
        //arrange
        int index = 2;

        //act
        bool actual = Interaction.IsIndexValid(index, test[0].Answers);

        //assert
        Assert.IsTrue(actual);
    }

    [TestMethod()]
    public void IsIndexValid_Negative1_Fail()
    {
        //arrange
        int index = -1;

        //act
        bool actual = Interaction.IsIndexValid(index, test.Questions);

        //assert
        Assert.IsFalse(actual);
    }

    [TestMethod()]
    public void IsIndexValid_6_Fail()
    {
        //arrange
        int index = 6;

        //act
        Debug.WriteLine(test.Count);
        bool actual = Interaction.IsIndexValid(index, test.Questions);

        //assert
        Assert.IsFalse(actual);
    }
    #endregion

    #region AddTest
    [TestMethod()]
    public void Add_1Test_Success()
    {
        //arrange
        var expected = Interaction.DefTest;
        //act
        inter.AddTest(test);
        var actual = inter.GetTest();
        //assert
        Assert.AreEqual(expected, actual);
    }
    #endregion

    #region GetTest
    [TestMethod()]
    public void GetTest_Success()
    {
        //arrange
        var expected = test;

        //act


        inter.AddTest(test);
        var actual = inter.GetTest();


        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetTest_NotExistingFile_Fail()
    {
        //arrange
        var expected = test;

        //act
        inter = new("file1123.xml");
        inter.GetTest();

    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GetTest_FileWithOtherData_Fail()
    {
        //act
        File.Create("file2.xml").Close();
        var sw = File.OpenWrite("file2.xml");
        sw.Write(new byte[] { 1, 2, 3, 4 }, 0, 4);
        sw.Close();

        inter = new("file2.xml");
        inter.GetTest();
    }
    #endregion

    #region AddQuestion
    [TestMethod()]
    public void AddQuestion_Success()
    {
        //arrange
        Test expected = new() { Questions = { new Question("How are you?") } };

        //act
        inter.AddQuestion(Interaction.CreateQuestion("How are you?"));
        var actual = inter.GetTest();

        //arrange
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void AddQuestion_Fail() { } //add question never fails
    #endregion

    #region DeleteQuestion
    [TestMethod()]
    public void DeleteQuestion_Success()
    {
        //arrange
        Test expected = new();
        expected.AddRange(test.Questions);
        expected.RemoveAt(1);

        //act
        inter.AddTest(test);
        inter.DeleteQuestion(1);
        var actual = inter.GetTest();

        //assert
        Assert.AreEqual(expected, actual);
    }

    [ExpectedException(typeof(QuestionException))]
    [TestMethod()]
    public void DeleteQuestion_NegativeIndex_Fail()
    {
        //arrange
        int index = -1;

        //act
        inter.AddTest(test);
        inter.DeleteQuestion(index);
    }

    [ExpectedException(typeof(QuestionException))]
    [TestMethod()]
    public void DeleteQuestion_GreaterIndex_Fail()
    {
        //arrange
        int index = 1000;

        //act
        inter.AddTest(test);
        inter.DeleteQuestion(index);
    }
    #endregion

    #region ChangeQuestion
    [TestMethod()]
    public void ChangeQuestion_Success()
    {
        //arrange
        Test expected = new();
        expected.AddRange(test.Questions);
        expected[0] = question1;
        expected[0].Answers = question.Answers;

        //act
        inter.AddTest(test);
        inter.ChangeQuestion(0, question1);
        var actual = inter.GetTest();

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    [DataRow(-1)]
    [DataRow(1000)]
    [DataRow(3)]
    public void ChangeQuestion_WrongIndex_Fail(int index)
    {
        //act      
        inter.AddTest(test);
        inter.ChangeQuestion(index, question1);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    public void ChangeQuestion_WrongQuestion_Fail()
    {
        //act   
        inter.AddTest(test);
        inter.ChangeQuestion(1, "Hi my name");
    }
    #endregion

    #region CreateQuestion
    [TestMethod()]
    public void CreateQuestion_Success()
    {
        //arrange
        Question expected = new Question("How are you?");

        //act
        var actual = Interaction.CreateQuestion("How are you?");

        //assert
        Assert.AreEqual(expected, actual, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    [DataRow("How are you!")]
    [DataRow("How are you.")]
    [DataRow("How are you")]
    [DataRow("1ow are you!")]
    [DataRow("how are you!")]
    public void CreateQuestion_Fail(string question)
    {
        //act
        Interaction.CreateQuestion(question);
    }
    #endregion

    #region AddAnswer
    [TestMethod()]
    public void AddAnswer_Success()
    {
        //arrange 
        Question expectedQ = new("How are you?");
        expectedQ.Answers.Add("D");
        Test expected = new() { Questions = { expectedQ } };
        //act
        var actualQ = Interaction.CreateQuestion(question);
        inter.AddQuestion(actualQ);
        inter.AddAnswer(0, "D");
        var actual = inter.GetTest();
        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(AnswerException))]
    public void AddAnswer_TryingAdd5thAnswer_Fail()
    {
        //arrange 
        Question expectedQ = new("How are you?") { Answers = answers };
        expectedQ.Answers.Add("D");
        Test expected = new() { Questions = { expectedQ } };

        //act
        inter.AddQuestion(question);
        inter.AddAnswer(0, "D");
        inter.AddAnswer(0, "E");
        var actual = inter.GetTest();
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    public void AddAnswer_WrongQuestionIndex_Fail()
    {
        //act
        inter.AddQuestion(question);
        inter.AddAnswer(3, "D");
    }
    #endregion

    #region DeleteAnswer
    [TestMethod()]
    public void DeleteAnswer_Success()
    {
        //arrange 
        Question expectedQ = new("How are you?") { Answers = answers };
        expectedQ.Answers.RemoveAt(0);
        Test expected = new() { Questions = { expectedQ } };

        //act    

        var actualQ = Interaction.CreateQuestion(question);
        inter.AddQuestion(actualQ);
        inter.AddAnswer(0, "A");
        inter.AddAnswer(0, "B");
        inter.AddAnswer(0, "C");
        inter.DeleteAnswer(0, 0);
        var actual = inter.GetTest();

        //assert
        Assert.AreEqual(expected, actual);
    }

    [ExpectedException(typeof(QuestionException))]
    [TestMethod()]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(3)]
    public void DeleteAnswer_NotValidQuestionIndex_Fail(int questionIndex)
    {
        //arrange
        int answerIndex = 0;

        //act 
        inter.AddTest(test);
        inter.DeleteAnswer(questionIndex, answerIndex);
    }

    [ExpectedException(typeof(AnswerException))]
    [TestMethod()]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(4)]
    public void DeleteAnswer_NotValidAnswerIndex_Fail(int answerIndex)
    {
        //arrange
        int questionIndex = 0;

        //act 
        inter.AddTest(test);
        inter.DeleteAnswer(questionIndex, answerIndex);
    }
    #endregion

    #region SetRightAswer
    [TestMethod()]
    public void SetRightAnswer_Success()
    {
        //arrange
        Test expectedT = new();
        expectedT.AddRange(test.Questions);
        expectedT[0].RightAnswer = 2;
        var expected = expectedT.Questions[0].RightAnswer;

        //act
        inter.AddTest(test);
        inter.SetRightAnswer(0, 2);
        var actual = inter.GetTest().Questions[0].RightAnswer;

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(AnswerException))]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(4)]
    public void SetRightAnswer_NotValidAnswerIndex_Fail(int answerIndex)
    {
        //arrange
        int questionIndex = 0;
        //act
        inter.AddTest(test);
        inter.SetRightAnswer(questionIndex, answerIndex);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(3)]
    public void SetRightAnswer_NotValidQuestionIndex_Fail(int questionIndex)
    {
        //arrange
        int answerIndex = 0;
        //act
        inter.AddTest(test);
        inter.SetRightAnswer(questionIndex, answerIndex);
    }
    #endregion

    #region ChangeAnswer
    [TestMethod()]
    public void ChangeAnswer_D_Success()
    {
        //arrange 
        Test expected = new() { Questions = { question } };
        expected.Questions[0].Answers[2] = "D";

        //act
        inter.AddQuestion(question);
        inter.ChangeAnswer(0, 2, "D");
        var actual = inter.GetTest();

        //assert
        Assert.AreEqual(expected, actual);
    }

    [ExpectedException(typeof(QuestionException))]
    [TestMethod()]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(3)]
    public void ChangeAnswer_NotValidQuestionIndex_Fail(int questionIndex)
    {
        //arrange
        int answerIndex = 0;
        //act
        inter.AddTest(test);
        inter.ChangeAnswer(questionIndex, answerIndex, "Q");
    }

    [ExpectedException(typeof(AnswerException))]
    [TestMethod()]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(4)]
    public void ChangeAnswer_NotValidAnswerIndex_Fail(int answerIndex)
    {
        //arrange
        int questionIndex = 0;
        //act
        inter.AddTest(test);
        inter.ChangeAnswer(questionIndex, answerIndex, "Q");
    }

    #endregion

    #region Count
    [TestMethod()]
    public void Count_3_Success()
    {
        //arrange
        int expected = test.Count;

        //act
        inter.AddTest(test);
        int actual = inter.Count;

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void Count_EmptyTestNegative1_Success()
    {
        //arrange
        int expected = -1;
        //act
        int actual = inter.Count;
        //assert
        Assert.AreEqual(expected, actual);
    }
    #endregion

    #region FilePath
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    [DataRow("file.jpg")]
    [DataRow("file.txt")]
    [DataRow("file.doc")]
    public void SetFilePath_WrongExtension_Fail(string filePath)
    {
        //act
        inter.FilePath = filePath;
    }

    [TestMethod]
    public void GetFilePath_filexml_Success()
    {
        //arrange 
        string expected = "file.xml";

        //act
        string actual = inter.FilePath;

        //assert
        Assert.AreEqual(expected, actual);
    }
    #endregion

    #region ResetRightAnswer
    [TestMethod()]
    public void ResetRightAnswer_Null_Success()
    {
        //act
        inter.AddTest(test);
        inter.ResetRightAnswer(0);
        var question = inter.GetTest()[0];
        var rightAnswer = question.RightAnswer;

        Assert.IsNull(rightAnswer);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(3)]
    public void ResetRightAnswer_WrongQuestionIndex_Fail(int questionIndex)
    {
        //act
        inter.AddTest(test);
        inter.ResetRightAnswer(questionIndex);
    }
    #endregion

    #region CreateUser
    [TestMethod()]
    public void CreateUser_Success()
    {
        //arrange
        string firstName = "Bohdan", lastName = "Liashenko";
        User expected = new(firstName, lastName);

        //act
        User actual = Interaction.CreateUser(firstName, lastName);

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(UserException))]
    [DataRow("bohdan", "liashenko")]
    [DataRow("1Bohdan", "Li1ashenko")]
    [DataRow("BoHdan", "L@iashenko")]
    [DataRow("Bohdan!", "Liаshenko")]
    public void CreateUser_WrongNameandLastName_Fail(string firstName, string lastName)
    {
        //act
        Interaction.CreateUser(firstName, lastName);
    }

    [TestMethod()]
    [ExpectedException(typeof(UserException))]
    public void CreateUser_WrongName_Fail()
    {
        //arrange
        string firstName = "Bohdan d", lastName = "Liashenko";
        //act
        Interaction.CreateUser(firstName, lastName);
    }

    [TestMethod()]
    [ExpectedException(typeof(UserException))]
    public void CreateUser_WrongLastName_Fail()
    {
        //arrange
        string firstName = "Bohdan", lastName = "Liashenko d";
        //act
        Interaction.CreateUser(firstName, lastName);
    }

    #endregion

    #region GetNotExistingRightAnswers
    [TestMethod()]
    public void GetNotExistingRightAnswers_None_Success()
    {
        //arrange
        int expected = 0;

        //act
        inter.AddTest(test);
        var list = inter.GetNotExistingRightAnswers();
        int actual = list.Count;

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void GetNotExistingRightAnswers_3_Success()
    {
        //arrange
        int expected = 3;

        //act
        inter.AddTest(test);
        for(int i = 0; i < test.Count; i++)
            inter.ResetRightAnswer(i);

        var list = inter.GetNotExistingRightAnswers();
        int actual = list.Count;

        //assert
        Assert.AreEqual(expected, actual);
    }
    #endregion

    #region CheckForRightAnswers
    [TestMethod()]
    public void CheckForRightAnswers_Null_Success()
    {
        //act
        inter.AddTest(test);
        string actual = inter.CheckForRightAnswers();
        //assert
        Assert.IsNull(actual);
    }

    [TestMethod()]
    public void CheckForRightAnswers_NotNull_Success()
    {
        //act
        inter.AddTest(test);
        for(int i = 0; i < test.Count; i++)
            inter.ResetRightAnswer(i);

        string actual = inter.CheckForRightAnswers();
        Debug.WriteLine(actual);

        //assert
        Assert.IsNotNull(actual);
    }
    #endregion

    #region SetUserAnswer
    [TestMethod()]
    public void SetUserAnswer_0_0_Success()
    {
        //arrange
        int answerIndex = 0, questionIndex = 0;
        //act
        inter.AddTest(test);
        inter.SetUserAnswer(questionIndex, answerIndex);
        var actual = inter.GetTest()[questionIndex].UserAnswer;
        //assert
        Assert.AreEqual(answerIndex, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(QuestionException))]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(3)]
    public void SetUserAnswer_WrongQuestionIndex_Fail(int questionIndex)
    {
        //arrange
        int answerIndex = 0;
        //act
        inter.AddTest(test);
        inter.SetUserAnswer(questionIndex, answerIndex);
    }

    [TestMethod()]
    [ExpectedException(typeof(AnswerException))]
    [DataRow(10)]
    [DataRow(-1)]
    [DataRow(4)]
    public void SetUserAnswer_WrongAnswerIndex_Fail(int answerIndex)
    {
        //arrange
        int questionIndex = 0;
        //act
        inter.AddTest(test);
        inter.SetUserAnswer(questionIndex, answerIndex);
        var actual = inter.GetTest()[questionIndex].UserAnswer;
        //assert
        Assert.AreEqual(answerIndex, actual);
    }

    #endregion

    [TestMethod()]
    public void ResetUserAnswers()
    {
        //arrange
        int?[] expected = { null, null, null };

        //act
        test[0].UserAnswer = 2;
        test[1].UserAnswer = 3;
        test[2].UserAnswer = 1;
        inter.AddTest(test);
        inter.ResetUserAnswers();

        int?[] actual = new int?[3];
        int index = 0;
        test = inter.GetTest();
        foreach(var q in test.Questions)
        {
            actual[index++] = q.UserAnswer; 
        }
        //assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void CalculatePersentOfRightAnswers_Success()
    {
        //arrange
        DateTime time = new(2022, 11, 8);
        User user = new("Bohdan", "Liashenko");

        test[0].UserAnswer = test[0].RightAnswer;
        test[1].UserAnswer = test[1].RightAnswer;
        test[2].UserAnswer = test[2].RightAnswer;

        Statistic stats = new()
        {
            { user, new Mark(100, time) }
        };
        string expected = stats[0];

        //act
        inter.AddTest(test);
        inter.CalculatePersentOfRightAnswers(time, user);
        string actual = inter.GetTest().GetLastStatistic();

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void Clear_Success()
    {
        //arrange 
        string expected = "";

        //act
        inter.FilePath = "file.xml";
        inter.AddTest(test);
        inter.ClearFile();

        string actual = File.ReadAllText("file.xml");

        //assert
        Assert.AreEqual(expected, actual);
    }
}
