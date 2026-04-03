using BepInEx.Configuration;

namespace TorchCEO.Config;

static class DefaultConfig
{
    /// <summary>Nominal range for flashlight intensity; range compensation uses this as the reference.</summary>
    internal const float CursorFlashlightRangeDefault = 5000f;

    internal static ConfigEntry<bool> ShowWelcomeMessage { get; private set; }

    internal static ConfigEntry<bool> CursorFlashlightEnabled { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightIntensityBelowGround { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightIntensityAboveGroundDay { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightIntensityAboveGroundNight { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightRange { get; private set; }

    public static void Setup()
    {
        ShowWelcomeMessage = ConfigReference.Bind("General", "Show TorchCEO welcome message", true, "Show a one-time welcome when the game starts explaining the mod.");

        CursorFlashlightEnabled = ConfigReference.Bind("Flashlight", "Enable cursor flashlight", false, "In-game only: spotlight that follows the mouse on the current floor");
        CursorFlashlightIntensityBelowGround = ConfigReference.Bind("Flashlight", "Intensity below ground", 5f, "Floors with Z below 0 (underground)");
        CursorFlashlightIntensityAboveGroundDay = ConfigReference.Bind("Flashlight", "Intensity above ground (day)", 0f, "Floor 0 and higher during day (not evening/night per game clock). 0 disables the torch for that period.");
        CursorFlashlightIntensityAboveGroundNight = ConfigReference.Bind("Flashlight", "Intensity above ground (night)", 1f, "Floor 0 and higher during evening/night (TimeController.IsNight). 0 disables the torch for that period.");
        CursorFlashlightRange = ConfigReference.Bind("Flashlight", "Range", CursorFlashlightRangeDefault, "How far the spotlight reaches (world units); past this, no light. Above the default range, intensity is scaled down so the center does not blow out to white.");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
