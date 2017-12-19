using System;
using System.Runtime.InteropServices;
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
            new HashSHA256(log).Sign(ref xml, nodeToSign, certificateSerialNumber, certificatePassword);
        }

        [DllExport("SignSHA256Unicode", CallingConvention = CallingConvention.StdCall)]
        public static void SignSHA256Unicode(
            [MarshalAs(UnmanagedType.LPWStr)] ref string xml,
            [MarshalAs(UnmanagedType.LPWStr)] string nodeToSign,
            [MarshalAs(UnmanagedType.LPWStr)] string certificateSerialNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string certificatePassword)
        {
            var log = new Logger($"log{DateTime.Now:yyyyMMdd}.txt");
            new HashSHA256(log).Sign(ref xml, nodeToSign, certificateSerialNumber, certificatePassword);
        }
    }
}
