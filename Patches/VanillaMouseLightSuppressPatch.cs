using HarmonyLib;
using TorchCEO.Config;

namespace TorchCEO.Patches;

/// <summary>
/// Vanilla build/demolish uses <see cref="MouseLightHandler"/> for a cursor light when it is dark outside.
/// TorchCEO replaces that behavior with its own flashlight; suppress the stock light while this mod is loaded.
/// </summary>
[HarmonyPatch(typeof(MouseLightHandler), nameof(MouseLightHandler.EnableLight))]
internal static class VanillaMouseLightSuppressPatch
{
    static bool Prefix()
    {
        if (DefaultConfig.CursorFlashlightEnabled.Value == true)
        {
            return false;
        }

        return true;
    }
}
