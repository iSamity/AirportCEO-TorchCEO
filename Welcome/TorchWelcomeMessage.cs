using AirportCEOModLoader.Core;
using HarmonyLib;
using TorchCEO.Config;

namespace TorchCEO.Welcome;

[HarmonyPatch(typeof(UpdatePanelUI), nameof(UpdatePanelUI.DisplayOnlyUpdateButtons))]
internal static class TorchWelcomeMessage
{
    [HarmonyPostfix]
    static void ShowWelcomeIfEnabled()
    {
        if (!DefaultConfig.ShowWelcomeMessage.Value)
            return;

        var welcomeMessageText =
            "Welcome to TorchCEO!\n\n" +
            "This mod adds a cursor flashlight: a point light that follows your mouse on the current floor.\n\n" +
            "The torch is off by default. Open the mod configuration (F1) and enable it under Flashlight." +
            (Plugin.ToggleTorchShortcut != null
                ? "\n\nYou can also press " + Plugin.ToggleTorchShortcut.Value + " to toggle it (ShortcutCeo)."
                : "\n\nInstall the ShortcutCeo mod if you want an optional in-game keyboard shortcut to toggle the torch.");

        DialogUtils.QueueDialog(welcomeMessageText);
        DefaultConfig.ShowWelcomeMessage.Value = false;
    }
}
