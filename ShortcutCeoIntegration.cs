using BepInEx.Bootstrap;

namespace TorchCEO;

/// <summary>
/// Optional ShortcutCeo: <see cref="ShortcutCeoPluginGuid"/> is a BepInEx
/// <see cref="BepInDependency.DependencyFlags.SoftDependency"/>.
/// Actual calls to ShortcutCeo APIs live in <see cref="ShortcutCeoShortcutRegistration"/> so the ShortcutCeo
/// assembly is only loaded when that type is first used (after <see cref="IsShortcutCeoLoaded"/>).
/// </summary>
internal static class ShortcutCeoIntegration
{
    internal const string ShortcutCeoPluginGuid = "org.iSamity.plugins.ShortcutCeo";

    internal static bool IsShortcutCeoLoaded()
    {
        return Chainloader.PluginInfos.TryGetValue(ShortcutCeoPluginGuid, out var info)
            && info.Instance != null;
    }
}
