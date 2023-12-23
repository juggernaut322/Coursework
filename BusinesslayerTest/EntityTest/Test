using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer.Entity.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Entity.Test.Tests;

[TestClass()]
public class TestTests
{

    Test expected;
    Test actual;

    [TestInitialize]
    public void Init()
    {
        expected = Interaction.DefTest;
        actual = Interaction.DefTest;
    }

    [TestMethod()]
    public void Equals_Success()
    {
        //act

        var equal = expected.Equals(actual);

        //assert
        Assert.IsTrue(equal);
    }

    [TestMethod()]
    public void Equals_Fail()
    {
        //act
        expected.Add("How are you?");
        var equal = expected.Equals(actual);

        //assert
        Assert.IsFalse(equal);
    }

    [TestMethod()]
    public void HashCodeEquals_Success()
    {
        //act
        int expectedHash = expected.GetHashCode();
        int actualHash = actual.GetHashCode();

        //assert
        Assert.AreEqual(expectedHash, actualHash);
    }
    [TestMethod()]
    public void HashCodeEquals_Fail()
    {
        //act
        expected.Add("How are you?");
        int expectedHash = expected.GetHashCode();
        int actualHash = actual.GetHashCode();

        //assert
        Assert.AreNotEqual(expectedHash, actualHash);
    }

}
