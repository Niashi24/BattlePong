using System.Collections;
using System.Collections.Generic;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

public class BallColorScript : MonoBehaviour
{
    [SerializeField]
    private BallScript ballScript;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private ColorT[] colorBySpeedMultiplier;

    [System.Serializable]
    private struct ColorT
    {
        public Color color;
        public float t;
    }

    public Color Color { get; private set; } = Color.white;

    public void OnChangeMultiplier(float multiplier)
    {
        trailRenderer.startColor = trailRenderer.endColor = Color = GetColor(multiplier);
    }

    // private Color GetColor(float multiplier)
    // {
    //     Color color = defaultColor;
    //     foreach (var colorT in colorBySpeedMultiplier)
    //         if (multiplier >= colorT.t)
    //             color = colorT.color;
    //     
    //     return color;
    // }

    private Color GetColor(float multiplier)
    {
        if (colorBySpeedMultiplier.Length == 0) return defaultColor;

        var i = colorBySpeedMultiplier.LastIndexWhere((x) => multiplier >= x.t);
        var ipp = (i + 1).AtMost(colorBySpeedMultiplier.Length - 1);

        if (i == ipp) return colorBySpeedMultiplier[i].color;

        return Color.Lerp(
            colorBySpeedMultiplier[i].color,
            colorBySpeedMultiplier[ipp].color,
            Mathf.InverseLerp(colorBySpeedMultiplier[i].t, colorBySpeedMultiplier[ipp].t, multiplier)
        );
    }
}
