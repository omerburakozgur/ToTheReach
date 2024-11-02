using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectEnablerScript : MonoBehaviour
{
    public bool enableBool = true;
    public GameObject[] Objects;
    [Header("Events")]
    public UnityEvent EventsOnEnter;
    private NewPlayerController playerController;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            playerController = collision.GetComponent<NewPlayerController>();

            foreach (var obj in Objects)
            {
                obj.SetActive(enableBool);
            }
            EventsOnEnter.Invoke();
        }
        
    }

    public void SetFootstepState(int state)
    {
        playerController.footstepStates = state;
    }

    public void EnableOrDisableObjects()
    {
        foreach (var obj in Objects)
        {
            obj.SetActive(enableBool);
        }
    }
}
