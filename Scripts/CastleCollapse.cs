using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleCollapse : MonoBehaviour
{
    public CameraShake cameraShakeReference;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = StaticVariables.effectVolume;
    }

    public void CameraShake()
    {
        cameraShakeReference.StartShakeCamera();
    }
}
