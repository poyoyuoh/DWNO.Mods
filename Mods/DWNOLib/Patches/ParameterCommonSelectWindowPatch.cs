using HarmonyLib;

namespace DWNOLib.Patches;
internal class ParameterCommonSelectWindowPatch
{
    [HarmonyPatch(typeof(ParameterCommonSelectWindow), "GetRecodeList", new System.Type[] { typeof(ParameterCommonSelectWindowMode) }, new ArgumentType[] { ArgumentType.Ref })]
    [HarmonyPrefix]
    private static bool ParameterCommonSelectWindow_GetRecodeList_Prefix(ref ParameterCommonSelectWindowMode param_common_select_window_mode, ref Il2CppSystem.Collections.Generic.List<ParameterCommonSelectWindow> __result)
    {
        if (param_common_select_window_mode == null)
        {
            __result = null;
            return false;
        }
        Csvb<ParameterCommonSelectWindow> commonSelectWindowCsvb = param_common_select_window_mode.GetCommonSelectWindowCsvb();
        if (commonSelectWindowCsvb == null)
        {
            __result = null;
            return false;
        }
        int recordMax = commonSelectWindowCsvb.m_params.Length;
        Il2CppSystem.Collections.Generic.List<ParameterCommonSelectWindow> list = new Il2CppSystem.Collections.Generic.List<ParameterCommonSelectWindow>();
        list.Capacity = recordMax;
        Il2CppSystem.Collections.Generic.List<ParameterCommonSelectWindow> list2 = list;
        ParameterCommonSelectWindow parameterCommonSelectWindow = null;
        for (int i = 0; i < recordMax; i++)
        {
            parameterCommonSelectWindow = commonSelectWindowCsvb.m_params[i];
            if (parameterCommonSelectWindow != null)
            {
                list2.Add(parameterCommonSelectWindow);
            }
        }
        __result = list2;
        return false;
    }
}
