using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SoundRandsElement
{
    [Range(0, 1)]
    public float volume = 1;
    public List<AudioClip> list;
}
[System.Serializable]
public class SoundSingleElement
{
    [Range(0, 1)]
    public float volume = 1;
    public AudioClip clip;

}

[AutoBind]
public class GameSoundManager : MonoBehaviour,IListener<PlaySoundMsg>
{
    public AudioSource musicAudio;
    public float musicVolume=0.5f;
    public SoundMap sounds;
    public SoundClipMap soundClips;
    public SlotSoundSettings slotSoundSettings;
    public GenericDictionary<ESoundType, SoundSingleElement> soundClipsSingle;
    public GenericDictionary<ESoundType, SoundRandsElement> soundClipsRand;
    //[Serializable] public class SoundClipRandMap : SerializableDictionary<ESoundType, AudioClip> { }

    void Awake()
    {
        SoundManager.MusicVolume = PlayerPrefs.GetFloat("music", 1);
        SoundManager.SoundVolume = PlayerPrefs.GetFloat("soundFx", 1);
    }

    void Start()
    {
        musicAudio.PlayLoopingMusicManaged(musicVolume, 1.0f, true);
    }

    public float SoundVolume
    {
        get
        {
            return SoundManager.SoundVolume;
        }
        set
        {
            SoundManager.SoundVolume = value;
            PlayerPrefs.SetFloat("soundFx", value);
        }
    }
    public float MusicVolume
    {
        get
        {
            return SoundManager.MusicVolume;
        }
        set
        {
            SoundManager.MusicVolume = value;
            PlayerPrefs.SetFloat("music", value);
        }
    }
    [Button]
    public void PlaySlot()
    {
        StartCoroutine(slotSoundSettings.Play());

    }
    Dictionary<ESoundType,int> lastRandom=new Dictionary<ESoundType,int>();

    private AudioSource GetAudioSource(ESoundType type)
    {
        if (!sounds.TryGetValue(type, out AudioSource sound))
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            sound = go.AddComponent<AudioSource>();
            sounds[type] = sound;

        }

        return sound;
    }
    private void PlayRandomClip(ESoundType type, SoundRandsElement element)
    {
        var list = element.list;
        AudioSource sound=GetAudioSource(type);
        int index = UnityEngine.Random.Range(0, list.Count);
        if (list.Count > 1)
        {
            if (lastRandom.TryGetValue(type, out var lastIndex) && lastIndex == index)
            {
                index++;
                if (index >= list.Count)
                {
                    index = 0;
                }
            }
            lastRandom[type] = index;
        }
        sound.clip = list[index];
        SoundManager.PlayOneShotSound(sound, sound.clip,element.volume);

    }
    public void PlaySound(ESoundType type,bool isForceVolume=false,float forceVolume=1)
    {
        if (type == ESoundType.None)
        {
            return;
        }

        if (soundClipsRand.TryGetValue(type, out var list))
        {
            PlayRandomClip(type, list);
            return;
        }
        if (soundClipsSingle.TryGetValue(type, out var element))
        {
            var audioSource=GetAudioSource(type);
            if (isForceVolume)
            {
                SoundManager.PlayOneShotSound(audioSource, element.clip, forceVolume);
            }
            else
            {
                SoundManager.PlayOneShotSound(audioSource, element.clip, element.volume);
            }
            //SoundManager.PlayOneShotSound(audioSource, element.clip, element.volume);
            return;
        }

        if (!sounds.TryGetValue(type, out AudioSource sound))
        {
            if (soundClips.TryGetValue(type, out var clip))
            {
                GameObject go = new GameObject();
                go.transform.parent = transform;
                sound=go.AddComponent<AudioSource>();
                sound.clip = clip;
                sounds[type] = sound;
            }
        }

        if (sound != null)
        {
            SoundManager.PlayOneShotSound(sound,sound.clip);
            //sound.PlayOneShot(sound.clip);
        }
    }
    [Serializable] public class SoundMap : SerializableDictionary<ESoundType, AudioSource> { }
    [Serializable] public class SoundClipMap : SerializableDictionary<ESoundType, AudioClip> { }
    //[Serializable] public class SoundClipRandMap : SerializableDictionary<ESoundType, AudioClip> { }



    public void Handle(PlaySoundMsg ev)
    {
        PlaySound(ev.sound,ev.isForceVolume,ev.forceVolume);
    }
}
[System.Serializable]
public class SlotSoundSettings
{
    public AudioSource audio1;
    public AudioSource audio2;
    public float delay1 = 0.4f;
    public float delay2 = 0.4f;

    public float period = 0.08f;
    public float period2 = 0.2f;
    public float period3 = 0.2f;
    public IEnumerator Play(int count = 3)
    {
        float current = 0;
        while (current < delay1)
        {
            current += Time.deltaTime;
            SoundManager.PlayOneShotSound(audio1, audio1.clip);
            yield return new WaitForSeconds(period);
        }

        current = 0;
        while (current < delay2)
        {
            current += Time.deltaTime;
            SoundManager.PlayOneShotSound(audio1, audio1.clip);
            yield return new WaitForSeconds(period + (period2 - period) * current / delay2);
        }

        SoundManager.PlayOneShotSound(audio2, audio2.clip);

        if (count > 1)
        {
            yield return new WaitForSeconds(period3);
            SoundManager.PlayOneShotSound(audio1, audio1.clip);
            yield return new WaitForSeconds(period3);
            SoundManager.PlayOneShotSound(audio1, audio1.clip);
            yield return new WaitForSeconds(period3);
            SoundManager.PlayOneShotSound(audio2, audio2.clip);

            if (count > 2)
            {

                yield return new WaitForSeconds(period3);
                SoundManager.PlayOneShotSound(audio1, audio1.clip);
                yield return new WaitForSeconds(period3);
                SoundManager.PlayOneShotSound(audio1, audio1.clip);
                yield return new WaitForSeconds(period3);
                SoundManager.PlayOneShotSound(audio1, audio1.clip);
                yield return new WaitForSeconds(period3);
                SoundManager.PlayOneShotSound(audio2, audio2.clip);
            }
        }

    }
}

public enum ESoundType
{
    None = 0,
    ButtonDefault = 1,
    PopupShow = 2,
    PopupHide = 3,
    Rewerd1 = 4,
    OpenCard = 5,
    OpenChest = 6,
    Broken1 = 7,
    Broken2 = 8,
    Special1 = 9,
    Build = 10,
    FindObject=11,
    Hint=12,
    Locked=13,
    UnLock=14,
    OpenLidChest=15, 
    ObjectFly=16,
    PickUp=16,
    FoundCat=17,
}
[System.Serializable]
public class PlaySoundMsg
{
    public ESoundType sound;
    public float forceVolume = 1f;
    public bool isForceVolume=false;
    public PlaySoundMsg()
    {

    }
    public PlaySoundMsg(ESoundType sound)
    {
        this.sound=sound;
    }
}