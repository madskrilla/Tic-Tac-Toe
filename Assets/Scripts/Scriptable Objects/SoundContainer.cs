using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
class Sound
{
    public string name;
    public AudioClip soundClip;
    [Range(0, 1f)]
    public float volume;
    public bool loop;
    public AudioSource source;
}

[CreateAssetMenu(fileName = "SoundList", menuName = "Settings/SoundList")]
class SoundContainer : ScriptableObject
{
    public Sound[] SoundList;
}

