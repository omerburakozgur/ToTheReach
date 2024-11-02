using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectiveUpdater : MonoBehaviour
{
    public string questText;
    UIManager uiManager;

    private void Awake()
    {
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            uiManager.UpdateQuestObjectiveText(questText);

        }
    }
}
