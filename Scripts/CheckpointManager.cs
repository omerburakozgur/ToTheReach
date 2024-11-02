using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{

    NewPlayerController playerController;
    HealthManager playerHealthManager;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;
    public UIManager uiManagerReference;
    public AudioSystem audioManagerReference;
    public GameObject deathEffectPanel;


    static public Vector2 lastCheckpointLocation;
    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
    }

    public void RespawnPlayerOnCheckpoint()
    {

        GetReferences();
        //checkpointManager.RespawnPlayerOnCheckpoint();
        playerAnimator.SetTrigger("respawned");
        playerAnimator.SetBool(AnimationStrings.isAlive, true);

        uiManagerReference.lifePoolCount--;
        uiManagerReference.UpdateHeartsUI();

        // play screen effect and sound etc.
        playerController.transform.position = lastCheckpointLocation;
        playerHealthManager.Health = playerHealthManager.MaxHealth;
        playerHealthManager.IsAlive = true;
        uiManagerReference.UpdatePlayerHP();

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
        



        //deathEffectPanel.SetActive(true);
    }

    private void GetReferences()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
        playerHealthManager = playerController.GetComponent<HealthManager>();
        playerAnimator = playerController.GetComponentInChildren<Animator>();
        uiManagerReference = GameObject.Find("UI Manager").GetComponent<UIManager>();
        audioManagerReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        spriteRenderer = playerAnimator.GetComponent<SpriteRenderer>();
    }
}

