
using System;
using System.Linq;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;
using Ray2D = Freya.Ray2D;

public class AIPaddleInput : PaddleInput
{
    [SerializeField]
    private BallScript ball;

    [SerializeField]
    private PaddleScript paddle;

    [SerializeField]
    private int bounceLookAhead = 1;

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
            if (Mathf.Abs(dx) < 0.01f)
                _reachedTarget = true;

            return dx.Sign0().Log(this) * -Mathf.Cos(_paddleTransform.eulerAngles.z * Mathf.Deg2Rad).Log(this);
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
            _targetX = calcTargetX().Log();
            _reachedTarget = false;
            _special = true;
        }
        
        // Debug.Log($"{_xDir} {_special}");
    }

    private float calcTargetX()
    {
        var ray = new Line2D(_paddleTransform.position, _paddleTransform.right);
        float target = _paddleTransform.localPosition.x;
        int i = 0;
        foreach (var (start, end) in ball.ToCircleBounceEnumerable())
        {
            
            var segment = new LineSegment2D(start, end);
            Debug.DrawLine(start, end, Color.green, Time.deltaTime);
            if (IntersectionTest.LinearIntersectionPoint(ray, segment, out var point))
            {
                return _paddleTransform.InverseTransformPoint(point).x - _paddleTransform.localPosition.x;
            }
            else if (i + 1 == bounceLookAhead)
            {
                point = _paddleTransform.rotation * ((Vector3)end - _paddleTransform.position);
                return _paddleTransform.InverseTransformPoint(point).x - _paddleTransform.localPosition.x;
            }

            i++;
            if (i >= bounceLookAhead) break;
        }

        return target;
    }

}