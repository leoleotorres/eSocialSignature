using System.Xml;
using eSocialSignature.Hashes;
using eSocialSignature.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eSocialSignatureTests
{
    [TestClass]
    public class SignatureTests
    {
        public const string SerialNumber = @"41dacd08daa347b1a000d83eb510f4a3";

        [TestMethod]
        public void ShouldSignFile()
        {
            var doc = new XmlDocument();
            doc.Load("envio-sem-assinatura.xml");

            var xml = doc.OuterXml;

            var log = new Logger("logTest.txt");
            new HashSHA256(log).Sign(ref xml, "evtInfoEmpregador", SerialNumber, "1234");
            
            var signedXml = new XmlDocument();
            signedXml.LoadXml(xml);
            signedXml.Save("envio-assinado.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }
    }
}
