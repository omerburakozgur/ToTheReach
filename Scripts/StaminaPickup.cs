using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class StaminaPickup : MonoBehaviour
{

    public int staminaRestore = 20;
    private Vector3 spinRotationSpeed = new Vector3(0, 30, 0);
    public Transform playerTransform;
    public GameObject staminaRestoredTextPrefab;
    public Canvas gameCanvas;
    public UIManager uiManagerReference;
    public AudioSystem AudioSystemReference;

    private void Awake()
    {
        gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
        uiManagerReference = GameObject.Find("UI Manager").GetComponent<UIManager>();
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }

    private void OnEnable()
    {
        CharacterEvents.staminaPotionPickedUp += pickedPotionUp;



    }

    private void OnDisable()
    {
        CharacterEvents.staminaPotionPickedUp -= pickedPotionUp;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NewPlayerController playerController = collision.GetComponent<NewPlayerController>();

        if (playerController)
        {
            print($"Stamina Restore {staminaRestore}");
            bool staminaRestored = playerController.RestoreStamina(staminaRestore);

            if (staminaRestored)
            {
                Destroy(gameObject);
                AudioSystemReference.PlaySound(AudioSystemReference.potionPickup, playerTransform.position, 5f);   
            }
        }
    }

    private void pickedPotionUp(int staminaRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerTransform.transform.position);

        TMP_Text tmpText = Instantiate(staminaRestoredTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = "+" + staminaRestored.ToString();
    }

    private void Update()
    {
        // This is how you spin a 2d sprite, commented because looks pretty bad
        //transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
