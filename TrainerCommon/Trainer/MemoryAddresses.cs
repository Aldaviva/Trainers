#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TrainerCommon.Trainer;

public interface MemoryAddress {

    /// <exception cref="ApplicationException" accessor="get">if the memory could not be read</exception>
    public IntPtr address { get; }

}

public readonly struct FixedMemoryAddress(IntPtr address): MemoryAddress {

    public IntPtr address { get; } = address;

}

/// <param name="processHandle"></param>
/// <param name="moduleName">The name of the module to which the offsets are relative, such as <c>UnityPlayer.dll</c>, or <c>null</c> to use the process' main module.</param>
/// <param name="pointerOffsets"></param>
public readonly struct IndirectMemoryAddress(ProcessHandle processHandle, string? moduleName, IReadOnlyList<int> pointerOffsets): MemoryAddress {

    /// <inheritdoc />
    public IntPtr address {
        get {
            /*
             * Is the game 32-bit or 64-bit?
             * Note that the trainer must be compiled as 64-bit in order to read memory from 64-bit games.
             */
            int targetProcessWordLengthBytes = processHandle.is64Bit ? 8 : 4;

            IntPtr memoryAddress = MemoryEditor.getModuleBaseAddressByName(processHandle, moduleName);

            for (int offsetIndex = 0; offsetIndex < pointerOffsets.Count; offsetIndex++) {
                int offset = pointerOffsets[offsetIndex];

                if (offsetIndex == 0) {
                    memoryAddress = IntPtr.Add(memoryAddress, offset);
                } else {
                    bool success = Win32.ReadProcessMemory(processHandle.handle, memoryAddress, out IntPtr memoryValue, targetProcessWordLengthBytes, out long _);
                    if (!success) {
                        throw new ApplicationException($"Could not read memory address 0x{memoryAddress.ToInt64():X}: {Marshal.GetLastWin32Error()}");
                    }

                    memoryAddress = IntPtr.Add(memoryValue, offset);
                }
            }

            return memoryAddress;
        }
    }

}

public readonly struct OffsetMemoryAddress(MemoryAddress baseAddress, int offset): MemoryAddress {

    public IntPtr address => IntPtr.Add(baseAddress.address, offset);

}