using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundName name;
    // public AudioType audioType = AudioType.Sfx;
    public AudioClip clip;
    public bool loop = false;
    public bool playOnAwake = false;
    [Range(0f, 1f)]
    public float volume = 1;
    // TODO conditionally show pitch in inspector
    [Range(0.1f, 3f)]
    public float pitch = 1;
    public bool randomPitch = false;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [HideInInspector]
    public AudioSource source;
}