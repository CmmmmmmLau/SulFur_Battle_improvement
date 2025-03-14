# Battle Improvements
Add some feature to improve the sulfur's battle experience

![demo](https://raw.githubusercontent.com/CmmmmmmLau/SulFur_Battle_improvement/refs/heads/master/docs/preview.png)
## Features
All features can be turned off by cfg in BepInEx/Config folder

Battlefield 1/5 style kill message

![hit](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/docs/killmessage.gif?raw=true)

Loot drop effect, never miss your loot again.

![lootvfx](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/docs/lootdrop_vfx.gif?raw=true)

You can get your weapons back from the donation box.

Of course... you have to lose something...

![deadprotection](https://raw.githubusercontent.com/CmmmmmmLau/SulFur_Battle_improvement/refs/heads/master/docs/deadprotection.gif)

Bullet behavior reworked. Now your bullets won't be blocked by dead body, and neither will theirs.

![deadbody](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/docs/deadbodycollision.gif?raw=true)

Enables the health bar in the dev tools.

Every time you gain some experience on current weapon, your second weapon will also gain some experience.


## Configurable
- In Game config menu, default open key is F1
- Volume and distance of the hit sound.
- Color of the hitmarker.
- Volume of the kill message.
- How many weapon durability you will lose when you get your weapon back.
- The chance of the attachment and enhancement will be lost when you get your weapon back.

## Game Compatibility
**Only Latest version of Sulfur is supported.**
For older version, please try the older released mod.

## Manual Installation
1. Download ant install the bepinex 5 from [github](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2)
2. Download this mod from github [release page](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/releases)
3. Unzip the file in ``SULFUR\BepInEx\plugins\`` folder.
4. Enjoy

## What's Next?
- ~~Death Protection~~ Done
- battlefield 5 style
- ~~Loot light beam~~ Done
- Remove Friendly Fire

# Know Issue
Currently only able to record player's bullet damage. Other type of damage date not include the damage source, therefore it will take more work to try to record it.

# Copyright
Audio/Texture: Electronic Arts, DICE

UI library: Hirashi3630/UrGUI

