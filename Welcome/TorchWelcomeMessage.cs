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

        var toggleHint = Plugin.ToggleTorchShortcut != null
            ? Plugin.ToggleTorchShortcut.Value.ToString()
            : "Ctrl+T";

        var welcomeMessageText =
            "Welcome to TorchCEO!\n\n" +
            "This mod adds a cursor flashlight: a point light that follows your mouse on the current floor.\n\n" +
            "The torch is off by default. Press " + toggleHint +
            " to toggle it, or open the mod configuration (F1) and enable it under Flashlight.";

        DialogUtils.QueueDialog(welcomeMessageText);
        DefaultConfig.ShowWelcomeMessage.Value = false;
    }
}
