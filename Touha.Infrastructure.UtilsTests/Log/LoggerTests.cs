using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touha.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Touha.Infrastructure.Utils.Log.Tests
{
    [TestClass()]
    public class LoggerTests
    {
        [TestMethod()]
        public void WriteTest()
        {
            //Logger log = Logger.GetInstance();
            Logger.Write("aa");
            Thread.Sleep(3000);
            Assert.IsTrue(File.ReadAllText("log.txt").Contains("aa"));
        }
    }
}