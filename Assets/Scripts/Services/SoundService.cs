using System;
using UnityEngine;

namespace Services
{
    public class SoundService : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _music;
        [SerializeField] private AudioClip _cutropeClip;

        private void Awake()
        {
            PlayMusic();
        }

        public void PlayCutRope()
        {
            PlaySound(_cutropeClip);
        }

        private void PlaySound(AudioClip audioClip)
        {
            var newSound = new GameObject();
            var audioSource = newSound.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void PlayMusic()
        {
            PlaySound(_music);
        }
    }
}