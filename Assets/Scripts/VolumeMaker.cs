using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMaker : MonoBehaviour
{
    public static void Play2DSound(AudioClip clip, float volume = 1.0f)
    {
        GameObject go = new GameObject("OneShot2DSound");
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f; // 0 = pure 2D sound
        source.Play();
        GameObject.Destroy(go, clip.length);
    }
    public static void Play2DSoundIfCloseToCamera(AudioClip clip, Vector3 position, float maxDistance = 20f, float volume = 1.0f)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        float distance = Vector3.Distance(mainCamera.transform.position, position);

        if (distance <= maxDistance)
        {
            Play2DSound(clip, volume);
        }
    }
}

