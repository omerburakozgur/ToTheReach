using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public HealthManager playerHealthManagerReference;
    public NewPlayerController playerController;
    public Image hpImage;
    public Image staminaImage;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI staminaText;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthManagerReference = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerHP()
    {
        print("hp" + playerHealthManagerReference.Health);
        print("maxhp" + playerHealthManagerReference.MaxHealth);
        float hpFillAmount = (float) playerHealthManagerReference.Health / (float) playerHealthManagerReference.MaxHealth;
        hpImage.fillAmount = hpFillAmount;
        print(hpFillAmount);
        hpText.text = ($"{playerHealthManagerReference.Health}/{playerHealthManagerReference.MaxHealth}");

    }

    public void UpdatePlayerStamina()
    {
        print("stamina" + playerController.stamina);
        print("maxStamina" + playerController.maxStamina);
        float staminaFillAmount = playerController.stamina / playerController.maxStamina;
        staminaImage.fillAmount = staminaFillAmount;
        print("staminaFillAmount" + staminaFillAmount);
        staminaText.text = ($"{playerController.stamina} / {playerController.maxStamina}");

    }
}
