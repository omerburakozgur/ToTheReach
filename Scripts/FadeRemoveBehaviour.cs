using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 1f;
    private float timeElapsed = 0f;

    public float fadeDelay = 0.0f;
    private float fadeDelayElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;
    bool deathSoundPlayed = false;

    CheckpointManager checkpointManager;
    UIManager uiManager;
    CutsceneEvents deathPanel;
    AudioSystem audioSystemReference;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.transform.parent.gameObject;
        checkpointManager = GameObject.Find("Checkpoint Manager").GetComponent<CheckpointManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        audioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fadeDelay > fadeDelayElapsed)
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            string objTag = animator.transform.parent.tag;
            if (objTag.Equals("Player") && uiManager.lifePoolCount == 0 && !deathSoundPlayed)
            {
                AudioSystem.StopAllSoundEffects();
                audioSystemReference.PlaySound(audioSystemReference.deathMusic);
                deathSoundPlayed = true;
            }
            // check if it is alive
            // if it is not start the fading and particle effects 

            timeElapsed += Time.deltaTime;
            float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            if (timeElapsed > fadeTime)
            {
                Destroy(objToRemove);

                if (objTag.Equals("Player") && uiManager.lifePoolCount == 0)
                {
                    
                    SceneManager.LoadScene("MainMenu"); // change this to checkpoint or some other scene

                }

            }
        }

    }
}
