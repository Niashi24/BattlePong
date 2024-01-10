using System;
using System.Collections;
using System.Collections.Generic;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;

public class OutlineNGon : MonoBehaviour
{
    [SerializeField]
    private int n = 12;

    [SerializeField]
    private LineRenderer primaryRenderer;

    [SerializeField]
    private LineRenderer secondaryRenderer;

    [SerializeField]
    private Color[] colors;

    [SerializeField, Min(0)]
    private float t = 0;

    [SerializeField]
    private float maxT = 3f;

    private void OnDrawGizmosSelected()
    {
        SetT(t);
    }

    public void SetT(float t)
    {
        this.t = t = Mathf.Clamp(t, 0, maxT);
        if (t == 0)
        {
            SetLocalT(0, primaryRenderer);
            SetLocalT(0, secondaryRenderer);
            return;
        }
        
        float t1 = t % 1 == 0 ? 1 : t % 1;
        float t2 = t > 1 ? 1 : 0;
        int c1 = Mathf.CeilToInt(t) - 1;
        int c2 = Math.Max(0, c1 - 1);
        SetLocalT(t1, primaryRenderer);
        primaryRenderer.startColor = primaryRenderer.endColor = colors[c1];
        SetLocalT(t2, secondaryRenderer);
        secondaryRenderer.startColor = secondaryRenderer.endColor = colors[c2];
    }

    private void SetLocalT(float t, LineRenderer lineRenderer)
    {
        if (t == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        float numSides = t * n;
        int numFullSides = Mathf.CeilToInt(numSides) - 1;
        int numVerts = numFullSides + 2;
        float partialSide = numSides - numFullSides;
        // Debug.Log($"Num: {numSides}, fullSides: {numFullSides}, Partial: {partialSide}");
        
        Vector3[] verts = new Vector3[numVerts];
        for (int i = 0; i <= numFullSides; i++)
        {
            float angle = 2 * Mathf.PI * i / n;
            verts[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        
        verts[numVerts - 1] = Vector2.Lerp((2 * Mathf.PI * numFullSides / n).AngleToDirection(),
            (2 * Mathf.PI * (numFullSides + 1) / n).AngleToDirection(), 
            partialSide);

        lineRenderer.positionCount = verts.Length;
        lineRenderer.SetPositions(verts);
    }
}
