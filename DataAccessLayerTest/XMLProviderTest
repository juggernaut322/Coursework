using BusinessLogicLayer;
using BusinessLogicLayer.Entity.Stats;
using BusinessLogicLayer.Entity.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests;

[TestClass()]
public class XMLProviderTests
{
    //[TestMethod()]
    //public void XMLProviderTest()
    //{
    //    Assert.Fail();
    //}

    //[TestMethod()]
    //public void XMLProviderTest1()
    //{
    //    Assert.Fail();
    //}

    [TestMethod()]
    public void SerializeTest()
    {

    }

    [TestMethod()]
    public void DeserializeTest()
    {
        //arrange
        Test expected = Interaction.DefTest;

        //act
        XMLProvider xp = new(typeof(Test), new Type[] {typeof(Statistic), typeof(Question), typeof(Question)});
        xp.Serialize(expected, "file.xml");
        var actual = xp.Deserialize("file.xml") as Test;

        //assert
        Assert.AreEqual(expected, actual);
    }
}
