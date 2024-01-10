using System;
using System.Collections;
using System.Collections.Generic;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattlePong
{
    public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private InputButtonDisplay fill;

        [SerializeField]
        private OutlineNGon outlineNGon;

        [SerializeField]
        private SmoothValue chargeValue;

        public bool Active { get; private set; } = false;
        public bool Enabled { get; private set; } = true;
        public bool overrideHighlight = false;

        private void SetActive(bool active)
        {
            Active = active;
            fill.SetHighlight(overrideHighlight || active, Enabled);
        }

        private void Update()
        {
            outlineNGon.SetT(chargeValue.Tick());
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            fill.SetHighlight(Active, Enabled);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetActive(true);
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if(pointerEventData.eligibleForClick)
                SetActive(true);
        }

        //Detect if clicks are no longer registering
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetActive(false);
        }

        public void SetCharge(float charge)
        {
            chargeValue.target = charge;
        }
    }
}
