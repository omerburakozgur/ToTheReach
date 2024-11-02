// This script is a Manager that controls all of the audio for the project. All audio
// commands are issued through the static methods of this class. Additionally, this 
// class creates AudioSource "channels" at runtime and manages them

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static AudioManager current;

    [Header("Ambient Audio")]
    public AudioClip[] ambientClips;	//The background ambient sound
    public AudioClip[] musicClips;      //The background music

    [Header("Minigame Generic Audio")]
    public AudioClip[] genericSuccessSound;  // Generic Success +
    public AudioClip genericFailSound;     // Generic Fail +

    [Header("Letter Puzzle Minigame Audio")]
    public AudioClip[] letterPieceSound;     // Paper Sound +

    [Header("Codelock Minigame Audio")]
    public AudioClip codelockClickSound;   // Digital Beep Sound +
    public AudioClip codelockGameWonSound; // Digital Success, level up

    [Header("Lockpicking Minigame Audio")]
    public AudioClip lockpickingSound;     // Metal pick sound 
    public AudioClip lockpickFailedSound;  // Metal click sound 1 +
    public AudioClip lockpickWonSound;     // Metal click sound 2 +

    [Header("Lock Breaking Minigame Audio")]
    public AudioClip[] lockbreakHitSounds;   // Metal Hit Sound +
    public AudioClip lockbreakWonSound;      // Metal Hit Sound 2 

    [Header("Dna Minigames Audio")]
    public AudioClip[] dnaClickSounds;   // Click Sound 1 +

    [Header("Wire Connect Minigame Audio")]
    public AudioClip[] wireConnectGrabSounds; // Electric 1 +
    public AudioClip[] wireConnectSuccessSounds; // Electric 2 +
    public AudioClip wireConnectWonSound;     // Generic Success sound 

    [Header("Simon Says Minigame Audio")]
    public AudioClip[] simonSaysClickSounds;  // Click Sound 1 +
    public AudioClip simonSaysWonSound;    // Generic Success
    public AudioClip simonSaysFailSound;   // X beep sound (Gerek olmayabilir)

    [Header("Fingerprint Minigame Audio")]
    public AudioClip fingerprintClickSound;   // Digital Click
    public AudioClip fingerprintSuccessSound; // Generic Success

    [Header("Walk Audio")]
    public AudioClip[] WoodWalkStepClips;	 //The footstep sound effects
    public AudioClip[] OutsideWalkStepClips; //The footstep sound effects
    public AudioClip[] InsideWalkStepClips;  //The footstep sound effects
    public AudioClip[] GrassWalkStepClips;   //The footstep sound effects

    [Header("Mixer Groups")]
    public AudioMixerGroup ambientGroup;  //The ambient mixer group
    public AudioMixerGroup musicGroup;    //The music mixer group
    public AudioMixerGroup stingGroup;    //The sting mixer group
    public AudioMixerGroup playerGroup;   //The player mixer group
    public AudioMixerGroup minigameGroup; //The voice mixer group

    AudioSource ambientSource;			//Reference to the generated ambient Audio Source
    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource stingSource;            //Reference to the generated sting Audio Source
    AudioSource playerSource;           //Reference to the generated player Audio Source
    AudioSource minigameSource;         //Reference to the generated minigame Audio Source



    void Awake()
    {
        //If an AudioManager exists and it is not this...
        if (current != null && current != this)
        {
            //...destroy this. There can be only one AudioManager
            Destroy(gameObject);
            return;
        }

        //This is the current AudioManager and it should persist between scene loads
        current = this;
        DontDestroyOnLoad(gameObject);

        //Generate the Audio Source "channels" for our game's audio
        ambientSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        musicSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        stingSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        playerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        minigameSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        //Assign each audio source to its respective mixer group so that it is
        //routed and controlled by the audio mixer
        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        stingSource.outputAudioMixerGroup = stingGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        minigameSource.outputAudioMixerGroup = minigameGroup;

        //Being playing the game audio
        StartLevelAudio();
    }

    public void playSound(AudioClip clip)
    {
        minigameSource.PlayOneShot(clip);
    }

    void StartLevelAudio()
    {
        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClips[0];
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClips[0];
        current.musicSource.loop = true;
        current.musicSource.Play();
    }

    public static void PlayFootstepAudio(int number)
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.playerSource.isPlaying)
            return;

        //Pick a random footstep sound
        int index = Random.Range(0, current.OutsideWalkStepClips.Length);

        switch (number)
        {
            case 0:
                //play wood walk sound effect
                current.playerSource.clip = current.WoodWalkStepClips[index];
                break;
            case 1:
                //play outside walk sound effect
                current.playerSource.clip = current.OutsideWalkStepClips[index];
                break;
            case 2:
                //play inside walk sound effect
                current.playerSource.clip = current.InsideWalkStepClips[index];
                break;
            case 3:
                //play inside walk sound effect
                current.playerSource.clip = current.GrassWalkStepClips[index];
                break;
        }

        //Set the footstep clip and tell the source to play

        current.playerSource.Play();
    }

    public void PlayPaperAudio()
    {

        //Pick a random paper sound
        int index = Random.Range(0, current.letterPieceSound.Length);
        //Play the randomly selected audio clip
        current.minigameSource.PlayOneShot(letterPieceSound[index]);
    }

    public void PlayDnaClickSound()
    {

        //Pick a random paper sound
        int index = Random.Range(0, current.dnaClickSounds.Length);
        //Play the randomly selected audio clip
        current.minigameSource.PlayOneShot(dnaClickSounds[index], 0.2f);
    }

    public void PlayCodelockClick()
    {
        minigameSource.PlayOneShot(codelockClickSound, 0.05f); // Default 1f
        print("Codelock Click");
    }

    public void PlayGenericFail()
    {
        minigameSource.PlayDelayed(10f);
        minigameSource.PlayOneShot(genericFailSound, 0.2f); // Default 1f
        print("Generic Fail");
    }

    public void PlayGenericSuccess(int index)
    {
        minigameSource.PlayDelayed(10f);
        minigameSource.PlayOneShot(genericSuccessSound[index], 0.2f); // Default 1f
        print("Generic Success");
    }

    public void PlayCodelockGameWon()
    {
        minigameSource.PlayOneShot(codelockGameWonSound, 0.2f); // Default 1f
        print("Codelock Open");
    }

    public void PlaySimonSaysClickTone(int index)
    {
        minigameSource.PlayOneShot(simonSaysClickSounds[index], 0.05f); // Default 1f

    }

    public void PlayWireConnectClick()
    {
        int index = Random.Range(0, current.wireConnectGrabSounds.Length);

        minigameSource.PlayOneShot(wireConnectGrabSounds[index], 0.2f); // Default 1f
        print("WireConnectClick");
    }

    public void PlayWireConnectSuccess()
    {
        int index = Random.Range(0, current.wireConnectGrabSounds.Length);

        //minigameSource.PlayOneShot(wireConnectSuccessSounds[index], 0.2f); // Default 1f // no sound added rn
        minigameSource.PlayOneShot(wireConnectGrabSounds[index], 0.2f); // Default 1f

        print("wireConnectSuccess");
    }

    public void PlayWireConnectWon()
    {
        minigameSource.PlayOneShot(wireConnectWonSound, 0.2f); // Default 1f
        print("wireConnectWon");
    }


}
