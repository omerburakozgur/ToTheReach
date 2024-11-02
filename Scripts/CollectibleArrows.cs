using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleArrows : MonoBehaviour
{
    public UIManager uiManagerReference;
    public AudioSystem AudioSystemReference;
    public int collectibleArrowCount = 1;
    public Transform playerTransform;
    public GameObject plusOneArrowTextPrefab;
    public Canvas gameCanvas;



    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        uiManagerReference = GameObject.Find("UI Manager").GetComponent<UIManager>();
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiManagerReference.arrowCount < uiManagerReference.arrowLimit)
            {
                PickedArrow();
                if (uiManagerReference.arrowCount + collectibleArrowCount > uiManagerReference.arrowLimit)
                {
                    uiManagerReference.arrowCount = uiManagerReference.arrowLimit;
                }
                else
                {
                    uiManagerReference.arrowCount += collectibleArrowCount;

                }
                AudioSystemReference.PlaySound(AudioSystemReference.arrowPickup, playerTransform.position, 5f);

                //play sound from audio system
                Destroy(gameObject);
            }

        }
    }

    public void PickedArrow()
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerTransform.transform.position);

        TMP_Text tmpText = Instantiate(plusOneArrowTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = ("+" + collectibleArrowCount);

        //tmpText.text = healthRestored.ToString();
    }
}
