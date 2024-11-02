using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem>
{

    public static AudioSystem AudioSystemInstance;
    static AudioSystem current;

    // All sound effects should be in here to be able manage all them without navigating different scripts

    [Header("Audio Sources")]
    public AudioSource ambientSource;			//Reference to the generated ambient Audio Source
    public AudioSource ambientSourceTwo;			//Reference to the generated ambient Audio Source
    public AudioSource musicLoopSource;            //Reference to the generated music Audio Source
    public AudioSource introSource;            //Reference to the generated music Audio Source
    public AudioSource soundsSource;           //Reference to the generated music Audio Source
    public AudioSource stingSource;            //Reference to the generated sting Audio Source
    public AudioSource playerSource;           //Reference to the generated player Audio Source

    [Header("Ambient Audio Sources")]
    public AudioSource[] ambientAudioSources; 

    [Header("Background Ambient Audio")]
    public AudioClip natureAmbient;
    public AudioClip dungeonAmbient;
    public AudioClip thunderAmbient;
    public AudioClip castleAmbient;
    public AudioClip interiorAmbient;
    public AudioClip caveAmbient;

    [Header("Spesific Ambient Audio")]
    public AudioClip windAmbient;	//The background ambient sound
    public AudioClip windAmbientTwo;	//The background ambient sound
    public AudioClip windAmbientForest;	//The background ambient sound
    public AudioClip lavaAmbient;

    [Header("Loop Music Audio")]
    public AudioClip[] musicClips;      //The background music
    public AudioClip[] musicClipIntros;      //The background music
    // bossfight start music

    [Header("Generic Audio")]
    public AudioClip[] genericSuccessSound; // Generic Success +
    public AudioClip genericFailSound;      // Generic Fail +
    public AudioClip coinPickup;
    public AudioClip itemPurchase;
    public AudioClip potionPickup;
    public AudioClip XPPickup;
    public AudioClip arrowPickup;
    public AudioClip doorClick;
    public AudioClip doorOpen;
    public AudioClip keyPickup;
    public AudioClip deathMusic;
    public AudioClip checkpointEnter;
    public AudioClip doubleJumpCollect;
    public AudioClip playerJump;
    public AudioClip playerLand;
    public AudioClip lavaSound;
    public AudioClip dialoguePopup;
    public AudioClip playerDeathSFX;
    public AudioClip[] castleCollapse;

    [Header("Leaf Ranger Audio")]
    public AudioClip LRSPAttack;

    [Header("Fire Knight & Metal Bladekeeper Audio")]
    public AudioClip FireSFXOne;
    public AudioClip FireSFXTwo;
    public AudioClip BladeThrow;

    [Header("Wind Hashashin Audio")]
    public AudioClip[] WindSFXs;

    [Header("Water Priestess Audio")]
    public AudioClip IceExplosionSFX;
    public AudioClip BubbleSoundSFX;
    public AudioClip WaterSplashSFXOne;
    public AudioClip WaterSplashSFXTwo;
    public AudioClip WaterSplashSFXThree;

    [Header("Enemy Audio")]
    public AudioClip skeletonMove;
    public AudioClip[] enemyPlayerDetectionSoundHuman;
    public AudioClip enemyPlayerDetectionSoundMonster;

    [Header("Arrow Audios")]
    public AudioClip arrowHitSound;
    public AudioClip arrowMissSound;
    public AudioClip bossProjectileMissSound;


    //player death sfx, add it to the animation
    //player jump sfx, add it to the death function
    //equipment purchase, add it to the button event
    //arrow/blade hit, miss sounds, add it to the projectile script
    //town center bell sound, add it to an object, enable it with a trigger

    [Header("Boss Spesific Audio")]
    public AudioClip bossProjectileStart; //add to projectile
    public AudioClip bossProjectileHit; //add to projectile
    public AudioClip bossCloseAttack; //add to projectile
    public AudioClip bossDeathSFX;
    public AudioClip bossfightCutsceneEntry;
    public AudioClip bossfightSuccess;

    [Header("Walk Audio")]
    public AudioClip[] GrassWalkStepClips;	 //The footstep sound effects
    public AudioClip[] DungeonWalkStepClips; //The footstep sound effects
    public AudioClip[] CastleWalkStepClips;  //The footstep sound effects
    public AudioClip[] SnowWalkStepClips;   //The footstep sound effects

    [Header("Mixer Groups")]
    public AudioMixerGroup ambientGroup;  //The ambient mixer group
    public AudioMixerGroup musicGroup;    //The music mixer group
    public AudioMixerGroup soundsGroup;   //The music mixer group
    public AudioMixerGroup stingGroup;    //The sting mixer group
    public AudioMixerGroup playerGroup;   //The player mixer group
    public AudioMixerGroup minigameGroup; //The voice mixer group

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

        DontDestroyOnLoad(transform.root.gameObject);

        //Being playing the game audio
    }

    void Start()
    {
        PlayLoopMusic(0);
    }

    public void PlayLoopMusic(int index)
    {
        introSource.clip = musicClipIntros[index];
        introSource.PlayDelayed(2f);
        musicLoopSource.clip = musicClips[index];
        musicLoopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    public void PlaySound(AudioClip clip)
    {
        soundsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicLoopSource.clip = clip;
        musicLoopSource.Play();

    }

    public void PlayAmbient(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1)
    {
        soundsSource.PlayOneShot(clip, vol);
    }

    public static void StopAllSoundEffects()
    {
        current.ambientSource.Stop();
        current.musicLoopSource.Stop();
        current.introSource.Stop();
        current.soundsSource.Stop();
        current.stingSource.Stop();
        current.playerSource.Stop();
    }

    public void PlayBossfightEntrySound()
    {
        PlaySound(bossfightCutsceneEntry, 5f);
        bossfightCutsceneEntry = null;
    }

    public void PlayBuySound()
    {
        PlaySound(itemPurchase, 5f);
    }

    public void PlayFootstepSFX(int index)
    {
        int tempIndex = Random.Range(0, GrassWalkStepClips.Length);
        switch (index)
        {
            case 0: //Grass
                playerSource.clip = GrassWalkStepClips[tempIndex];
                break;
            case 1: //Dungeon
                playerSource.clip = DungeonWalkStepClips[tempIndex];
                break;
            case 2: //Stone
                playerSource.clip = CastleWalkStepClips[tempIndex];
                break;
            case 3: //Snow
                playerSource.clip = SnowWalkStepClips[tempIndex];
                break;
        }

        //playerSource.clip = musicClipIntros[index];
        playerSource.PlayOneShot(playerSource.clip);
    }

    public void PlayWindSFX()
    {
        int tempIndex = Random.Range(0, WindSFXs.Length);

        //playerSource.clip = musicClipIntros[index];
        playerSource.PlayOneShot(WindSFXs[tempIndex], 5f);
    }

    public void StopTheLoopMusic()
    {
        musicLoopSource.Stop();

    }

}