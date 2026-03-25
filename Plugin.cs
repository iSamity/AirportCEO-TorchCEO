using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ShortcutCeo.config;
using TorchCEO.Config;
using TorchCEO.Flashlight;
using UnityEngine;

namespace TorchCEO;

[BepInPlugin($"org.iSamity.{MyPluginInfo.PLUGIN_GUID}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("org.airportceomodloader.humoresque")]
[BepInDependency("org.iSamity.plugins.ShortcutCeo")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static ConfigFile ConfigReference { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ToggleTorchShortcut;

    private void Awake()
    {
        // Plugin startup logic
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
        ToggleTorchShortcut = ConfigReference.Bind(
            "Flashlight",
            "Toggle cursor flashlight",
            new KeyboardShortcut(KeyCode.T, KeyCode.LeftControl),
            "Toggles the cursor torch.");

        ConfigManager.AddShortcut(ToggleTorchShortcut, () =>
        {
            DefaultConfig.CursorFlashlightEnabled.Value = !DefaultConfig.CursorFlashlightEnabled.Value;
        });
    }
}