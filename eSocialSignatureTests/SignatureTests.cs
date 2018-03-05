using System;
using System.IO;
using System.Xml;
using eSocialSignature;
using eSocialSignature.Hashes;
using eSocialSignature.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eSocialSignatureTests
{
    [TestClass]
    public class SignatureTests
    {
        private readonly string SerialNumber = Environment.GetEnvironmentVariable("CERT_TOKEN_A3_SERIAL_NUMBER", EnvironmentVariableTarget.User);
        private readonly string TokenPin = Environment.GetEnvironmentVariable("CERT_TOKEN_A3_PIN", EnvironmentVariableTarget.User);
        private readonly string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "envio-sem-assinatura.xml");

        private readonly XmlDocument Xml;

        public SignatureTests()
        {
            Xml = new XmlDocument();
            Xml.Load(FileName);
        }

        [TestMethod]
        public void ShouldSignXml()
        {
            var xml = Xml.OuterXml;

            var log = new Logger("logTest.txt");
            xml = new HashSHA256(log).Sign(xml, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.LoadXml(xml);
            signedXml.Save("envio-assinado.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }

        [TestMethod]
        public void ShouldSignFile()
        {
            var xml = Xml.OuterXml;

            var log = new Logger("logTest2.txt");
            xml = new HashSHA256(log).Sign(xml, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.LoadXml(xml);
            signedXml.Save("envio-assinado-arquivo.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }

        [TestMethod]
        public void ShouldSignXmlWithExportsMethodsAnsi()
        {
            var xml = Xml.OuterXml;

            Exports.SignSHA256Ansi(ref xml, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.LoadXml(xml);
            signedXml.Save("envio-assinado.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }

        [TestMethod]
        public void ShouldSignXmlWithExportsMethodsUnicode()
        {
            var xml = Xml.OuterXml;

            Exports.SignSHA256Unicode(ref xml, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.LoadXml(xml);
            signedXml.Save("envio-assinado.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }

        [TestMethod]
        public void ShouldSignFileWithExportMethodsAnsi()
        {
            Exports.SignFileWithSHA256Ansi(FileName, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.Load(FileName);
            signedXml.Save("envio-assinado-exports-ansi.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }

        [TestMethod]
        public void ShouldSignFileWithExportMethodsUnicode()
        {
            Exports.SignFileWithSHA256Unicode(FileName, "evtInfoEmpregador", SerialNumber, TokenPin);

            var signedXml = new XmlDocument();
            signedXml.Load(FileName);
            signedXml.Save("envio-assinado-exports-unicode.xml");
            Assert.IsTrue(signedXml.GetElementsByTagName("Signature").Count > 0);
        }
    }
}
