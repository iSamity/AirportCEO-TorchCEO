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
        ShowWelcomeMessage = ConfigReference.Bind("General", "Show TorchCEO welcome message", true, "Show a short welcome the first time you start the game after installing the mod.");

        CursorFlashlightEnabled = ConfigReference.Bind("Flashlight", "Enable cursor flashlight", false, "Turns on a light that follows your mouse on the floor you are viewing.");
        CursorFlashlightIntensityBelowGround = ConfigReference.Bind("Flashlight", "Intensity below ground", 2f, "How bright the light is on underground floors (below the surface). Set to 0 to turn it off there.");
        CursorFlashlightIntensityAboveGroundDay = ConfigReference.Bind("Flashlight", "Intensity above ground (day)", 0f, "How bright the light is on ground level and upper floors during bright daylight. Set to 0 to keep the torch off in daytime.");
        CursorFlashlightIntensityAboveGroundNight = ConfigReference.Bind("Flashlight", "Intensity above ground (night)", 1f, "How bright the light is on ground level and upper floors when it is dark outside (evening and night). Set to 0 to turn it off after dark.");
        CursorFlashlightRange = ConfigReference.Bind("Flashlight", "Range", CursorFlashlightRangeDefault, "How far the light reaches before it fades away. Very large values are toned down in the middle so the hotspot does not become an overly bright white circle.");
    }

    static ConfigFile ConfigReference => Plugin.ConfigReference;
}
