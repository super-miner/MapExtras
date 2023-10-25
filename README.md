# MapExtras
 This is a mod for the game Core Keeper that adds various features to the map. You can find the mod io page [here](https://mod.io/g/corekeeper/m/map-extras)

# Roadmap/TODO
- Code refactoring
    - Create a few generic scripts for console logging, config, UI that can be shared between this mod and Map Extras (this might be able to be it's own library mod if the dependency system is working)
    - Add logging wherever possible to help with finding errors
    - Make some classes (like MapCircle) derive from MonoBehaviour
    - Find container objects using name instead of child id and fix the debug system for this process
    - Move config logic from MapManager to its own class
- Display player's usernames and health on the map
- Allow you to set border colors for each portal icon on the map
- Fix lag when loading large circles
