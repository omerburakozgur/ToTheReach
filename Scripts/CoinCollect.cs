using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    Animator animator;
    public AudioSystem AudioSystemReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
            AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

            animator = GetComponent<Animator>();
            AudioSystemReference.PlaySound(AudioSystemReference.coinPickup, collision.transform.position);

            uiManager.coinCount++;
            animator.SetTrigger(AnimationStrings.collectTrigger);
        }
        
    }
}
