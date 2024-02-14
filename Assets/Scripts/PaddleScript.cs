using System;
using System.Collections;
using System.Collections.Generic;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaddleScript : MonoBehaviour, IBallTrigger
{
    [Header("Components")]
    [SerializeField]
    private PaddleInput paddleInput;

    [SerializeField]
    private Rigidbody2D rbdy2D;

    [SerializeField]
    private Collider2D coll2D;
    
    [Header("Effects")]

    [SerializeField]
    private ParticleSystem chargeSystem;

    [SerializeField]
    private ParticleSystem chargeHitSystem;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip normalHitSFX;

    [SerializeField]
    private AudioClip chargeHitSFX;
    
    [Header("Paddle Parameters")]
    [SerializeField]
    private float moveSpeed = 120f;

    [Header("Ball Interaction Parameters")]
    [SerializeField]
    private FloatReference speedIncrease = new FloatReference(1);

    [SerializeField]
    private FloatReference speedDecrease = new FloatReference(-1);
    
    [SerializeField]
    private FloatReference baseChargeIncrease = new FloatReference(0.25f);
    
    [SerializeField]
    private FloatReference baseChargeReceieveIncrease = new FloatReference(0.5f);

    [field: SerializeField]
    private FloatReference maxCharges = new FloatReference(3);

    [field: SerializeField]
    private FloatReference chargeMultiplier = new FloatReference(1);
    
    [SerializeField]
    private AnimationCurve bounceAngleCurve;

    public float Charge { get; private set; }
    public bool ReadyToFire { get; private set; }

    public event Action<float> OnChargeChange;
    public event Action<bool> OnReadyFireChange;

    public event Action OnNormalHit;
    public event Action OnChargeHit;

    private Transform _transform;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        // This can be brought into a separate script if needed
        OnNormalHit += PlayNormalHitSFX;
        OnChargeHit += PlayChargeFX;
        OnChargeHit += PlayChargeHitSFX;
    }

    private void OnDisable()
    {
        OnNormalHit -= PlayNormalHitSFX;
        OnChargeHit -= PlayChargeFX;
        OnChargeHit -= PlayChargeHitSFX;
    }

    private void PlayChargeFX()
    {
        chargeHitSystem.Simulate(0);
        chargeHitSystem.Play();
    }

    private void PlayNormalHitSFX()
    {
        audioSource.PlayOneShot(normalHitSFX);
    }

    private void PlayChargeHitSFX()
    {
        audioSource.PlayOneShot(chargeHitSFX);
    }

    private void FixedUpdate()
    {
        float dir = paddleInput.XDir;
        rbdy2D.velocity = _transform.rotation * Vector3.right * (dir * moveSpeed);
    }

    private bool _specialBefore = false;

    private void LateUpdate()
    {
        if (!_specialBefore && paddleInput.Special && Charge >= 1f)
            SetReadyToFire(!ReadyToFire);
        chargeSystem.gameObject.SetActive(ReadyToFire && CanSpecial);
        _specialBefore = paddleInput.Special;
    }

    public (Vector2, Vector2) HitBall(BallScript ballScript, RaycastHit2D hit2D, Vector2 position, Vector2 direction)
    {
        var point = hit2D.point;
        var paddleSize = coll2D.bounds.size;
        float t = Vector2.Dot(point - (Vector2)_transform.position, _transform.right) / paddleSize.x * 2;
        float angle = bounceAngleCurve.Evaluate(t);
        angle += Random.Range(-5f, 5f);
        var dir = VectorExtensions.Rotate(_transform.up.ToVector2(), angle * Mathf.Deg2Rad);
        if (ReadyToFire && CanSpecial)
        {
            ballScript.AddMultiplier(speedIncrease.Value);
            Charge--;
            SetReadyToFire(false);
            
            OnChargeHit?.Invoke();
        }
        else
        {
            Charge = (Charge
                      + baseChargeIncrease * chargeMultiplier
                      + baseChargeReceieveIncrease * ballScript.CurrentMultiplier)
                .AtMost(maxCharges.Value);
            
            ballScript.AddMultiplier(speedDecrease.Value);
            
            OnNormalHit?.Invoke();
        }
        
        // Place ball at top of paddle
        var pos = new VectorMath2D().ProjPointToLine<Vector2, VectorMath2D>(
            _transform.position + _transform.up * (paddleSize.y / 2 + ballScript.BallRadius),
            _transform.right,
            hit2D.centroid);

        OnChargeChange?.Invoke(Charge);
        return (pos, dir);
    }

    public void ResetCharge()
    {
        Charge = 0;
        OnChargeChange?.Invoke(Charge);
        SetReadyToFire(false);
    }

    public bool CanSpecial => Charge >= 1;

    private void SetReadyToFire(bool readyToFire)
    {
        ReadyToFire = readyToFire;
        OnReadyFireChange?.Invoke(ReadyToFire);
    }

}
