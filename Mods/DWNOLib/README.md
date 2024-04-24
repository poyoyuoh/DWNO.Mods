# DWNOLib
 Modding library for Digimon World Next Order.

 DWNOLib help you create and modify the game parameter with ease, such as adding new Digimon into the game.
 It also have a new save system, which break the imposed limit of the game (for example, the game would only allow a maximum of 320 digimons due to how the save file worked!).
 And it make modding persistent data also easily possible!

 The library is currently in ALPHA stage, as such, it is not recommended to use it, as many thing will change/break.

# TODO (For BETA stage)
 HIGH PRIORITY:
 - Finish the save system. Currently not everything is saved/loaded.
 - Find a way to make AssetBundle of digimon work when creating them from unity. (not directly related to the library, but essential)
 - Make a class Database of all the original parameter.
 Medium priority:
 - Allow creating new item, and add/edit existing item data type. (Food, Material, KeyItem, etc...)
 - Add a way to remove original parameters. (Common window, placements, shop, etc...)
 - Make editing original talk event possible. (The csvb file for them are inside the talkMain class)
 - Allow adding/changing Enemy level parameters.
 - Allow adding/changing Player level parameters.
 - Add support for "scenario-like" dialog to DialogManager.
 - Integrate CriBindDir into the library.
 Low priority:
 - Bugfix: fix the error spam after a battle or action. This is from base game. Probably related to animation.
 - Comment the codes.
 - Make a documentation on how to use the library.

# TODO (For Stable/Release) - End goal
 - Make possible to create new map.
 - Make video tutorial on how to use the library (as well as how to create assetbundle for digimon and such)