using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenuManager : MonoBehaviour
{
    HealthManager playerHealthManager;
    NewPlayerController playerController;
    UIManager uiManager;

    [Header("Blacksmith Upgrade Cost References")]
    public int bootsUpgradeCost;
    public int armorUpgradeCost;
    public int bowUpgradeCost;
    public int quiverUpgradeCost;
    public int arrowUpgradeCost;

    [Header("Blacksmith Button References")]
    public GameObject bootsButton;
    public GameObject helmetButton;
    public GameObject glovesButton;
    public GameObject bowButton;
    public GameObject quiverButton;
    public GameObject arrowButton;

    [Header("Blacksmith Panel References")]
    public Image bootsPanelImage;
    public Image helmetPanelImage;
    public Image glovesPanelImage;
    public Image bowPanelImage;
    public Image quiverPanelImage;
    public Image arrowPanelImage;

    [Header("Merchant Purchase Cost References")]
    public int arrowPurchaseCost;
    public int foodPurchaseCost;
    public int xpPurchaseCost;
    public int lifePoolPurchaseCost;
    public int drinkPurchaseCost;


    [Header("Merchant Panel References")]
    public Image arrowPurchasePanelImage;
    public Image foodPurchasePanelImage;
    public Image xpPurchasePanelImage;
    public Image lifePoolPurchasePanelImage;
    public Image drinkPurchasePanelImage;


    [Header("Merchant Button References")]
    public GameObject arrowPurchaseButton;
    public GameObject foodPurchaseButton;
    public GameObject xpPurchaseButton;
    public GameObject lifePoolPurchaseButton;
    public GameObject drinkPurchaseButton;


    private void Awake()
    {

        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        GetPlayerReferences();
    }

    public void HealInteraction(int heal)
    {
        playerHealthManager.Heal(heal);
    }

    private void OnEnable()
    {
        GetPlayerReferences();

    }

    private void OnDisable()
    {
        GetPlayerReferences();

    }
    public void ArrowPurchaseInteraction()
    {

        if (uiManager.coinCount > arrowPurchaseCost)
        {
            GetPlayerReferences();
            uiManager.arrowCount = uiManager.arrowLimit;
            uiManager.UpdateCoinCount(-arrowPurchaseCost);
            uiManager.UpdateArrowCount();
            arrowPurchaseButton.SetActive(false);
            arrowPurchasePanelImage.color = Color.green;
        }
    }

    public void FoodPurchaseInteraction()
    {
        if (uiManager.coinCount > foodPurchaseCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-foodPurchaseCost);
            playerHealthManager.Heal(200);
            foodPurchaseButton.SetActive(false);
            foodPurchasePanelImage.color = Color.green;
        }
    }

    public void XPPurchaseInteraction()
    {
        if (uiManager.coinCount > xpPurchaseCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-xpPurchaseCost);
            xpPurchaseButton.SetActive(false);
            xpPurchasePanelImage.color = Color.green;

            uiManager.UpdateXPCount(uiManager.xpPointsLimit+1);
            uiManager.UpdateXpUI();
            uiManager.OnLevelUp();
            uiManager.UpdatePlayerHP();
            uiManager.UpdateArrowCount();
        }
    }

    public void PlaySongInteraction()
    {
        //
    }

    public void BootsBuyInteraction()
    {

        if (uiManager.coinCount > bootsUpgradeCost)
        {
            GetPlayerReferences();

            uiManager.UpdateCoinCount(-armorUpgradeCost);
            // +50 stamina and +50% recovery rate
            playerController.maxStamina = 150;
            playerController.staminaRecoveryRate = 0.3f;
            uiManager.UpdatePlayerStamina();
            bootsButton.SetActive(false);
            bootsPanelImage.color = Color.green;
        }
    }

    public void HelmetBuyInteraction()
    {
        if (uiManager.coinCount > armorUpgradeCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-armorUpgradeCost);
            playerHealthManager.MaxHealth += 25;
            uiManager.UpdatePlayerHP();
            helmetButton.SetActive(false);
            helmetPanelImage.color = Color.green;
        }
    }

    public void GlovesBuyInteraction()
    {
        if (uiManager.coinCount > armorUpgradeCost)

        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-armorUpgradeCost);
            playerHealthManager.MaxHealth += 25;
            uiManager.UpdatePlayerHP();
            glovesButton.SetActive(false);
            glovesPanelImage.color = Color.green;
        }
    }

    public void BowUpgradeInteraction(int damageIncrease)
    {
        if (uiManager.coinCount > bowUpgradeCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-bowUpgradeCost);
            playerController.UpdateDamageValues(damageIncrease);
            bowButton.SetActive(false);
            bowPanelImage.color = Color.green;
        }
    }

    public void ArrowUpgradeInteraction(int damageIncrease)
    {
        if (uiManager.coinCount > arrowUpgradeCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-arrowUpgradeCost);
            playerController.UpdateDamageValues(damageIncrease);
            arrowButton.SetActive(false);
            arrowPanelImage.color = Color.green;
        }
    }

    public void QuiverBuyInteraction(int arrowCountIncrease)
    {
        if (uiManager.coinCount > quiverUpgradeCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-quiverUpgradeCost);
            uiManager.arrowLimit += arrowCountIncrease;
            uiManager.UpdateArrowCount();
            quiverButton.SetActive(false);
            quiverPanelImage.color = Color.green;
        }
    }

    public void DrinkBuyInteraction()
    {
        if (uiManager.coinCount > drinkPurchaseCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-drinkPurchaseCost);
            drinkPurchaseButton.SetActive(false);
            drinkPurchasePanelImage.color = Color.green;
        }
    }

    public void LifePoolBuyInteraction()
    {
        if (uiManager.coinCount > lifePoolPurchaseCost)
        {
            GetPlayerReferences();
            uiManager.UpdateCoinCount(-lifePoolPurchaseCost);
            lifePoolPurchaseButton.SetActive(false);
            lifePoolPurchasePanelImage.color = Color.green;
            uiManager.lifePoolCount++;
            uiManager.UpdateHeartsUI();
        }
    }

    private void GetPlayerReferences()
    {
        playerHealthManager = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
    }
}
