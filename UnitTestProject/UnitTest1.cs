using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WellNet.Utils;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ConnectionManager.CreateConnectionEntries();
        }
    }
}
