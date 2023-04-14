﻿#nullable enable

using System;
using System.Windows.Forms;
using DakarDesertRallyTrainer.Games;
using Gma.System.MouseKeyHook;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace DakarDesertRallyTrainer.Cheats;

public class NoSpeedLimitCheat: BaseCheat {

    private static readonly int[] SPEED_LIMIT_OFFSETS_1_9_0 = { 0x5589060, 0x10, 0xF0, 0x30C };
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_7_0 = { 0x55BECA0, 0x10, 0xF0, 0x30C };
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_6_0 = { 0x555A7B0, 0x10, 0xF0, 0x30C };
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_5_0 = { 0x57C3550, 0x10, 0xF0, 0x30C };

    private static readonly int[] BANNER_OFFSETS_1_9_0 = { 0x5B200B8, 0x3B0, 0x10, 0x7F0, 0x658, 0x364 };
    private static readonly int[] BANNER_OFFSETS_1_7_0 = { 0x5B5DC38, 0x3B0, 0x10, 0x7F0, 0x658, 0x364 };
    private static readonly int[] BANNER_OFFSETS_1_6_0 = { 0x5AF6638, 0x3B0, 0x10, 0x7F0, 0x658, 0x364 };
    private static readonly int[] BANNER_OFFSETS_1_5_0 = { 0x5C62C80, 0x30, 0x2B0, 0x408, 0x658, 0x364 };

    public override string name { get; } = "No speed limit";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        (int[] speedLimitOffsets, int[] bannerOffsets) = gameVersionCode switch {
            DakarDesertRally.Versions.V1_9_0 => (SPEED_LIMIT_OFFSETS_1_9_0, BANNER_OFFSETS_1_9_0),
            DakarDesertRally.Versions.V1_7_0 => (SPEED_LIMIT_OFFSETS_1_7_0, BANNER_OFFSETS_1_7_0),
            DakarDesertRally.Versions.V1_6_0 => (SPEED_LIMIT_OFFSETS_1_6_0, BANNER_OFFSETS_1_6_0),
            DakarDesertRally.Versions.V1_5_0 => (SPEED_LIMIT_OFFSETS_1_5_0, BANNER_OFFSETS_1_5_0),
            _                                => (Array.Empty<int>(), Array.Empty<int>())
        };

        IndirectMemoryAddress speedLimitAddress = new(processHandle, null, speedLimitOffsets);
        IndirectMemoryAddress bannerAddress     = new(processHandle, null, bannerOffsets);

        MemoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);
        MemoryEditor.writeToProcessMemory(processHandle, bannerAddress, 9999);
    }

}