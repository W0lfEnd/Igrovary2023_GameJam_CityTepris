using DG.Tweening;
using System;
using UnityEngine;
using Random = System.Random;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    [SerializeField] private GameObject _soundsParent;
    [SerializeField] private AudioSource _soundPrefab;
    [SerializeField] private Sound[] _sounds;

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

    public void TryPlaySoundByType(SoundType soundType)
    {
        AudioClip audioClip = GetRandomAudioClipByType(soundType);

        if (audioClip == null)
            return;

        AudioSource soundPrefab = Instantiate(_soundPrefab, _soundsParent.transform);
        soundPrefab.clip = audioClip;
        soundPrefab.Play();

        DOVirtual.DelayedCall(audioClip.length, () => Destroy(soundPrefab.gameObject));
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
