#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TrainerCommon.Trainer;

public static class MemoryEditor {

    private static readonly Encoding DEFAULT_ENCODING = Encoding.Unicode;

    public static ProcessHandle? openProcess(Process targetProcess) {
        IntPtr handle = Win32.OpenProcess(
            Win32.ProcessAccessFlags.VIRTUAL_MEMORY_READ |
            Win32.ProcessAccessFlags.VIRTUAL_MEMORY_WRITE |
            Win32.ProcessAccessFlags.VIRTUAL_MEMORY_OPERATION,
            false, targetProcess.Id);
        return handle != IntPtr.Zero ? new ProcessHandle(targetProcess, handle, Win32.isProcess64Bit(targetProcess)) : null;
    }

    public static T readFromProcessMemory<T>(ProcessHandle processHandle, MemoryAddress memoryAddress, int? bytesToRead = null) {
        bytesToRead ??= Marshal.SizeOf<T>();

        byte[] buffer = new byte[bytesToRead.Value];

        bool readSuccess = Win32.ReadProcessMemory(processHandle.handle, memoryAddress.address, buffer, buffer.Length, out long bytesRead);

        if (!readSuccess || bytesRead == 0 || bytesRead > buffer.Length) {
            throw new ApplicationException($"read wrong number of bytes (expected {buffer.Length:N0}, actual {bytesRead:N0})");
        }

        return convertBufferToType<T>(buffer);
    }

    public static void writeToProcessMemory<T>(ProcessHandle processHandle, MemoryAddress memoryAddress, T value) {
        byte[] buffer = convertValueToBuffer(value);

        bool writeSuccess = Win32.WriteProcessMemory(processHandle.handle, memoryAddress.address, buffer, buffer.Length, out IntPtr bytesWritten);

        if (!writeSuccess || bytesWritten.ToInt32() != buffer.Length) {
            throw new ApplicationException($"wrote wrong number of bytes (expected {buffer.Length:N0}, actual {bytesWritten.ToInt32():N0})");
        }
    }

    public static void writeToProcessMemory(ProcessHandle processHandle, MemoryAddress memoryAddress, string value, Encoding? characterEncoding = default) {
        characterEncoding ??= DEFAULT_ENCODING;
        writeToProcessMemory(processHandle, memoryAddress, characterEncoding.GetBytes(value));
    }

    private static byte[] convertValueToBuffer<T>(T value) => value switch {
        byte[] bytes => bytes,
        int i        => BitConverter.GetBytes(i),
        // strings are handled separately by writeToProcessMemory(ProcessHandle, MemoryAddress, string, Encoding?)
        _ => throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(convertValueToBuffer)} does not support input type {typeof(T)}")
    };

    public static IntPtr getModuleBaseAddressByName(ProcessHandle processHandle, string? moduleName) {
        string cacheKey = moduleName ?? string.Empty;

        if (!processHandle.moduleBaseAddressCache.TryGetValue(cacheKey, out IntPtr result)) {
            result = moduleName == null
                ? processHandle.process.MainModule!.BaseAddress
                : Win32.getModuleBaseAddress(new IntPtr(processHandle.process.Id), moduleName);

            processHandle.moduleBaseAddressCache[cacheKey] = result;
        }

        return result;
    }

    private static T convertBufferToType<T>(byte[] buffer) {
        return (T) (object) (Type.GetTypeCode(typeof(T)) switch {
            TypeCode.String => Encoding.Unicode.GetString(buffer, 0, buffer.Length).Split(new[] { (char) 0 }, 2)[0],
            TypeCode.Int32  => BitConverter.ToInt32(BitConverter.IsLittleEndian ? buffer : buffer.Reverse().ToArray(), 0),
            _               => throw new ArgumentOutOfRangeException()
        });
    }

}

public class ProcessHandle: IDisposable {

    public Process process { get; }
    public IntPtr handle { get; }
    public bool is64Bit { get; }
    public IDictionary<string, IntPtr> moduleBaseAddressCache { get; } = new Dictionary<string, IntPtr>();

    internal ProcessHandle(Process process, IntPtr handle, bool is64Bit) {
        this.process = process;
        this.handle  = handle;
        this.is64Bit = is64Bit;
    }

    private void dispose(bool disposing) {
        _ = Win32.CloseHandle(handle);
        if (disposing) {
            process.Dispose();
        }
    }

    public void Dispose() {
        dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ProcessHandle() {
        dispose(false);
    }

}