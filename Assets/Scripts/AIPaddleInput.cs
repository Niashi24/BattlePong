
using System;
using System.Linq;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;
using Ray2D = Freya.Ray2D;

public class AIPaddleInput : PaddleInput
{
    [SerializeField]
    private BallScript ball;

    [SerializeField]
    private PaddleScript paddle;

    [SerializeField]
    private int bounceLookAhead = 1;

    [SerializeField]
    private float chanceToUseSpecial = 0.25f;

    [SerializeField]
    private float targetVariance = 0.2f;

    [SerializeField]
    private float speed = 1f;

    private Transform _ballTransform;
    private Transform _paddleTransform;

    private bool _special = false;
    private float _targetX = 0f;
    private bool _reachedTarget = false;

    public override float XDir
    {
        get
        {
            if (_reachedTarget) return 0f;

            float dx = (_targetX - _paddleTransform.localPosition.x);
            if (Mathf.Abs(dx) < 0.1f)
                _reachedTarget = true;

            return dx.Sign0() * _paddleTransform.right.x * speed; //* -Mathf.Cos(_paddleTransform.eulerAngles.z * Mathf.Deg2Rad);
        }
    }

    public override bool Special => _special;
    

    private void Awake()
    {
        _ballTransform = ball.transform;
        _paddleTransform = paddle.transform;
    }

    private void OnEnable()
    {
        ball.OnDirectionChange += OnBallDirectionChanged;
    }

    private void OnDisable()
    {
        ball.OnDirectionChange -= OnBallDirectionChanged;
    }

    private void OnBallDirectionChanged(Vector2 direction)
    {
        if (Vector2.Dot(direction, _paddleTransform.up) >= 0f || !ball.isActiveAndEnabled)
        {
            _reachedTarget = true;
            _special = false;
        }
        else
        {
            _targetX = calcTargetX() * _paddleTransform.right.x;
            _targetX += Random.Range(-targetVariance, targetVariance);
            _reachedTarget = false;
            _special = ShouldUseSpecial();
        }
        
        // Debug.Log($"{_xDir} {_special}");
    }

    private bool ShouldUseSpecial()
    {
        if (paddle.Charge >= 2f) return true;
        if (ball.CurrentMultiplier > 0f) return true;
        return Random.value < chanceToUseSpecial;
    }

    private float calcTargetX()
    {
        return calcTargetXLocal() * _paddleTransform.right.x;
        // float targetX = 
    }

    private float calcTargetXLocal()
    {
        var line = new Line2D(_paddleTransform.position, _paddleTransform.right);
        int i = 0;
        foreach (var (start, end) in ball.ToCircleBounceEnumerable())
        {
            
            var segment = new LineSegment2D(start, end);
            Debug.DrawLine(start, end, Color.green, 0.5f);
            if (IntersectionTest.LinearIntersectionPoint(line, segment, out var point))
            {
                ObjectExtensions.DebugCircle(point, 1, Color.red, 0.5f);
                return point.x;
                // return _paddleTransform.InverseTransformPoint(point).x;
            }
            else if (i + 1 == bounceLookAhead)
            {
                point = end;
                ObjectExtensions.DebugCircle(point, 1, Color.green, 0.5f);
                return point.x;
            }

            i++;
            if (i >= bounceLookAhead) break;
        }

        return line.origin.x;
    }

}