using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class ShopItemDataPatch
{
    [HarmonyPatch(typeof(ShopItemData.SellerActiveInfo), "ReadSaveData")]
    [HarmonyPostfix]
    private static void ShopItemData_SellerActiveInfo_ReadSaveData_Postfix(ShopItemData.SellerActiveInfo __instance)
    {
        for (int i = 0; i < __instance.m_productsData.Count; i++)
        {
            if (__instance.m_productsData[i] == null)
                continue;

            foreach (ShopItemData.SellerActiveInfo.ShopItemData data in __instance.m_productsData[i].Values)
            {
                if (ParameterManagerLib.CustomShopItemDatas.Contains(data.MasterData) && data.MasterData.m_random >= 100)
                {
                    data.Active = true;
                    data.SoldOutTimeCount = 1;
                }

            }
        }

        __instance.UpdateSellerLineup();
    }
}
