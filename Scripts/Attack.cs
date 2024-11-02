using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public int attackDamage = 10;
    public Vector2 knockback;
    int localScaleModifier;
    public bool isTrap;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthManager healthManager = collision.GetComponent<HealthManager>();

        if (healthManager != null)
        {
            if (collision.transform.position.x < transform.position.x)
                localScaleModifier = -1;
            else
            {
                localScaleModifier = 1;

            }

            //Vector2 deliveredKnockback = collision.transform.root.localScale.x > 0 ? new Vector2(-knockback.x, knockback.y) : knockback;
            Vector2 deliveredKnockback = new Vector2(knockback.x * localScaleModifier, knockback.y);

            print("local scale modifier: "+localScaleModifier);

            //if (collision.tag.Equals("Player"))
            //{
            //    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            //    rb.velocity = deliveredKnockback;
            //}
            bool gotHit = healthManager.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
                print($"{collision.name} got hit for {attackDamage} damage!");

        }
    }
}
