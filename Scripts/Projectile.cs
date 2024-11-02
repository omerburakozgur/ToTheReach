using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator animator;

    public Vector2 projectileSpeed = new Vector2(3f, 3f);
    public Vector2 knockback = new Vector2(0, 0);
    public ProjectileLauncher launcher;

    public Transform collectibleArrowSpawnPosition;
    public GameObject horizontalCollectibleArrowPrefab;
    public GameObject verticalCollectibleArrowPrefab;
    public GameObject blackArrowPrefab;
    public bool enemyArrow = false;
    public Transform spawnerCharacterObject;
    public AudioSystem AudioSystemReference;
    public AudioClip projectileSound;
    public AudioClip projectileHitSound;
    public AudioClip projectileMissSound;

    private bool arrowSpawned = false;
    GameObject projectile;
    GameObject player;
    public int attackDamage = 10;
    int localScaleModifier;

    Rigidbody2D rb;

    private void Awake()
    {
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
        launcher = playerAnimator.gameObject.GetComponent<ProjectileLauncher>();
        //gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
        collectibleArrowSpawnPosition = gameObject.GetComponentInChildren<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (spawnerCharacterObject.root.tag.Equals("Player"))
        {
            localScaleModifier = playerAnimator.transform.root.localScale.x > 0 ? 1 : -1;
            attackDamage = player.GetComponent<NewPlayerController>().rangedAttackDamage;

        }
        else
            localScaleModifier = spawnerCharacterObject.root.localScale.x > 0 ? 1 : -1;

        if (launcher.fourtyFive)
        {

            projectileSpeed.y = 9f;
            rb.velocity = new Vector2(localScaleModifier * projectileSpeed.y, -projectileSpeed.y);

        }
        else
        {
            projectileSpeed.y = 0f;
            rb.velocity = new Vector2(projectileSpeed.x * transform.localScale.x, projectileSpeed.y);

        }

        // Set update method, set 5 seconds timer when Instantiating the projectile, destroy the projectile when the timer runs out,
        // add a ground check for projectile, if the projectile collides with ground layer get the collision position, destroy the projectile
        // Instantiate a collectible arrow prefab on the collision position for player to pick up
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit effect

        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            HealthManager healthManager = collision.GetComponent<HealthManager>();

            if (healthManager != null)
            {
                print(transform.root.name);
                print(transform.root.localScale.x);

                //transform.localScale.x
                Vector2 deliveredKnockback = localScaleModifier > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                bool gotHit = healthManager.Hit(attackDamage, deliveredKnockback);

                if (gotHit)
                    print($"{collision.name} got hit for {attackDamage} damage!");

                transform.localScale = new Vector2(1, 1);
                animator.SetTrigger(AnimationStrings.projectileHitTrigger);
                AudioSystemReference.PlaySound(AudioSystemReference.arrowHitSound);

                rb.velocity = Vector2.zero;
                //Destroy(gameObject);

            }
        }
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Player"))
        {

            AudioSystemReference.PlaySound(AudioSystemReference.arrowMissSound);

            if (enemyArrow)
            {
                if (!arrowSpawned)
                    projectile = Instantiate(blackArrowPrefab, collectibleArrowSpawnPosition.position, horizontalCollectibleArrowPrefab.transform.rotation);
                arrowSpawned = true;

                if (localScaleModifier < 0)
                {
                    projectile.transform.localScale = new Vector2(-1, 1);
                    //sr.flipX = true;
                }
                //spawn horizontal arrow

            }
            // check if it has y velocity
            if (rb.velocity.y == 0 && !enemyArrow)
            {

                // quaternion.identity
                if (!arrowSpawned)
                    projectile = Instantiate(horizontalCollectibleArrowPrefab, collectibleArrowSpawnPosition.position, horizontalCollectibleArrowPrefab.transform.rotation);
                arrowSpawned = true;

                if (localScaleModifier < 0)
                {
                    projectile.transform.localScale = new Vector2(-1, 1);
                    //sr.flipX = true;
                }
                //spawn horizontal arrow
            }

            // check if it has y velocity
            if (rb.velocity.y != 0 && !enemyArrow)
            {
                // quaternion.identity
                if (!arrowSpawned)
                    projectile = Instantiate(verticalCollectibleArrowPrefab, collectibleArrowSpawnPosition.position, verticalCollectibleArrowPrefab.transform.rotation);
                arrowSpawned = true;

                if (localScaleModifier < 0)
                {
                    projectile.transform.localScale = new Vector2(-1, 1);
                    //sr.flipX = true;
                }
                //spawn horizontal arrow
            }

            launcher.fourtyFive = false;
            Destroy(gameObject);
            // arrow hit sound fx
            //Destroy(gameObject);
        }


    }
}
