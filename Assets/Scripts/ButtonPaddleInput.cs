using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePong
{
	public class ButtonPaddleInput : PaddleInput
	{
		[SerializeField]
		private ControlButton left, right, special;

		[SerializeField]
		private PaddleScript paddleScript;

		private void OnEnable()
		{
			paddleScript.OnChargeChange += SetChargeEnabledIfAble;
			paddleScript.OnReadyFireChange += HighlightCharge;
			SetChargeEnabledIfAble(paddleScript.Charge);
		}

		private void OnDisable()
		{
			paddleScript.OnChargeChange -= SetChargeEnabledIfAble;
			paddleScript.OnReadyFireChange -= HighlightCharge;
		}

		public override float XDir
		{
			get
			{
				float x = 0;
				if (right.Active) x++;
				if (left.Active) x--;
				return x;
			}
		}
		
		public override bool Special => special.Active;

		private void SetChargeEnabledIfAble(float charge)
		{
			special.SetCharge(charge);
			special.SetEnabled(paddleScript.CanSpecial);
		}

		private void HighlightCharge(bool readyToFire)
		{
			special.overrideHighlight = readyToFire;
		}
	}
}