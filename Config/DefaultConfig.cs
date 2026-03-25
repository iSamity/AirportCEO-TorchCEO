using BepInEx.Configuration;

namespace TorchCEO.Config;

static class DefaultConfig
{
    internal static ConfigEntry<bool> ShowWelcomeMessage { get; private set; }

    internal static ConfigEntry<bool> CursorFlashlightEnabled { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightIntensityBelowGround { get; private set; }
    internal static ConfigEntry<float> CursorFlashlightIntensityAboveGround { get; private set; }

    public static void Setup()
    {
        ShowWelcomeMessage = ConfigReference.Bind("General", "Show TorchCEO welcome message", true, "Show a one-time welcome when the game starts explaining the mod.");

        CursorFlashlightEnabled = ConfigReference.Bind("Flashlight", "Enable cursor flashlight", false, "In-game only: point light that follows the mouse on the current floor");
        CursorFlashlightIntensityBelowGround = ConfigReference.Bind("Flashlight", "Intensity below ground", 5f, "Floors with Z below 0 (underground)");
        CursorFlashlightIntensityAboveGround = ConfigReference.Bind("Flashlight", "Intensity above ground", 1f, "Floor 0 and higher; 0 disables the torch on surface / upper levels");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
