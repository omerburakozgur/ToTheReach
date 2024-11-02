using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public DoorManager doorReference;
    public Animator animator;
    public AudioSystem AudioSystemReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
            AudioSystemReference.PlaySound(AudioSystemReference.keyPickup, collision.transform.position, 2f);

            animator.SetTrigger(AnimationStrings.collectTrigger);
            // Play collect sound fx from animator
            doorReference.keyObtained = true;

        }
    }
}
