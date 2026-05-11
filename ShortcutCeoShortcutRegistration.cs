using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using ShortcutCeo.config;

namespace TorchCEO;

/// <summary>
/// Contains direct references to ShortcutCeo. Keep this type separate from <see cref="Plugin"/> so the runtime
/// does not load ShortcutCeo.dll until <see cref="RegisterToggle"/> runs (only after Chainloader reports ShortcutCeo loaded).
/// </summary>
internal static class ShortcutCeoShortcutRegistration
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void RegisterToggle(ConfigEntry<KeyboardShortcut> shortcut, Action toggleTorch)
    {
        ConfigManager.AddShortcut(shortcut, toggleTorch);
        Plugin.Logger.LogInfo("TorchCEO: ShortcutCeo — toggle bound.");
    }
}
