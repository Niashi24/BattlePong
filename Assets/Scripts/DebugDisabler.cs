using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePong
{
    internal static class DebugDisabler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void DisableDebug()
        {
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        }
    }
}
