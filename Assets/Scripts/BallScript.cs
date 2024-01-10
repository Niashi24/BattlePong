using System;
using System.Collections;
using System.Collections.Generic;
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
    //
    // [SerializeField]
    // private Rigidbody2D rbdy2D;

    [SerializeField]
    private float ballRadius = 0.5f;
    public float BallRadius => ballRadius;
    //
    // [SerializeField]
    // private CircleCollider2D circleCollider;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private BallColorScript ballColor;

    // public float CurrentSpeed { get; private set; }
    public float CurrentMultiplier { get; private set; } = 0f;
    public Color Color => ballColor.Color;

    public Vector2 Direction { get; private set; } = Vector2.zero;
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
        // rbdy2D.velocity = rbdy2D.velocity.WithMagnitude(GetSpeed());
        ballColor.OnChangeMultiplier(CurrentMultiplier);
    }

    private void FixedUpdate()
    {
        // Physics2D.CircleCast(transform.position, circleCollider.radius, Direction);
        // var hit = Physics2D.CircleCast(_transform.position, ballRadius, Direction, Speed * Time.deltaTime, layerMask);
        // if (hit)
        // {
        //     // Direction = Direction.Reflect(hit.normal);
        //     SetDirection(Direction.Reflect(hit.normal));
        //     Debug.Log("here");
        // }
        // _transform.Translate(Speed * Time.deltaTime * Direction);

        (_transform.position, Direction) = Move(Time.deltaTime * Speed, _transform.position, Direction);
        // rbdy2D.MovePosition(rbdy2D.position + Direction * (Speed * Time.deltaTime));
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

    private void MoveRecursive(float distance, ref Vector2 position, ref Vector2 direction)
    {
        while (true)
        {
            if (distance <= 0) return;

            var hit = Physics2D.CircleCast(position, ballRadius, Direction, distance, layerMask);
            if (hit)
            {
                float traveled = Vector2.Distance(position, hit.centroid);
                position = hit.centroid;
                direction = direction.Reflect(hit.normal);

                if (hit.collider.CompareTag("Player"))
                {
                    // Do paddle stuff

                    return;
                }

                distance = distance - traveled;
                continue;
            }

            break;
        }
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
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        SetMultiplier(CurrentMultiplier);
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        SetMultiplier(0);
    }

    public void StartRandom()
    {
        SetDirection(Random.value > 0.5f ? Vector2.up : Vector2.down);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, ballRadius);
    }
}
