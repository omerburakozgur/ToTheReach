using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    HealthManager healthManager;
    TouchingDirections touchingDirections;
    public float flightSpeed = 3f;
    public float waypointReachedDistance = 0.1f;

    public List<Transform> waypoints;
    public DetectionZone AttackDetectionZone;

    Transform nextWaypoint;
    int waypointNumber = 0;

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

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<HealthManager>();
        touchingDirections = GetComponent<TouchingDirections>();

    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNumber];
    }

    private void OnEnable()
    {
        healthManager.healthManagerDeath.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        healthManager.healthManagerDeath.RemoveListener(OnDeath);

    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = AttackDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (healthManager.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        if (touchingDirections.isGrounded && !animator.GetBool(AnimationStrings.isGrounded))
        {
            animator.SetBool(AnimationStrings.isGrounded, true);
        }
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
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
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);

        }

        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector2(1, 1);

            }
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}
