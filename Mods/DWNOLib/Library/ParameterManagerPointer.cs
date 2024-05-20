using System;

namespace DWNOLib.Library;
public class ParameterManagerPointer
{
    /// <summary>
    /// Pointer reference for the HashIdSearchClass.
    /// The Csvb SoloCameraData is not inside the ParameterManager, but in uDigiviceBG instead.
    /// </summary>
    public static IntPtr DigiviceSoloCameraPointer { get; set; }

    public static IntPtr PlacementEnemyPointer { get; set; }

    public static IntPtr PlacementNPCPointer { get; set; }

    public static IntPtr NPCEnemyPointer { get; set; }
}
