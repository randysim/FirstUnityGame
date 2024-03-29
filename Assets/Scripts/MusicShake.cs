﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicShake : MonoBehaviour
{
    public CameraShake cameraShake;
    public AudioSource backgroundTrack;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;

    private void Awake()
    {
        StartCoroutine(cameraShake.Shake(100000f));
        clipSampleData = new float[sampleDataLength];
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            backgroundTrack.clip.GetData(clipSampleData, backgroundTrack.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

            if (cameraShake == null) {
                cameraShake = FindObjectOfType<CameraShake>();
                StartCoroutine(cameraShake.Shake(100000f));
            }

            cameraShake.setMagnitude(clipLoudness);
        }

    }
}
