using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TorchCEO.Config;
using TorchCEO.Flashlight;
using UnityEngine;

namespace TorchCEO;

[BepInPlugin($"org.iSamity.{MyPluginInfo.PLUGIN_GUID}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
[BepInDependency(ShortcutCeoIntegration.ShortcutCeoPluginGuid, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static ConfigFile ConfigReference { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ToggleTorchShortcut;

    private void Awake()
    {
        Logger = base.Logger;
        ConfigReference = base.Config;

        DefaultConfig.Setup();

        gameObject.AddComponent<CursorFlashlightController>();

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Start()
    {
        if (!ShortcutCeoIntegration.IsShortcutCeoLoaded())
        {
            ToggleTorchShortcut = null;
            Logger.LogInfo(
                "TorchCEO: ShortcutCeo not installed — no keyboard toggle; enable the torch under Flashlight in config (F1 / TorchCEO.cfg). Install ShortcutCeo if you want an in-game shortcut.");
            return;
        }

        try
        {
            ToggleTorchShortcut = ConfigReference.Bind(
                "Flashlight",
                "Toggle cursor flashlight",
                new KeyboardShortcut(KeyCode.T, KeyCode.LeftControl),
                "Press this combo to turn the cursor torch on or off (registered with ShortcutCeo).");

            ShortcutCeoShortcutRegistration.RegisterToggle(ToggleTorchShortcut, ToggleTorchEnabled);
        }
        catch (Exception ex)
        {
            ToggleTorchShortcut = null;
            Logger.LogWarning($"TorchCEO: ShortcutCeo is loaded but toggle registration failed ({ex.Message}).");
        }
    }

    private static void ToggleTorchEnabled()
    {
        DefaultConfig.CursorFlashlightEnabled.Value = !DefaultConfig.CursorFlashlightEnabled.Value;
    }
}
