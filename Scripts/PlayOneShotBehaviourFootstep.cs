using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayOneShotBehaviourFootstep : StateMachineBehaviour
{
    public AudioClip[] soundsToPlay;
    public float volume = 1f;
    public bool playAfterDelay = true, playOnUpdate = true;

    //Delay
    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;
    private AudioSystem audioSystemReference;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timeSinceEntered += Time.deltaTime;

        if (playAfterDelay && !hasDelayedSoundPlayed)
        {
            int tempIndex = Random.Range(0, soundsToPlay.Length);
            if (timeSinceEntered > playDelay)
            {
                //AudioSource.PlayClipAtPoint(soundsToPlay[tempIndex], animator.transform.parent.position, volume);
                audioSystemReference.PlayFootstepSFX(0);
                Debug.Log("player footstep sfx");
                hasDelayedSoundPlayed = true;
            }
            
        }

        if (playOnUpdate && timeSinceEntered > playDelay)
        {
            timeSinceEntered = 0;
            hasDelayedSoundPlayed = false;
            //audioSystemReference.PlayFootstepSFX(0);

        }

        if (playOnUpdate && !hasDelayedSoundPlayed)
        {
            //AudioSource.PlayClipAtPoint(soundToPlay, animator.transform.parent.position, volume);
            //audioSystemReference.PlayFootstepSFX(0);

            hasDelayedSoundPlayed = true;


        }
    }
}
