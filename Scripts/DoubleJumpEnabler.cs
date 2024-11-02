using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpEnabler : MonoBehaviour
{
    Animator animator;
    public AudioSystem AudioSystemReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            NewPlayerController playerController = collision.GetComponent<NewPlayerController>();
            AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

            animator = GetComponent<Animator>();
            AudioSystemReference.PlaySound(AudioSystemReference.doubleJumpCollect, collision.transform.position);

            playerController.doubleJumpEnabled = true;
            animator.SetTrigger(AnimationStrings.collectTrigger);
        }
    }
}
