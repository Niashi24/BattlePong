using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaturnRPG.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    [field: SerializeField]
    public float BaseSpeed { get; private set; } = 6f;

    [field: SerializeField]
    public float SpeedMultiplier { get; private set; } = 1.5f;

    [SerializeField]
    private float ballRadius = 0.5f;
    public float BallRadius => ballRadius;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private BallColorScript ballColor;

    // public float CurrentSpeed { get; private set; }
    public float CurrentMultiplier { get; private set; } = 0f;
    public Color Color => ballColor.Color;

    public Vector2 Direction { get; private set; } = Vector2.zero;
    public event Action<Vector2> OnDirectionChange;
    
    public float Speed { get; private set; } = 0f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSpeed(BaseSpeed);
        // StartRandom();
    }

    public void SetSpeed(float speed)
    {
        // CurrentMultiplier = Mathf.Log(speed / BaseSpeed, SpeedMultiplier);
        SetMultiplier(Mathf.Log(speed / BaseSpeed, SpeedMultiplier));
        // CurrentSpeed = speed;
    }

    public void SetMultiplier(float multiplier)
    {
        CurrentMultiplier = Mathf.Max(0, multiplier);
        float speed = GetSpeed();
        Speed = speed;
        ballColor.OnChangeMultiplier(CurrentMultiplier);
    }

    private void FixedUpdate()
    {

        var iterator = this.ToCircleCastEnumerable();

        var ((pos, vel, hit), trigger) = iterator
            .Select(x => (x, x.Item3.collider?.GetComponent<IBallTrigger>()))
            .FirstWhereOrLastOrDefault(x => x.Item2 != null, 
                ((_transform.position, Direction, new RaycastHit2D()), null));

        Vector2 prevDirection = Direction;

        if (trigger != null)
            (_transform.position, Direction) = trigger.HitBall(this, hit, pos, vel);
        else
            (_transform.position, Direction) = (pos, vel);
        
        if (prevDirection != Direction) OnDirectionChange?.Invoke(Direction);
    }

    private (Vector2, Vector2) Move(float distance, Vector2 position, Vector2 direction)
    {
        if (direction == Vector2.zero) return (position, direction);
        
        const int MAX_ITERATIONS = 5;
        int i = 0;

        Collider2D previousCollider = null;
        while (distance > 0)
        {
            if (i >= MAX_ITERATIONS)
            {
                Debug.Log("Hit max iterations");
                break;
            }
            
            var hit = Physics2D.CircleCast(position, ballRadius, Direction, distance, layerMask);
            if (hit && Vector2.Dot(hit.normal, Direction) < 0 && hit.collider != previousCollider)
            {
                float traveled = Vector2.Distance(position, hit.centroid);
                Debug.DrawLine(position, hit.centroid, Color.green, 1);
                position = hit.centroid;
                direction = direction.Reflect(hit.normal);

                if (hit.collider.TryGetComponent<IBallTrigger>(out var ballTrigger))
                {
                    return ballTrigger.HitBall(this, hit, position, direction);
                }

                distance -= traveled;
                previousCollider = hit.collider;
            }
            else
            {
                position += Direction * (Speed * Time.deltaTime);
                // distance = 0;
                break;
            }

            i++;
        }

        return (position, direction);
    }

    private float GetSpeed() => BaseSpeed * Mathf.Pow(SpeedMultiplier, CurrentMultiplier);

    public void AddMultiplier(float multiplier)
    {
        SetMultiplier(CurrentMultiplier + multiplier);
    }

    public void SetDirection(Vector2 dir)
    {
        // rbdy2D.velocity = dir.WithMagnitude(GetSpeed());
        Direction = dir;
        OnDirectionChange?.Invoke(Direction);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        SetMultiplier(CurrentMultiplier);
    }

    public void ResetBall()
    {
        transform.localPosition = Vector3.zero;
        SetMultiplier(0);
    }

    public void StartRandom()
    {
        SetDirection(Random.value > 0.5f ? Vector2.up : Vector2.down);
    }

    public CircleCastEnumerable ToCircleCastEnumerable()
    {
        return new CircleCastEnumerable(
            _transform.position,
            Direction,
            layerMask,
            ballRadius,
            Time.deltaTime * Speed
        );
    }

    public CircleBounceEnumerable ToCircleBounceEnumerable()
    {
        return new CircleBounceEnumerable(
            _transform.position,
            Direction,
            layerMask,
            ballRadius,
            float.PositiveInfinity
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, ballRadius);
    }
}
