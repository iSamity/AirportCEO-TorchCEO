# TorchCEO

> Steam Workshop description is maintained in [`Steam/steamtext.txt`](Steam/steamtext.txt) (BBCode). This README mirrors that content in Markdown.

## Note

This mod requires the [AirportCEO Mod Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=3109136766) to function. Please make sure it is correctly downloaded and installed.

## Description

While enabled, a spotlight tracks the cursor on the current floor and points at the hit on that plane. Intensity is set separately for underground levels (Z below ground), for surface and upper floors during day, and again during evening/night (game clock). Set an above-ground value to 0 to keep the torch off for that period. You can tune how far the light reaches; past the configured range it fades out, and intensity is scaled down at longer ranges so the center does not blow out to white.

## Features

- Config: enable cursor flashlight (default: off)
- Toggle in-game with a keyboard shortcut (default **Ctrl+T**; configurable under Flashlight via [ShortcutCeo](https://github.com/iSamity/AirportCEO-ShortcutCeo-V4))
- Independent intensity for below ground, above ground by day, and above ground at night
- Configurable spotlight range with automatic intensity compensation for large ranges
- Depends on [ShortcutCeo](https://github.com/iSamity/AirportCEO-ShortcutCeo-V4) for the configurable shortcut (install alongside this mod)

## Version

Read more on [GitHub Releases](https://github.com/iSamity/AirportCEO-TorchCEO/releases).

- **v1.0.0** — Initial release: cursor spotlight, day/night above-ground intensities, configurable range

## Developers

Source and contribution info: [GitHub](https://github.com/iSamity/AirportCEO-TorchCEO).

[![Buy Me a Coffee](https://i.imgur.com/nyZtCjx.png)](https://buymeacoffee.com/isamity)
