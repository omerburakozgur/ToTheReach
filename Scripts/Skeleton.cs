using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(HealthManager))]


public class Skeleton : MonoBehaviour
{
    [Header("Movement")]

    public float walkAcceleration = 200f;
    public float maxSpeed = 200f;
    public float walkStopRate = 0.001f;
    public float moveOffset = 0;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    [Header("References")]

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone playerDetectionZone;
    public Transform playerTransform;
    public Slider floatingHPBarSlider;

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
    public bool _hasFollowingRange = false;

    public bool HasFollowingRange
    {
        get { return _hasFollowingRange; }
        private set
        {
            _hasFollowingRange = value;
            if (!Platforming)
            {
                if (_hasFollowingRange)
                    Patrolling = false;
                else
                    Patrolling = true;
            }
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
    public bool Platforming = true;

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
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        floatingHPBarSlider = GetComponentInChildren<Slider>();

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
        if (Platforming)
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
        else if (Patrolling)
        {

            if (CanMove && touchingDirections.isGrounded)
            {
                if (!healthManager.LockVelocity)
                    Patrol();

                if (rb.velocity.x > 0)
                {
                    transform.localScale = new Vector2(1, 1);
                    floatingHPBarSlider.transform.localScale = new Vector2(1, 1);
                }

                else
                {
                    transform.localScale = new Vector2(-1, 1);
                    floatingHPBarSlider.transform.localScale = new Vector2(-1, 1);
                }


            }

        }
        else if (HasFollowingRange)
        {

            if (CanMove && touchingDirections.isGrounded)
            {
                playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

                // Normal movement
                //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),rb.velocity.y);
                // if local scale > 0 decrease the x position by the offset value
                // if local scale < 0 decrease the x position by the offset value
                moveOffset = transform.localScale.x > 0 ? -moveOffset : Mathf.Abs(moveOffset);

                Vector2 target = new Vector2(playerTransform.position.x + moveOffset, playerTransform.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, maxSpeed * Time.deltaTime);
                rb.MovePosition(newPos);
                if (playerTransform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);

                }
            }
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }



    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
            floatingHPBarSlider.transform.localScale = new Vector2(-1, 1);

        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
            floatingHPBarSlider.transform.localScale = new Vector2(1, 1);


        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;

        HasTarget = attackZone.detectedColliders.Count > 0;
        HasFollowingRange = playerDetectionZone.detectedColliders.Count > 0;

    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded && Platforming)
        {
            FlipDirection();
        }
    }

    private void Patrol()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * patrolSpeed;
        UpdateDirection();

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

    private void UpdateDirection()
    {
        Vector3 localScale = transform.root.localScale;

        if (localScale.x > 0)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.y);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.y);
            }
        }
    }
}
