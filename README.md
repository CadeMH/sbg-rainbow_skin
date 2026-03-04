# Rainbow Skin
#### _A BepInEx5 mod to cycle through all available skin colors in Super Battle Golf._

# Usage
#### Press the keybind to toggle 

# TO-DO
#### Update the shop page to better display the correct skin color.

## Installation

- [Install BepInEx 5](https://github.com/BepInEx/BepInEx/releases)
- Download and drop the `rainbow_skin.dll` file in the `\BepInEx\plugins` folder of your Super Battle Golf installation
- After opening the game for the first time, you can adjust the toggle keybind and cycle speed in `\BepInEx\config\dev.cmax.rainbow_skin.cfg`

## Building for source
Set the `GameDir` varible in `rainbow_skin.csproj` to your Super Battle Golf installation directory, then run:
```sh
dotnet build
```

This will create the mod's DLL file and copy it into your game's BepinEx plugins folder, creating the folder first if it doesn't exist.