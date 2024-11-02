using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    // Animasyon kodlamasını buraya yapıyoruz, statik fonksiyonlara ekleyip movement üzerinden çağırıyoruz
    // Skilllerin ve attackların yapıldığı scriptte de bu scripti çağırıp animasyonları yine buradan oynatacağız

    private Animator animator;
    private NewPlayerController playerController;
    private NecromancerBoss necromancerBoss;

    private Collision coll;
    public SpriteRenderer sr;
    public TouchingDirections direction;
    public CameraShake cameraShakeReference;
    public AudioSystem AudioSystemReference;

    public GameObject ArrowRainObject;

    void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        playerController = GetComponentInParent<NewPlayerController>();
        sr = GetComponent<SpriteRenderer>();
        necromancerBoss = GetComponentInParent<NecromancerBoss>();
        AudioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

    }

    public void PlayCrossfadeAnimation(int animationState, float transitionTime = 0)
    {
        animator.CrossFade(animationState, transitionTime, 0);
    }

    public void isLanded()
    {
        animator.SetBool("isLanded", true);

    }

    public void isLandedFalse()
    {
        animator.SetBool("isLanded", false);

    }

    public void setIsSlidingFalse()
    {
        animator.SetBool(AnimationStrings.isSliding, false);

    }

    public void ReduceStaminaOnAttack()
    {
        playerController.stamina -= playerController.attackStaminaRequirement;

    }

    public void SpawnEnemiesAnimScript()
    {
        necromancerBoss.SpawnEnemies();
    }

    public void SetVerticalRangedAttackFalse()
    {
        animator.SetBool(AnimationStrings.hasRangedTargetVertical, false);
    }

    public void SetDoubleJumpFalse()
    {
        playerController.doubleJumpEnabled = false;
    }

    public void PlayCameraShake()
    {
        cameraShakeReference.StartShakeCamera();

    }

    public void DisableArowRain()
    {
        ArrowRainObject.SetActive(false);
    }

    public void EnableArrowRain()
    {
        ArrowRainObject.SetActive(true);
    }

    public void PlayFootstepAudio()
    {
        playerController = GetComponentInParent<NewPlayerController>();
        AudioSystemReference.PlayFootstepSFX(playerController.footstepStates);

    }

    public void PlayPlayerDeathSFX()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.playerDeathSFX);

    }

    public void PlayFireKnightSPAttack()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.FireSFXOne, 5f);

    }

    public void PlayFireSFX()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.FireSFXTwo, 5f);

    }

    public void PlayBladeThrow()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.BladeThrow, 5f);

    }

    public void PlayRandomWindSFX() 
    {
        AudioSystemReference.PlayWindSFX();
    }

    public void PlayIceExplosion()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.IceExplosionSFX);
    }

    public void PlayWaterSFXOne()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.WaterSplashSFXOne);
    }

    public void PlayWaterSFXTwo()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.WaterSplashSFXTwo);
    }

    public void PlayWaterSFXThree()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.WaterSplashSFXThree);
    }

    public void PlayBubbleSFX()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.BubbleSoundSFX);
    }

    public void PlayLeafRangerSPAttack()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.LRSPAttack);
    }

    public void PlayBossCloseAttackSFX()
    {
        AudioSystemReference.PlaySound(AudioSystemReference.bossCloseAttack);
    }

    public void PlayNormalSlideSFX()
    {
        AudioSystemReference.PlayFootstepSFX(0);
    }

}


