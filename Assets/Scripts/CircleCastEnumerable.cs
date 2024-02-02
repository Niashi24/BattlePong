
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaturnRPG.Utilities.Extensions;

public class CircleCastEnumerable: IEnumerable<(Vector2, Vector2, RaycastHit2D)>
{
    private float _radius;
    private LayerMask _layerMask;
    
    private Vector2 _startPos;
    private Vector2 _startDir;
    private float _startDistance;

    public CircleCastEnumerable(Vector2 pos, Vector2 dir,
        LayerMask layerMask, float radius, float distance = float.PositiveInfinity)
    {
        _startPos = pos;
        _startDir = dir;
        _layerMask = layerMask;
        _radius = radius;
        _startDistance = distance;
    }
    
    public IEnumerator<(Vector2, Vector2, RaycastHit2D)> GetEnumerator()
    {
        return CircleRayBounce(
            _startPos,
            _startDir,
            _layerMask,
            _radius,
            _startDistance
        );
    }

    private static IEnumerator<(Vector2, Vector2, RaycastHit2D)> CircleRayBounce(Vector2 pos, Vector2 dir,
        LayerMask layerMask, float radius, float distance = float.PositiveInfinity)
    {
        if (dir.sqrMagnitude == 0f) yield break;
        RaycastHit2D prev = new RaycastHit2D();
        
        while (true)
        {
            var hit = Physics2D.CircleCast(
                pos,
                radius,
                dir,
                distance,
                layerMask
            );
            if (hit && Vector2.Dot(hit.normal, dir) < 0 && hit.collider != prev.collider)
            {
                float traveled = Vector2.Distance(pos, hit.centroid);
                pos = hit.centroid + hit.normal * 0.01f;
                dir = dir.Reflect(hit.normal);
                distance -= traveled;
                prev = hit;
                yield return (pos, dir, hit);
            }
            else
            {
                if (float.IsInfinity(distance))
                    distance = 10000f;
                pos += dir * distance;
                yield return (pos, dir, hit);
                yield break;
            }
        }
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}