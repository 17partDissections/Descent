using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Q17pD
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        private AudioSource _audioSourcePrefab;
        private List<AudioSource> _activeAudioSources = new List<AudioSource>(), _availableAudioSources = new List<AudioSource>();
        private float _tempMusicDB, _tempSFXDB;

        private void Start() { _audioSourcePrefab = Resources.Load<AudioSource>("SFXAudioSource"); Revert(); }
        private void Update() { for (int i = _activeAudioSources.Count - 1; i >= 0; i--) if (!_activeAudioSources[i].isPlaying) ReturnSourceToPool(_activeAudioSources[i]); }
        public AudioSource PlaySound(SoundType soundType, AudioClip clip, float volume = 1f, bool loop = false, bool fade = false, float fadeTime = 5)
        {
            AudioSource source = GetAvailableAudioSource();
            source.clip = clip; source.loop = loop; source.volume = volume;
            if (soundType == SoundType.Music) source.outputAudioMixerGroup = _audioMixerGroup.audioMixer.FindMatchingGroups("Music")[0];
            else source.outputAudioMixerGroup = _audioMixerGroup.audioMixer.FindMatchingGroups("SFX")[0];
            _activeAudioSources.Add(source);
            source.Play();
            if (fade) { source.DOKill(); source.volume = 0; source.DOFade(volume, fadeTime); }
            return source;

        }
        public void StopSound(AudioSource source, bool fade = false, float fadeTime = 5)
        {
            if (fade) { source.DOKill(); source.DOFade(0, fadeTime).OnComplete(() => { source.Stop(); ReturnSourceToPool(source); }); }
            else { source.Stop(); ReturnSourceToPool(source); }
        }
        public void StopAllSounds(bool fade = false, float fadeTime = 5)
        {
            for (int i = _activeAudioSources.Count - 1; i >= 0; i--)
            {
                if (fade) { _activeAudioSources[i].DOKill(); _activeAudioSources[i].DOFade(0, fadeTime).OnComplete(() => _activeAudioSources[i].Stop()); }
                else _activeAudioSources[i].Stop();
                ReturnSourceToPool(_activeAudioSources[i]);
            }
        }
        private AudioSource GetAvailableAudioSource()
        {
            if (_availableAudioSources.Count > 0)
            {
                AudioSource source = _availableAudioSources[_availableAudioSources.Count - 1];
                _availableAudioSources.RemoveAt(_availableAudioSources.Count - 1);
                return source;
            }
            AudioSource newSource = Instantiate(_audioSourcePrefab, transform);
            return newSource;
        }
        private void ReturnSourceToPool(AudioSource source)
        {
            source.clip = null;
            _activeAudioSources.Remove(source); _availableAudioSources.Add(source);
        }
        public void SetMusicVolume(float percent) => _audioMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, percent));
        public void SetSFXVolume(float percent) => _audioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 0, percent));
        public void Save()
        {
            PlayerPrefs.SetFloat("MusicVolume", _tempMusicDB);
            PlayerPrefs.SetFloat("SFXVolume", _tempSFXDB);
        }
        public void Revert()
        {
            _audioMixerGroup.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0f));
            _audioMixerGroup.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0f));
        }
        public void SetVolumeFromSlider(SoundType soundType, float value)
        {
            _tempMusicDB = Mathf.Lerp(-80, 0, value);
            if (soundType == SoundType.Music) _audioMixerGroup.audioMixer.SetFloat("MusicVolume", _tempMusicDB);
            else _audioMixerGroup.audioMixer.SetFloat("MusicVolume", _tempMusicDB);
        }
        public void SetVolumeFromToggle(SoundType soundType, bool value)
        {
            _tempMusicDB = value == true ? 0 : -80;
            if (soundType == SoundType.Music) _audioMixerGroup.audioMixer.SetFloat("MusicVolume", _tempMusicDB);
            else _audioMixerGroup.audioMixer.SetFloat("MusicVolume", _tempMusicDB);
        }
    }
    public enum SoundType { Music, SFX }
}