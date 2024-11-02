using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XpCollect : MonoBehaviour
{
    public int XPIncrease = 5;
    public GameObject xpCollectTextPrefab;
    public AudioSystem AudioSystemReference;
    public Canvas gameCanvas;
    Animator animator;
    SpriteRenderer sr;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
            AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
            AudioSystemReference.PlaySound(AudioSystemReference.XPPickup, collision.transform.position, 2f);

            uiManager.xpPoints += XPIncrease;

            gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(collision.transform.position);
            TMP_Text tmpText = Instantiate(xpCollectTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
            tmpText.text = ("+" + XPIncrease + " XP");

            animator = GetComponent<Animator>();
            animator.SetTrigger(AnimationStrings.collectTrigger);

            sr = GetComponent<SpriteRenderer>();
            sr.color = Color.blue;
        }

    }
}
