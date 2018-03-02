using System;
using System.Runtime.InteropServices;
using System.Xml;
using eSocialSignature.Hashes;
using eSocialSignature.Log;
using RGiesecke.DllExport;

namespace eSocialSignature
{
    public static class Exports
    {
        [DllExport("SignSHA256Ansi", CallingConvention = CallingConvention.StdCall)]
        public static void SignSHA256Ansi(
            [MarshalAs(UnmanagedType.LPStr)] ref string xml,
            [MarshalAs(UnmanagedType.LPStr)] string nodeToSign,
            [MarshalAs(UnmanagedType.LPStr)] string certificateSerialNumber,
            [MarshalAs(UnmanagedType.LPStr)] string certificatePassword)
        {
            var log = new Logger($"log{DateTime.Now:yyyyMMdd}.txt");
            xml = new HashSHA256(log).Sign(xml, nodeToSign, certificateSerialNumber, certificatePassword) + null;
        }

        [DllExport("SignSHA256Unicode", CallingConvention = CallingConvention.StdCall)]
        public static void SignSHA256Unicode(
            [MarshalAs(UnmanagedType.LPWStr)] ref string xml,
            [MarshalAs(UnmanagedType.LPWStr)] string nodeToSign,
            [MarshalAs(UnmanagedType.LPWStr)] string certificateSerialNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string certificatePassword)
        {
            var log = new Logger($"log{DateTime.Now:yyyyMMdd}.txt");
            xml = new HashSHA256(log).Sign(xml, nodeToSign, certificateSerialNumber, certificatePassword) + null;
        }

        [DllExport("SignFileWithSHA256Ansi", CallingConvention = CallingConvention.StdCall)]
        public static void SignFileWithSHA256Ansi(
            [MarshalAs(UnmanagedType.LPStr)] string fileName,
            [MarshalAs(UnmanagedType.LPStr)] string nodeToSign,
            [MarshalAs(UnmanagedType.LPStr)] string certificateSerialNumber,
            [MarshalAs(UnmanagedType.LPStr)] string certificatePassword)
        {
            var log = new Logger($"log{DateTime.Now:yyyyMMdd}.txt");
            var doc = new XmlDocument();
            doc.Load(fileName);
            var xml = new HashSHA256(log).Sign(doc.OuterXml, nodeToSign, certificateSerialNumber, certificatePassword);
            doc.InnerXml = xml;
            doc.Save(fileName);
        }

        [DllExport("SignFileWithSHA256Unicode", CallingConvention = CallingConvention.StdCall)]
        public static void SignFileWithSHA256Unicode(
            [MarshalAs(UnmanagedType.LPWStr)] string fileName,
            [MarshalAs(UnmanagedType.LPWStr)] string nodeToSign,
            [MarshalAs(UnmanagedType.LPWStr)] string certificateSerialNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string certificatePassword)
        {
            var log = new Logger($"log{DateTime.Now:yyyyMMdd}.txt");
            var doc = new XmlDocument();
            doc.Load(fileName);
            var xml = new HashSHA256(log).Sign(doc.OuterXml, nodeToSign, certificateSerialNumber, certificatePassword);
            doc.InnerXml = xml;
            doc.Save(fileName);
        }
    }
}
