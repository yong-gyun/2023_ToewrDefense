using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    Transform _root;

    public void Init()
    {
        _root = new GameObject("@Sound_Root").transform;

        for (int i = 0; i < (int)Define.Sound.MaxCount; i++)
        {
            GameObject go = new GameObject($"{(Define.Sound) i}");
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = _root;
        }

        Object.DontDestroyOnLoad( _root);

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Stop(Define.Sound type)
    {
        _audioSources[(int) type].Stop();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect)
    {
        Play(GetOrAddAduioclip(path, type), type);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect)
    {
        if(type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];    
            audioSource.clip = audioClip;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAduioclip(string path, Define.Sound type)
    {
        AudioClip audioClip = null;

        if(type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>($"Sounds/Bgm/{path}");
        }
        else
        {
            if(_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>($"Sounds/Effect/{path}");
                _audioClips.Add(path, audioClip);
            }
        }

        return audioClip;
    }

    public void Clear()
    {
        Stop(Define.Sound.Bgm);
        _audioClips.Clear();
    }
}