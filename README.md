<div align="center">
  
# Sulfur Battle Improvements | 火湖战斗优化
[![BepInEx Version](https://img.shields.io/badge/BepInEx-5.x.x-green)](https://docs.bepinex.dev/)
[![License](https://img.shields.io/badge/License-GPL--3.0-blue)](LICENSE)

**改进火湖的战斗体验** | *Improve the sulfur's battle experience*

![demo](https://raw.githubusercontent.com/CmmmmmmLau/SulFur_Battle_improvement/refs/heads/master/doc/preview.png)

</div>

## Features | 核心功能
*All features are configurable via BepInEx/Config/*  
*所有功能均可通过BepInEx配置文件调整*

### ​**Battlefield 1 Style Feedback**​ | **战地1风格反馈系统**​  
<table> 
  <tr>
    <th>
      Hitmarker
    </th>
    <th>
      Impact Sound 
    </th>
    <th>
      Kill Message
    </th>
  </tr>
  <tr>
    <td>
      命中指示器
    </th>
    <td>
      打击音效
    </td>
    <td>
      击杀信息
    </td>
  </tr>
</table>

![hit](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/doc/killmessage.gif?raw=true)

### ​**Enhanced Loot Visibility**​ | **战利品可视性增强**​  
Dynamic drop effects with light tracking

光轨追踪的炫酷动态掉落效果

![lootvfx](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/doc/lootdrop_vfx.gif?raw=true)

### ​**Weapon Recovery System**​ | **武器寻回机制**​  
You can get your weapons back from the donation box.

你能从捐赠箱取回武器

*Of course... you have to lose something...*

*当然，这也是有代价的...*

![deadprotection](https://raw.githubusercontent.com/CmmmmmmLau/SulFur_Battle_improvement/refs/heads/master/doc/deadprotection.gif)

### ​**Bullet Collision Optimization**​ | ​**子弹碰撞优化**​ 

Bullet behavior reworked. Now your bullets won't be blocked by dead body, and neither will theirs.

子弹系统重做，现在你的子弹不会再被敌人尸体挡住了

![deadbody](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/blob/master/doc/deadbodycollision.gif?raw=true)

### ​**Quality of Life Improvements**​ | ​**体验优化**​ 
Enables the health bar in the dev tools.

开发工具中启用生命条显示

Every time you gain some experience on current weapon, your second weapon will also gain some experience.

也可启用副武器经验共享(部分经验)

## Configurable | 配置选项
*In-game config menu: Default key = F1*  
*游戏内配置菜单默认按键：F1*

| English Parameter               | 中文参数                |
|---------------------------------|-----------------------|
| Hit sound volume & distance     | 命中音效音量与传播距离  |
| Hitmarker color customization   | 命中指示器颜色自定义    |
| Kill message volume             | 击杀信息音量           |
| Weapon durability loss ratio    | 武器耐久损耗比例       |
| Attachment loss probability     | 配件丢失概率           |

## Game Compatibility | 说明
**Only supports latest Sulfur version**​  
**仅支持最新版本**​  
*For legacy versions, use older mod releases*  
*旧版本游戏请使用历史模组文件*

## Manual Installation | 安装指南
1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2)  
   安装BepInEx 5框架
   
2. Download this mod from [Releases](https://github.com/CmmmmmmLau/SulFur_Battle_improvement/releases)  
   从发布页面下载模组
   
3. Unzip to `SULFUR\BepInEx\plugins\`  
   解压到指定插件目录
   
4. Launch game and configure, enjoy~
   启动游戏进行配置

## What's Next? | 开发计划
- ~~Death Protection~~ ✅ Completed / 已完成
- Battlefield V Style 🚧 In Progress / 开发中
- ~~Loot Light Beam~~ ✅ Completed / 已完成
- Friendly Fire Removal 🚧 In Progress / 开发中

# Know Issue | 已知问题
Currently only able to record player's bullet damage. Other type of damage date not include the damage source, therefore it will take more work to try to record it.
当前仅记录玩家子弹伤害，其他伤害类型需要额外开发工作。

## 📜 License | 许可证

GPL-3.0 License - see [LICENSE](LICENSE) for details.  
GPL-3.0 许可证 - 详见 [LICENSE](LICENSE) 文件.


# Copyright | 版权声明
- Audio/Texture: Electronic Arts, DICE
- UI library: Hirashi3630/UrGUI

<div align="center">

**Enjoy Mod | ​游玩愉快**​  

</div>
