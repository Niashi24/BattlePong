using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePong
{
    public class FadeCone : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField, Min(0.001f)]
        private float duration = 1f;

        [SerializeField]
        private AnimationCurve rCurve;

        private float _timer = 0;

        private void OnEnable()
        {
            _timer = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            SetR(rCurve.Evaluate(Mathf.Clamp01(_timer / duration)));
        }

        private static readonly int MaxR = Shader.PropertyToID("_MaxR");

        public void SetR(float r)
        {
            spriteRenderer.material.SetFloat(MaxR, r);
        }
    }
}
