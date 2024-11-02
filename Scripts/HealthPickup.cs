using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healthRestore = 20;
    private Vector3 spinRotationSpeed = new Vector3(0, 30, 0);
    public Transform playerTransform;

    public AudioSystem AudioSystemReference;

    private void Awake()
    {
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player")) 
        {
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            HealthManager healthManager = collision.GetComponent<HealthManager>();

            if (healthManager)
            {
                bool wasHealed = healthManager.Heal(healthRestore);

                if (wasHealed)
                {

                    Destroy(gameObject);
                    AudioSystemReference.PlaySound(AudioSystemReference.potionPickup, playerTransform.position);
                }
            }
        }
        
    }

    private void Update()
    {
        // This is how you spin a 2d sprite, commented because looks pretty bad
        //transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
