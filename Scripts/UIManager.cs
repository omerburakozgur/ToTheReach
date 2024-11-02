using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static bool GameisPaused = false;

    [Header("Arrow References")]
    public TextMeshProUGUI arrowCountText;
    public int arrowLimit = 5;
    public int _arrowCount = 5;
    public Image arrowSlider;

    [SerializeField]
    public int arrowCount
    {
        get { return _arrowCount; }
        set
        {
            _arrowCount = value;
            UpdateArrowCount();
        }
    }

    [Header("Prefabs")]
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject xpTextPrefab;

    [Header("UI References")]
    public Canvas gameCanvas;
    public GameObject pauseMenuUI;
    public TextMeshProUGUI questObjectiveText;

    [Header("HP References")]
    public TextMeshProUGUI hpText;
    public Image hpSlider;

    [Header("Stamina UI References")]
    public TextMeshProUGUI staminaText;
    public Image staminaSlider;

    [Header("Script References")]
    public HealthManager playerHealthManagerReference;
    public NewPlayerController playerController;
    public AudioSystem audioSystemReference;

    [Header("Economy Variables")]
    public int _coinCount;
    public int coinCount
    {
        get
        {
            return _coinCount;
        }
        set
        {
            _coinCount = value;
            UpdateCoinCount();
        }

    }
    public TextMeshProUGUI coinCountText;

    [Header("XP References")]
    public int _xpPoints;
    public int xpPoints
    {
        get { return _xpPoints; }
        set
        {
            _xpPoints = value;
            UpdateXpUI();
        }
    }

    public int xpPointsLimit;
    public int levelCount;
    public TextMeshProUGUI xpLevelText;
    public TextMeshProUGUI xpPointText;
    public Image xpSlider;

    [Header("Life Pool References")]
    public int lifePoolCount = 3;
    public GameObject heartOne;
    public GameObject heartTwo;
    public GameObject heartThree;
    public GameObject heartFour;
    public GameObject DeathPanel;

    [Header("Sound Slider References")]
    public Slider musicSlider;
    public Slider effectSlider;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI effectSliderText;

    private void Start()
    {
        SetSliderStartingValue();
    }

    private void Awake()
    {
        gameCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
        //Pause();
        Resume();
        UpdateArrowCount();
        playerHealthManagerReference = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
        audioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = "-" + damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = "+" + healthRestored.ToString();
    }

    public void UpdatePlayerHP()
    {

        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
        print("hp" + playerHealthManagerReference.Health);
        print("maxhp" + playerHealthManagerReference.MaxHealth);
        float hpFillAmount = (float)playerHealthManagerReference.Health / (float)playerHealthManagerReference.MaxHealth;
        hpSlider.fillAmount = hpFillAmount;
        print(hpFillAmount);
        hpText.text = ($"{playerHealthManagerReference.Health}/{playerHealthManagerReference.MaxHealth}");
    }

    public void UpdatePlayerStamina()
    {
        playerHealthManagerReference = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();

        //print("stamina" + playerController.stamina);
        //print("maxStamina" + playerController.maxStamina);
        int rounded = (int)Math.Round(playerController.stamina, 0);
        staminaSlider.fillAmount = rounded / playerController.maxStamina;
        staminaText.text = ($"{rounded} / {playerController.maxStamina}");

    }

    public void PotionPickupUpdate()
    {
        //print("stamina" + playerController.stamina);
        //print("maxStamina" + playerController.maxStamina);
        int rounded = (int)Math.Round(playerController.stamina, 0);
        staminaSlider.fillAmount = rounded / playerController.maxStamina;
        staminaText.text = ($"{rounded} / {playerController.maxStamina}");

    }

    public void UpdateArrowCount()
    {
        arrowCountText.text = $"{_arrowCount}/{arrowLimit}";
        float arrowFillAmount = (float)_arrowCount / (float)arrowLimit;
        arrowSlider.fillAmount = arrowFillAmount;

    }

    private void UpdateCoinCount()
    {
        coinCountText.text = _coinCount.ToString();
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateCoinCount(int coin)
    {
        _coinCount += coin;
        UpdateCoinCount();
    }

    public void UpdateXPCount(int xp)
    {
        xpPoints += xp;
        // instanstiate +xp text
    }

    public void UpdateXpUI()
    {
        if (xpPoints >= xpPointsLimit)
        {
            xpPoints -= xpPointsLimit;
            xpPointText.text = $"XP {xpPoints}/{xpPointsLimit}";
            levelCount++;
            xpLevelText.text = levelCount.ToString();
            OnLevelUp();
        }
        else
        {
            xpPointText.text = $"XP {xpPoints}/{xpPointsLimit}";
        }

        float xpFillAmount = (float)xpPoints / (float)xpPointsLimit;
        xpSlider.fillAmount = xpFillAmount;
        // instanstiate blue xp text

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerController.transform.position);

    }

    public void OnLevelUp()
    {
        // Add damage, hp, stamina
        // instanstiate level bonuses level up text
        playerController.UpdateDamageValues(2);
        playerController.maxStamina += 5;
        playerHealthManagerReference.MaxHealth += 5;
        arrowLimit += 1;
        UpdateArrowCount();
        UpdatePlayerHP();
        UpdatePlayerStamina();

    }

    public void UpdateQuestObjectiveText(string text)
    {

        questObjectiveText.text = text;

    }

    public void UpdateHeartsUI()
    {
        switch (lifePoolCount)
        {
            case 0:
                heartOne.SetActive(false);
                heartTwo.SetActive(false);
                heartThree.SetActive(false);
                heartFour.SetActive(false);
                break;
            case 1:
                heartOne.SetActive(true);
                heartTwo.SetActive(false);
                heartThree.SetActive(false);
                heartFour.SetActive(false);
                break;
            case 2:
                heartOne.SetActive(true);
                heartTwo.SetActive(true);
                heartThree.SetActive(false);
                heartFour.SetActive(false);
                break;
            case 3:
                heartOne.SetActive(true);
                heartTwo.SetActive(true);
                heartThree.SetActive(true);
                heartFour.SetActive(false);
                break;
            case 4:
                heartOne.SetActive(true);
                heartTwo.SetActive(true);
                heartThree.SetActive(true);
                heartFour.SetActive(true);
                break;
        }
    }

    public void EnableDeathPanel()
    {
        DeathPanel.SetActive(true);

    }

    public void UpdateMusicVolume()
    {
        StaticVariables.musicVolume = musicSlider.value;
        musicSliderText.text = ((int)(musicSlider.value * 100)).ToString();
        SetSoundVolumes();
    }

    public void UpdateEffectVolume()
    {
        StaticVariables.effectVolume = effectSlider.value;
        effectSliderText.text = ((int)(effectSlider.value * 100)).ToString();
        SetSoundVolumes();
    }

    void SetSliderStartingValue()
    {

        effectSlider.value = StaticVariables.effectVolume;
        effectSliderText.text = ((int)(effectSlider.value * 100)).ToString();

        musicSlider.value = StaticVariables.musicVolume;
        musicSliderText.text = ((int)(musicSlider.value * 100)).ToString();
    }

    void SetSoundVolumes()
    {
        audioSystemReference.introSource.volume = StaticVariables.musicVolume / 50;
        audioSystemReference.musicLoopSource.volume = StaticVariables.musicVolume / 50;

        audioSystemReference.ambientSource.volume = StaticVariables.effectVolume / 50;
        audioSystemReference.playerSource.volume = StaticVariables.effectVolume / 50;
        audioSystemReference.soundsSource.volume = StaticVariables.effectVolume / 50;
        audioSystemReference.soundsSource.volume *= 3;
        audioSystemReference.stingSource.volume = StaticVariables.effectVolume / 50;

        foreach (var audioSource in audioSystemReference.ambientAudioSources)
        {
            audioSource.volume = StaticVariables.effectVolume / 50;
        }

    }
}
