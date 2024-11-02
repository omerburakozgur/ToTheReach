using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [Header("Character Toggles")]
    public Toggle[] toggles;

    public Slider musicSlider;
    public Slider effectSlider;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI effectSliderText;
    MenuSoundSystem menuSoundManager;
    public GameObject managers;

    [Header("Scene Load References")]
    public GameObject characterPanel;
    public GameObject loadingSliderParent;
    public Slider slider;
    public TextMeshProUGUI progressText;


    private void Start()
    {
        menuSoundManager = GameObject.Find("MenuSoundManager").GetComponent<MenuSoundSystem>();

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("Options"))
        {
            SetSliderStartingValue();

        }

        managers = GameObject.Find("Managers");
        Destroy(managers);
    }

    public void LoadGame()
    {
        menuSoundManager.PlayButtonClickSound();
        SetCharacterForPlaythrough();
        StartCoroutine(LoadGameSceneAsync());

    }

    IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        operation.allowSceneActivation = true;
        characterPanel.SetActive(false);
        loadingSliderParent.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = (int)(progress * 100f) + "%";
            yield return null;

        }
    }

    public void LoadCredits()
    {
        menuSoundManager.PlayButtonClickSound();
        SceneManager.LoadScene("Credits");

    }

    public void LoadOptions()
    {
        menuSoundManager.PlayButtonClickSound();
        SceneManager.LoadScene("Options");

    }

    public void LoadHowTo()
    {
        menuSoundManager.PlayButtonClickSound();
        SceneManager.LoadScene("HowTo");

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        menuSoundManager.PlayButtonClickSound();

    }

    public void QuitGame()
    {
        menuSoundManager.PlayButtonClickSound();
        Application.Quit();
    }

    public void LoadCharacterSelect()
    {
        menuSoundManager.PlayButtonClickSound();
        SceneManager.LoadScene("CharacterSelect");

    }

    public void SetCharacterForPlaythrough()
    {
        menuSoundManager.PlayButtonClickSound();
        if (toggles[0].isOn)
        {
            StaticVariables.selectedCharacter = 0;
        }
        else if (toggles[1].isOn)
        {
            StaticVariables.selectedCharacter = 1;

        }
        else if (toggles[2].isOn)
        {
            StaticVariables.selectedCharacter = 2;

        }
        else if (toggles[3].isOn)
        {
            StaticVariables.selectedCharacter = 3;

        }
        else if (toggles[4].isOn)
        {
            StaticVariables.selectedCharacter = 4;

        }
    }

    public void SetCharacterIndex(int index)
    {
        StaticVariables.selectedCharacter = index;

    }

    public void UpdateMusicVolume()
    {
        StaticVariables.musicVolume = musicSlider.value;
        musicSliderText.text = ((int)(musicSlider.value * 100)).ToString();

    }

    public void UpdateEffectVolume()
    {
        StaticVariables.effectVolume = effectSlider.value;
        effectSliderText.text = ((int)(effectSlider.value * 100)).ToString();

    }

    void SetSliderStartingValue()
    {

        effectSlider.value = StaticVariables.effectVolume;
        effectSliderText.text = ((int)(effectSlider.value * 100)).ToString();

        musicSlider.value = StaticVariables.musicVolume;
        musicSliderText.text = ((int)(musicSlider.value * 100)).ToString();
    }




}
