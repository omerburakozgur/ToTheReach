 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationManager : MonoBehaviour
{
    // Animasyon kodlamasýný buraya yapýyoruz, statik fonksiyonlara ekleyip movement üzerinden çaðýrýyoruz
    // Skilllerin ve attacklarýn yapýldýðý scriptte de bu scripti çaðýrýp animasyonlarý yine buradan oynatacaðýz

    private Animator animator;
    private NewPlayerController playerController;
    private Collision coll;
    [HideInInspector]
    public SpriteRenderer sr;

    public static readonly int Idle = Animator.StringToHash("idle");
    public static readonly int Run = Animator.StringToHash("run");
    public static readonly int Jump = Animator.StringToHash("jump_up");
    public static readonly int Fall = Animator.StringToHash("falling");
    public static readonly int Land = Animator.StringToHash("land");
    public static readonly int Slide = Animator.StringToHash("slide");
    public static readonly int TakeHit = Animator.StringToHash("take_hit");
    public static readonly int Defend = Animator.StringToHash("defend");
    public static readonly int Death = Animator.StringToHash("death");
    public static readonly int Attack1 = Animator.StringToHash("atk_1");
    public static readonly int Attack2 = Animator.StringToHash("atk_2");
    public static readonly int Attack3 = Animator.StringToHash("atk_3");
    public static readonly int AirAttack = Animator.StringToHash("air_atk");
    public static readonly int SpecialAttack = Animator.StringToHash("sp_atk");
    public static readonly int Projectile = Animator.StringToHash("arrow1");


    void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        playerController = GetComponentInParent<NewPlayerController>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayCrossfadeAnimation(int animationState, float transitionTime = 0)
    {
        animator.CrossFade(animationState, transitionTime, 0);
    }

    public void setIsSlidingFalse()
    {
        animator.SetBool(AnimationStrings.isSliding, false);

    }


}
