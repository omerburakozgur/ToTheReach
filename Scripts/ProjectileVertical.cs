using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVertical : MonoBehaviour
{
    public Animator animator;
    public Vector2 projectileSpeed = new Vector2(3f, 3f);
    public Vector2 knockback = new Vector2(0, 0);
    public int attackDamage = 10;
    int localScaleModifier;
    EnemyProjectileLauncher enemyLauncherScript;
    float yVelocity;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyLauncherScript = GameObject.Find("Necromancer").GetComponentInChildren<EnemyProjectileLauncher>();


    }

    // Start is called before the first frame update
    void Start()
    {
        if (enemyLauncherScript.upwards)
            yVelocity = Mathf.Abs(projectileSpeed.y);
        else
            yVelocity = projectileSpeed.y;

        rb.velocity = new Vector2(projectileSpeed.x * transform.localScale.x, yVelocity);

        // Set update method, set 5 seconds timer when Instantiating the projectile, destroy the projectile when the timer runs out,
        // add a ground check for projectile, if the projectile collides with ground layer get the collision position, destroy the projectile
        // Instantiate a collectible arrow prefab on the collision position for player to pick up
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit effect
        animator.SetTrigger(AnimationStrings.projectileHitTrigger);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (collision.CompareTag("Player"))
        {
            HealthManager healthManager = collision.GetComponent<HealthManager>();

            if (healthManager != null)
            {
                Vector2 deliveredKnockback = localScaleModifier > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                bool gotHit = healthManager.Hit(attackDamage, deliveredKnockback);

                if (gotHit)
                    print($"{collision.name} got hit for {attackDamage} damage!");

            }
        }
        // arrow hit sound fx
        //Destroy(gameObject);
    }



}
