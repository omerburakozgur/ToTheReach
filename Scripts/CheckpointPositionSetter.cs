using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointPositionSetter : MonoBehaviour
{
    Animator animator;
    public GameObject CheckpointText;
    public Canvas gameCanvas;
    private AudioSystem audioSystemReference;
    //Transform playerTransform;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //&& CheckpointManager.lastCheckpointLocation.x < transform.position.x
        if (collision.tag.Equals("Player"))
        {
            //playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            audioSystemReference = GameObject.Find("Audio System").GetComponent<AudioSystem>();
            animator = GetComponentInChildren<Animator>();

            animator.enabled = true;
            CheckpointManager.lastCheckpointLocation = transform.position;

            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(collision.transform.position);
            audioSystemReference.PlaySound(audioSystemReference.checkpointEnter, collision.transform.position);

            TMP_Text tmpText = Instantiate(CheckpointText, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
            Destroy(gameObject.GetComponent<CheckpointPositionSetter>());
        }
    }
}
