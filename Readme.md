Trainers
===

<a id="requirements"></a>
### Requirements
- [.NET Framework 4.7.2 runtime](https://dotnet.microsoft.com/download/dotnet-framework) or later (included in Windows 10 version 1803 and later)

### Arguments
- `--enable-cheat "CHEAT NAME"`
    - Optional, can be passed 0 or more times.
    - Automatically enables the given cheat on trainer startup, instead of you having to manually click the checkbox.
    - `CHEAT NAME` is the name of the cheat as it appears in the trainer UI, such as `No speed limit`. Case-insensitive.
    - Useful if you always want to play a game with a given cheat active, and you want to launch the game with a batch file that both starts the game and starts the trainer with this cheat enabled.
        ```bat
        @echo off
        Dakar2Game.exe
        DakarDesertRallyTrainer.exe --enable-cheat "No speed limit"
        ```

### Games
<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="false" levels="1,2" style="unordered" -->

- [Dakar 18](#dakar-18)
- [Dakar Desert Rally](#dakar-desert-rally)
- [Superhot: Mind Control Delete](#superhot-mind-control-delete)

<!-- /MarkdownTOC -->

## Dakar 18

![trainer screenshot](.github/images/dakar18.png)

[💾 Download](https://github.com/Aldaviva/Trainers/releases/latest/download/Dakar18Trainer.exe)

- No speed limit
    - All speed limit penalties and warnings are removed in all difficulty levels.
    - You are still automatically limited to 30 km/h in passage control auto-driving zones.

### Supported game versions

- **v.13**, released in March 2019, which contains Desafio Ruta 40 Rally and Inca Rally

## Dakar Desert Rally

![trainer screenshot](.github/images/dakardesertrally.png)

[💾 Download](https://github.com/Aldaviva/Trainers/releases/latest/download/DakarDesertRallyTrainer.exe)

- No speed limit
    - All speed limit penalties, and red and yellow speed warnings, are removed in all difficulty levels.
    - The gray speed limit warning is still visible in Simulation Mode and areas with speed limits, like villages and passage control.
    - Hilariously, you are *not* limited to 30 km/h in passage control auto-driving zones, unlike in [Dakar 18](#dakar-18). Therefore, you may want to slow down and disable this cheat before entering passage control, in order to avoid missing the exit and getting stuck in auto-drive for the rest of the stage.

### Supported game versions

- [**1.7.0**](https://store.steampowered.com/news/app/1839940/view/3654145459652107245), released on February 28, 2023, which has build number `10618159`. This version introduced the SnowRunner Truck Pack add-on.
- [**1.6.0**](https://store.steampowered.com/news/app/1839940/view/3644009189553404411), released on January 20, 2023, which has build number `10366290`. This version introduced the Free Roam game mode.
- [**1.5.0**](https://store.steampowered.com/news/app/1839940/view/5379014706391343864), released on November 15, 2022, which has build number `9902125`. This version introduced 7 new events in the Extended Saudi Arabia Map add-on.

## Superhot: Mind Control Delete

![trainer screenshot](.github/images/superhotmindcontroldelete.png)

[💾 Download](https://github.com/Aldaviva/Trainers/releases/latest/download/SuperhotMindControlDeleteTrainer.exe)

- Infinite health

### Supported game versions

- **1.0.0**, which has file version `2018.4.5.14584`
