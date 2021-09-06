using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

#nullable enable

namespace TrainerCommon.Trainer {

    public static class Win32 {

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [Flags]
        internal enum ProcessAccessFlags: uint {

            ALL                       = 0x001F0FFF,
            TERMINATE                 = 0x00000001,
            CREATE_THREAD             = 0x00000002,
            VIRTUAL_MEMORY_OPERATION  = 0x00000008,
            VIRTUAL_MEMORY_READ       = 0x00000010,
            VIRTUAL_MEMORY_WRITE      = 0x00000020,
            DUPLICATE_HANDLE          = 0x00000040,
            CREATE_PROCESS            = 0x00000080,
            SET_QUOTA                 = 0x00000100,
            SET_INFORMATION           = 0x00000200,
            QUERY_INFORMATION         = 0x00000400,
            QUERY_LIMITED_INFORMATION = 0x00001000,
            SYNCHRONIZE               = 0x00100000

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out long lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out IntPtr lpBuffer, int dwSize, out long lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hProcess);

        private const long INVALID_HANDLE_VALUE = -1;

        [Flags]
        private enum SnapshotFlags: uint {

            // HEAP_LIST = 0x00000001,
            // PROCESS   = 0x00000002,
            // THREAD    = 0x00000004,
            MODULE   = 0x00000008,
            MODULE32 = 0x00000010,
            // INHERIT   = 0x80000000,
            // ALL       = 0x0000001F,
            // NO_HEAPS  = 0x40000000

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct ModuleEntry32 {

            internal          uint   structSizeBytes;
            private readonly  uint   unused1;
            private readonly  uint   pid;
            private readonly  uint   unused2;
            private readonly  uint   unused3;
            internal readonly IntPtr baseAddress;
            private readonly  uint   moduleSizeBytes;
            private readonly  IntPtr moduleHandle;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal readonly string name;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            private readonly string path;

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Module32First(IntPtr hSnapshot, ref ModuleEntry32 lpme);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Module32Next(IntPtr hSnapshot, ref ModuleEntry32 lpme);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, IntPtr th32ProcessId);

        internal static IntPtr getModuleBaseAddress(IntPtr procId, string modName) {
            IntPtr baseAddress = IntPtr.Zero;
            IntPtr hSnap       = CreateToolhelp32Snapshot(SnapshotFlags.MODULE | SnapshotFlags.MODULE32, procId);

            if (hSnap.ToInt64() != INVALID_HANDLE_VALUE) {
                ModuleEntry32 modEntry = new() { structSizeBytes = (uint) Marshal.SizeOf(typeof(ModuleEntry32)) };

                if (Module32First(hSnap, ref modEntry)) {
                    do {
                        if (modEntry.name.Equals(modName)) {
                            baseAddress = modEntry.baseAddress;
                            break;
                        }
                    } while (Module32Next(hSnap, ref modEntry));
                }
            }

            CloseHandle(hSnap);

            return baseAddress;
        }

        // see https://msdn.microsoft.com/en-us/library/windows/desktop/ms684139%28v=vs.85%29.aspx
        internal static bool isProcess64Bit(Process process) {
            if (!Environment.Is64BitOperatingSystem) {
                return false;
            }

            if (!IsWow64Process(process.Handle, out bool isWow64)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return !isWow64;
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE      = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;

        public static void hideMinimizeAndMaximizeButtons(Window window) {
            IntPtr windowHandle        = new WindowInteropHelper(window).Handle;
            int    existingWindowStyle = GetWindowLong(windowHandle, GWL_STYLE);
            SetWindowLong(windowHandle, GWL_STYLE, existingWindowStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
        }

    }

}