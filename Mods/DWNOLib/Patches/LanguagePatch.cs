using DWNOLib.Library;
using HarmonyLib;
using UnityEngine;

namespace DWNOLib.Patches;
internal class LanguagePatch
{
    /// <summary>
    /// Looks in our LanguageStringDatas if we have an id linked to the hash string instead, otherwise continue the original function.
    /// </summary>
    [HarmonyPatch(typeof(Language), "makeHash")]
    [HarmonyPrefix]
    private static bool Language_makeHash_Prefix(string hashString, ref uint __result)
    {
        if (AppMainScript.m_instance == null || Language.m_systemLangage == -1)
            return false;

        if (ParameterManagerLib.LanguageStringDatas.ContainsKey(Language.systemLaungage))
        {
            if (ParameterManagerLib.LanguageStringDatas[Language.systemLaungage].ContainsKey(hashString))
            {
                __result = ParameterManagerLib.LanguageStringDatas[Language.systemLaungage][hashString];
                return false;
            }
        }

        if (ParameterManagerLib.LanguageStringDatas[SystemLanguage.English].ContainsKey(hashString))
        {
            __result = ParameterManagerLib.LanguageStringDatas[SystemLanguage.English][hashString];
            return false;
        }

        return true;
    }

    /// <summary>
    /// Looks in our LanguageDatas if we have the id instead, otherwise continue the original function.
    /// </summary>
    [HarmonyPatch(typeof(Language), "GetString", new System.Type[] { typeof(uint) })]
    [HarmonyPrefix]
    private static bool Language_GetString_uint_Prefix(uint lang_code, ref string __result)
    {
        if (ParameterManagerLib.LanguageDatas.ContainsKey(Language.systemLaungage))
        {
            if (ParameterManagerLib.LanguageDatas[Language.systemLaungage].ContainsKey(lang_code))
            {
                __result = ParameterManagerLib.LanguageDatas[Language.systemLaungage][lang_code];
                return false;
            }
        }

        if (ParameterManagerLib.LanguageDatas[SystemLanguage.English].ContainsKey(lang_code))
        {
            __result = ParameterManagerLib.LanguageDatas[SystemLanguage.English][lang_code];
            return false;
        }

        return true;
    }
}
