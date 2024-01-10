using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class NGonLine : MonoBehaviour
{
    [SerializeField]
    private SpriteShapeController spriteShape;
    
    [SerializeField, Min(1)]
    private int n;

    // Start is called before the first frame update
    // void OnDrawGizmosSelected()
    // {
    //     if (spriteShape == null) return;
    //     Vector3[] verts = new Vector3[n + 1];
    //     for (int i = 0; i <= n; i++)
    //     {
    //         float angle = 2 * Mathf.PI * i / n;
    //         verts[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    //     }
    //     spriteShape.spline.Clear();
    //     foreach (var vert in verts)
    //         spriteShape.spline.InsertPointAt(0, vert);
    //     for (int i = 0; i < verts.Length; i++)
    //     {
    //         spriteShape.spline.SetHeight(i, 0.1f);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
