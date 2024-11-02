using HeneGames.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] Characters;
    public Sprite[] characterImages;

    AudioSystem AudioSystemReference;
    public DialogueCharacter dialogueCharacter;

    // Start is called before the first frame update

    private void Awake()
    {
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
    }

    void Start()
    {
        DisableAllCharacters();
        SetActiveCharacter();
        SetSoundVolumes();
        SetDialogueIcon();
    }

    void SetActiveCharacter()
    {
        Characters[StaticVariables.selectedCharacter].SetActive(true);
    }

    void DisableAllCharacters()
    {
        // starting all the characters enabled at the start, otherwise gameobject.find references doesn't work

        foreach (var character in Characters)
        {
            character.SetActive(false);
        }
    }

    void SetSoundVolumes()
    {
        AudioSystemReference.introSource.volume = StaticVariables.musicVolume / 50;
        AudioSystemReference.musicLoopSource.volume = StaticVariables.musicVolume / 50;

        AudioSystemReference.ambientSource.volume = StaticVariables.effectVolume / 50;
        AudioSystemReference.playerSource.volume = StaticVariables.effectVolume / 50;
        AudioSystemReference.soundsSource.volume = StaticVariables.effectVolume / 50;
        AudioSystemReference.soundsSource.volume *= 3;
        AudioSystemReference.stingSource.volume = StaticVariables.effectVolume / 50;

    }

    void SetDialogueIcon()
    {
        dialogueCharacter.characterPhoto = characterImages[StaticVariables.selectedCharacter];

        switch (StaticVariables.selectedCharacter)
        {
            case 0:
                dialogueCharacter.characterName = "Leaf Ranger";
                break;
            case 1:
                dialogueCharacter.characterName = "Fire Knight";
                break;
            case 2:
                dialogueCharacter.characterName = "Wind Hashashin";
                break;
            case 3:
                dialogueCharacter.characterName = "Metal Bladekeeper";
                break;
            case 4:
                dialogueCharacter.characterName = "Water Priestess";
                break;
        }
    }

    

}
