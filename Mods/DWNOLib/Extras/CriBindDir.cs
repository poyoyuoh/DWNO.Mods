using BepInEx;
using HarmonyLib;
using System.IO;

// CriBindDir by https://github.com/SutandoTsukai181/DWNO-Mods
namespace DWNOLib.Extras;
internal class CriBindDir
{
    internal static AssetBundleManager ABManager;

    [HarmonyPatch(typeof(CriWareInitializer), "Awake")]
    [HarmonyPrefix]
    private static bool CriWareInitializer_Awake_Prefix(CriWareInitializer __instance)
    {
        __instance.fileSystemConfig.numberOfBinders = 16;
        return true;
    }

    [HarmonyPatch(typeof(AssetBundleManager), "Awake")]
    [HarmonyPrefix]
    private static bool AssetBundleManager_Awake_Prefix(AssetBundleManager __instance)
    {
        
        if (!Directory.Exists(Path.Combine(Paths.PluginPath, "BindDirectory")))
            Directory.CreateDirectory(Path.Combine(Paths.PluginPath, "BindDirectory"));
        ABManager = __instance;
        return true;
    }

    [HarmonyPatch(typeof(CriFsUtility), "BindCpk", new System.Type[] { typeof(CriFsBinder), typeof(string) })]
    [HarmonyPrefix]
    private static bool CriFsUtility_BindCpk_Prefix(CriFsBinder targetBinder, string srcPath)
    {
        if (ABManager == null)
        {
            Logger.Log($"ABManager instance not found! Directory binding skipped", Logger.LogType.Error);
            return true;
        }
        var bindRequest = CriFsUtility.BindDirectory(targetBinder, Path.Combine(Paths.PluginPath, "BindDirectory"));
        bindRequest.WaitForDone(ABManager);
        CriFsBinder.SetPriority(bindRequest.bindId, 5000);
        return true;
    }
}
