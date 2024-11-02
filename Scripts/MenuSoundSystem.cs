using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSoundSystem : StaticInstance<MenuSoundSystem>
{
    AudioSource menuSoundSource;
    public AudioClip buttonClick;

    public static MenuSoundSystem menuSoundSystem;
    static MenuSoundSystem current;

    private void Awake()
    {
        menuSoundSource = GetComponent<AudioSource>();
        //If an AudioManager exists and it is not this...
        if (current != null && current != this)
        {
            //...destroy this. There can be only one AudioManager
            Destroy(gameObject);
            return;
        }

        //This is the current AudioManager and it should persist between scene loads
        current = this;

        DontDestroyOnLoad(transform.root.gameObject);


    }

    public void FixedUpdate()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("Plains"))
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClickSound()
    {
        menuSoundSource.PlayOneShot(buttonClick);
    }

    public void MenuPlayOneShotSound(AudioClip audioClip)
    {
        menuSoundSource.PlayOneShot(audioClip);

    }
}
