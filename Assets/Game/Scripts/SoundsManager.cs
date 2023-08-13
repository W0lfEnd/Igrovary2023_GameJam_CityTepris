using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    [SerializeField] private GameObject _soundsParent;
    [SerializeField] private AudioSource _soundPrefab;
    [SerializeField] private Sound[] _sounds;
    private List<AudioSource> _instantiatedSounds = new();

    private void Awake() => InitializeSingleton();

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance == this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TryPlaySoundByType(SoundType soundType, bool isLooped = false)
    {
        AudioClip audioClip = GetRandomAudioClipByType(soundType);

        if (audioClip == null)
            return;

        AudioSource sound = Instantiate(_soundPrefab, _soundsParent.transform);

        sound.clip = audioClip;
        sound.loop = isLooped;
        sound.Play();

        _instantiatedSounds.Add(sound);

        if (isLooped)
            return;

        DOVirtual.DelayedCall(audioClip.length, () => Destroy(sound.gameObject));
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _instantiatedSounds.Count; i++)
        {
            if(_instantiatedSounds[i] != null)
            {
                Destroy(_instantiatedSounds[i].gameObject);
            }
        }

        _instantiatedSounds.Clear();
    }

    private AudioClip GetRandomAudioClipByType(SoundType soundType)
    {
        AudioClip audioClip = null;

        for (int i = 0; i < _sounds.Length; i++)
        {
            if(_sounds[i].type == soundType)
            {
                audioClip = _sounds[i].GetRandomClip();
                break;
            }
        }

        return audioClip;
    }
}

public enum SoundType { EnemyHit, EnemyDeath, WorldFlip, TurretReloading, TurretShoot, BladeSpinning }

[Serializable]
public struct Sound
{
    public SoundType type;
    public AudioClip[] audioClips;

    public AudioClip GetRandomClip()
    {
        if (audioClips == null || audioClips.Length == 0)
            return null;

        Random rand = new Random();
        return audioClips[rand.Next(0, audioClips.Length)];
    }
}
