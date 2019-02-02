﻿using System.Diagnostics;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

public static class ProcessUtility
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }
    public enum ReadType
    {
        Byte = 1,
        Int = 4,
    }

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern int CloseHandle(IntPtr hProcess);
    
    public static byte ReadMemByte(Process p, IntPtr address)
    {
        IntPtr hProc = OpenProcess(ProcessAccessFlags.VMRead, false, p.Id);
        byte[] val = new byte[1];

        int bytesRead = 0;
        ReadProcessMemory(hProc, address, val, val.Length, ref bytesRead);

        CloseHandle(hProc);

        return val[0];
    }

    public static int ReadMemInt(Process p, IntPtr address)
    {
        IntPtr hProc = OpenProcess(ProcessAccessFlags.VMRead, false, p.Id);
        byte[] val = new byte[4];

        int bytesRead = 0;
        ReadProcessMemory(hProc, address, val, val.Length, ref bytesRead);

        CloseHandle(hProc);

        return BitConverter.ToInt32(val, 0);
    }

    public static void WriteMem(Process p, IntPtr address, int v)
    {
        IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, p.Id);
        byte[] val = BitConverter.GetBytes(v);

        DoWrite(hProc, address, val);
    }

    public static void WriteMem(Process p, IntPtr address, byte v)
    {
        IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, p.Id);
        byte[] val = BitConverter.GetBytes(v);

        DoWrite(hProc, address, val);
    }

    static void DoWrite(IntPtr hProc, IntPtr address, byte[] val)
    {
        int bytesRead = 0;
        WriteProcessMemory(hProc, address, val, val.Length, out bytesRead);

        CloseHandle(hProc);
    }
    
    internal static Process GetProcess(string name)
    {
        var process = Process.GetProcessesByName(name).FirstOrDefault();
        if (process != null && process.HasExited)
            return null;

        return process;
    }
}