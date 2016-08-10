using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touha.Infrastructure.Utils.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touha.Infrastructure.UtilsTests;

namespace Touha.Infrastructure.Utils.Xml.Tests
{
    [TestClass()]
    public class XmlUtilTests
    {
        [TestMethod()]
        public void DeserializeViaXmlSerializerTest()
        {
            string xml = @"<XmlProduct>
    <ProductID>8a7d0644-c8b7-4bce-b248-ebc5241bafed</ProductID>
    <ProductName>aa</ProductName>
    <ProducingArea>bb</ProducingArea>
    <UnitPrice>22</UnitPrice>
</XmlProduct>";
            Assert.AreEqual(XmlUtil.DeserializeViaXmlSerializer<XmlProduct>(xml).ProductName, "aa");
        }

        [TestMethod()]
        public void SerializeViaXmlSerializerTest()
        {
            XmlProduct product = new XmlProduct { ProductID = Guid.NewGuid(), ProductName = "aa", ProducingArea = "bb", UnitPrice = 22 };
            string xml = XmlUtil.SerializeViaXmlSerializer<XmlProduct>(product);
            Assert.IsTrue(xml.Contains("<ProductName>aa</ProductName>"));
        }

        [TestMethod()]
        public void SerializerViaDataContractSerializerTest()
        {
            DataContractProduct product = new DataContractProduct { ProductID = Guid.NewGuid(), ProductName = "aa", ProducingArea = "bb", UnitPrice = 22 };
            string xml = XmlUtil.SerializerViaDataContractSerializer<DataContractProduct>(product);
            Assert.IsTrue(xml.Contains("<name>aa</name>"));
        }
    }
}