using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    public float timeToFade = 5f;
    private float timeElapsed = 0f;

    private Color startColor;

    TextMeshProUGUI healthText;
    RectTransform textTransform;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        healthText = GetComponent<TextMeshProUGUI>();
        startColor = healthText.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
            healthText.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
