using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(HealthManager))]


public class RangedSkeleton : MonoBehaviour
{
    [Header("Movement")]

    public float walkAcceleration = 200f;
    public float maxSpeed = 200f;
    public float walkStopRate = 0.001f;

    [Header("References")]

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    public DetectionZone attackZone;
    public DetectionZone rangedDetectionZone;
    public DetectionZone cliffDetectionZone;
    HealthManager healthManager;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool _hasRangedTarget = false;

    public bool HasRangedTarget
    {
        get { return _hasRangedTarget; }
        private set
        {
            _hasRangedTarget = value;
            animator.SetBool(AnimationStrings.hasRangedTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }

        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    [Header("Patrolling Movement")]
    // Patrolling
    public bool Patrolling = false;
    public float waypointReachedDistance = 0.1f;
    public float patrolSpeed = 3f;

    public List<Transform> waypoints;
    Transform nextWaypoint;
    int waypointNumber = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        healthManager = GetComponent<HealthManager>();
    }

    private void Start()
    {
        if (Patrolling)
        {
            nextWaypoint = waypoints[waypointNumber];
        }
    }

    private void FixedUpdate()
    {
        if (!Patrolling)
        {
            if (touchingDirections.isGrounded && touchingDirections.isOnWall)
            {
                FlipDirection();
            }
            if (!healthManager.LockVelocity)
            {

                if (CanMove && touchingDirections.isGrounded)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                        rb.velocity.y);

                else
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);

            }
        }
        else
        {
            if (!healthManager.LockVelocity)
                Patrol();

            // Reverse Direction
            if (!healthManager.LockVelocity)
            {
                if (rb.velocity.x > 0)
                    transform.localScale = new Vector2(1, 1);
                else
                    transform.localScale = new Vector2(-1, 1);
            }
        }

    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;

        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        HasRangedTarget = rangedDetectionZone.detectedColliders.Count > 0;


        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;

    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
        {
            FlipDirection();
        }
    }

    private void Patrol()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * patrolSpeed;

        if (distance <= waypointReachedDistance)
        {
            waypointNumber++;

            if (waypointNumber >= waypoints.Count)
            {
                waypointNumber = 0;
            }

            nextWaypoint = waypoints[waypointNumber];
        }
    }
}
