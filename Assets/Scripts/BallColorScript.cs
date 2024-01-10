using System.Collections;
using System.Collections.Generic;
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

    private Color GetColor(float multiplier)
    {
        Color color = defaultColor;
        foreach (var colorT in colorBySpeedMultiplier)
            if (multiplier >= colorT.t)
                color = colorT.color;
        
        return color;
    }
}
