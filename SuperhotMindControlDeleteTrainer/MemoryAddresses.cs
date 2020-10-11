using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable

namespace SuperhotMindControlDeleteTrainer {

    public interface MemoryAddress {

        public IntPtr address { get; }

    }

    public readonly struct FixedMemoryAddress: MemoryAddress {

        public IntPtr address { get; }

        public FixedMemoryAddress(IntPtr address) {
            this.address = address;
        }

    }

    public readonly struct IndirectMemoryAddress: MemoryAddress {

        private readonly ProcessHandle    processHandle;
        private readonly string?          moduleName;
        private readonly IEnumerable<int> offsets;

        public IndirectMemoryAddress(ProcessHandle processHandle, string moduleName, IEnumerable<int> offsets) {
            this.processHandle = processHandle;
            this.moduleName    = moduleName;
            this.offsets       = offsets;
        }

        public IntPtr address {
            get {
                IntPtr moduleBaseAddress = MemoryEditorImpl.getModuleBaseAddressByName(processHandle, moduleName) ??
                    throw new ArgumentException($"No module with name {moduleName} found in process {processHandle.process.ProcessName}");
                int    firstOffset         = offsets.First();
                IntPtr memoryAddress = IntPtr.Add(moduleBaseAddress, firstOffset);

                int targetProcessWordLengthBytes = Win32.isProcess64Bit(processHandle.process) ? Marshal.SizeOf<long>() : Marshal.SizeOf<int>();

                foreach (int offset in offsets.Skip(1)) {
                    bool success = Win32.ReadProcessMemory(processHandle.handle, memoryAddress, out IntPtr memoryValue, targetProcessWordLengthBytes, out long _);
                    if (!success) {
                        throw new ApplicationException($"Could not read memory address 0x{memoryAddress.ToInt64():X}: {Marshal.GetLastWin32Error()}");
                    }

                    memoryAddress = memoryValue;

                    memoryAddress = IntPtr.Add(memoryAddress, offset);
                }

                return memoryAddress;
            }
        }

    }

}