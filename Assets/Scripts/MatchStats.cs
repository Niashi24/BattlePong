using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BattlePong
{
    public class MatchStats : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text firstToText;

        [SerializeField]
        private TMP_Text timerText;

        public void SetFirstTo(int ft)
        {
            firstToText.text = $"FT{ft}";
        }

        public void SetTimer(string timerText)
        {
            this.timerText.text = timerText;
        }
    }
}
