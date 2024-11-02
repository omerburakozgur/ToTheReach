using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]


public class NecromancerBoss : MonoBehaviour
{
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    HealthManager healthManager;

    [Header("Movement")]
    public float walkAcceleration = 200f;
    public float maxSpeed = 200f;
    public int moveOffset = 5;
    public float walkStopRate = 0.001f;
    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    [Header("Detection Zones")]
    public DetectionZone attackZone;
    public DetectionZone rangedDetectionZone;
    public DetectionZone cliffDetectionZone;

    [Header("Enemy Spawn")]
    public GameObject[] enemies;
    public Transform enemySpawnPositionOne;
    public Transform enemySpawnPositionTwo;
    public float enemySpawnCooldown = 20f;
    private GameObject spawnedEnemyOne;
    private GameObject spawnedEnemyTwo;
    private Transform playerTransform;


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
            //roll dice
            // select vertical or horizontal ranged attack, maybe add more attacks (diagonal or something)
            int roll = Random.Range(0, 2);
            //print($"roll {roll}");
            if (roll == 0)
            {
                animator.SetBool(AnimationStrings.hasRangedTargetVertical, value);
                animator.SetBool(AnimationStrings.hasRangedTarget, false);
            }
            else
            {
                animator.SetBool(AnimationStrings.hasRangedTarget, value);
                animator.SetBool(AnimationStrings.hasRangedTargetVertical, false);
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

    public float SpawnCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.spawnCooldown);
        }

        private set
        {
            animator.SetFloat(AnimationStrings.spawnCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        healthManager = GetComponent<HealthManager>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if (touchingDirections.isGrounded && touchingDirections.isOnWall)
        {
            FlipDirection();
        }
        if (!healthManager.LockVelocity)
        {
            if (CanMove && touchingDirections.isGrounded)
            {
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
        if (SpawnCooldown <= 0.5f)

        {
            animator.SetBool(AnimationStrings.spawnedEnemy, true);
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

        if (SpawnCooldown > 0)
            SpawnCooldown -= Time.deltaTime;

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

    public void SpawnEnemies()
    {
        int randomEnemy;
        if (spawnedEnemyOne == null)
        {
            randomEnemy = UnityEngine.Random.Range(0, enemies.Length - 1);
            spawnedEnemyOne = Instantiate(enemies[randomEnemy], enemySpawnPositionOne.transform.position, Quaternion.identity);
            //spawnedEnemyOne.GetComponent<Skeleton>().Patrolling = true;
            spawnedEnemyOne.GetComponent<Skeleton>().Platforming = false;


        }
        if (spawnedEnemyTwo == null)
        {
            randomEnemy = UnityEngine.Random.Range(0, enemies.Length - 1);
            spawnedEnemyTwo = Instantiate(enemies[randomEnemy], enemySpawnPositionTwo.transform.position, Quaternion.identity);
            //spawnedEnemyTwo.GetComponent<Skeleton>().Patrolling = true;
            spawnedEnemyTwo.GetComponent<Skeleton>().Platforming = false;



        }
        animator.SetFloat(AnimationStrings.spawnCooldown, enemySpawnCooldown);

    }

    public void SetBossMovement(bool moveBool)
    {
        animator.SetBool(AnimationStrings.canMove, moveBool);

    }

    public void SetNecromancerSpawnTimer()
    {
        animator.SetFloat(AnimationStrings.spawnCooldown, 10);
    }
}
