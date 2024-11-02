using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = true, playAfterDelay = true, playOnUpdate = true;

    //Delay
    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playOnEnter) 
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.transform.parent.position, volume);
        }
        timeSinceEntered = 0f;
        hasDelayedSoundPlayed = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timeSinceEntered += Time.deltaTime;

        if (playAfterDelay && !hasDelayedSoundPlayed)
        {

            if (timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.transform.parent.position, volume);
                hasDelayedSoundPlayed = true;
            }
            
        }

        if (playOnUpdate && timeSinceEntered > playDelay)
        {
            timeSinceEntered = 0;
            hasDelayedSoundPlayed = false;
        }

        if (playOnUpdate && !hasDelayedSoundPlayed)
        {
            //AudioSource.PlayClipAtPoint(soundToPlay, animator.transform.parent.position, volume);
            hasDelayedSoundPlayed = true;


        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.transform.parent.position, volume);
        }
    }
}
