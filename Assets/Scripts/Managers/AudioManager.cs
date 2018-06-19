using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SoundContainer sounds;

    private static AudioManager inst;
    public static AudioManager GetInstance
    {
        get { return inst; }
    }

    // Use this for initialization
    void Awake()
    {
        if (inst != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Sound s;
        var list = sounds.SoundList;
        for (int i = 0; i < list.Length; i++)
        {
            s = list[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        inst = this;
    }

    public void PlaySound(string Name)
    {
        Sound s = sounds.SoundList.First(x => x.name == Name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    public void StopSound(string Name)
    {
        Sound s = sounds.SoundList.First(x => x.name == Name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void StopAllSounds()
    {
        var list = sounds.SoundList;
        for (int i = 0; i < list.Length; i++)
        {
            list[i].source.Stop();
        }
    }
}
