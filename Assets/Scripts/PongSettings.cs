using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePong
{
    [CreateAssetMenu]
    public class PongSettings : ScriptableObject
    {
        [field: SerializeField, Tooltip("Time (in seconds) until overtime starts.")]
        public int OvertimeTime { get; private set; } = 60;

        [field: SerializeField, Tooltip("Number of set wins to win a match.")]
        public int FirstToN { get; private set; } = 5;
    }
}
