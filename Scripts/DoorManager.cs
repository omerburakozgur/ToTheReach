using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool keyObtained = false;
    public GameObject openDoorReference;
    public AudioSystem audioSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSystem = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        if (keyObtained)
        {
            openDoorReference.SetActive(true);
            // Play door opened sound fx
            audioSystem.PlaySound(audioSystem.doorOpen, 8f);

            Destroy(gameObject);
        }
        else
        {
            // Enable fading tooltip text here (same as damage text prefab)
            audioSystem.PlaySound(audioSystem.doorClick, 8f);
            // play wood sound fx
        }
    }

}
