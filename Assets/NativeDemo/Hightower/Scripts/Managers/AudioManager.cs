using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class AudioManager : MonoBehaviour
    {
        public AudioClip Sfx_CashRegister;

        public AudioClip Sfx_PopOpen;

        public AudioClip Sfx_PopClose;

        public AudioClip Sfx_CharUnlock;



        public static AudioManager instance = null;
        private AudioClip[] SoundFiles;
        private Dictionary<string, int> ClipIndex = new Dictionary<string, int>();
        private AudioSource[] audioSources;
        private float[] audioSourcesVol;
        public AudioSource soundFx;
        public AudioSource music;
        public static bool isMute;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            audioSources = FindObjectsOfType<AudioSource>();
            audioSourcesVol = new float[audioSources.Length];
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSourcesVol[i] = audioSources[i].volume;
            }

        }
        public void Paused(bool state)
        {
            if (state)
            {
                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSourcesVol[i] = audioSources[i].volume;
                    audioSources[i].volume = 0;
                }
            }
            else
            {
                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].volume = audioSourcesVol[i];
                }

            }
        }
        public void InitAudio(AudioClip[] SoundFiles, AudioClip bgM)
        {
            for (int i = 0; i < SoundFiles.Length; i++)
            {
                ClipIndex.Add(SoundFiles[i].name, i);
            }
            this.SoundFiles = SoundFiles;
            music.clip = bgM;
        }
        public void Play(string clip, float volume = 1, bool PitchChange = false)
        {
            if (!ClipIndex.ContainsKey(clip))
                return;
            if (PitchChange)
                soundFx.pitch = Random.Range(0.8f, 1.2f);
            soundFx.PlayOneShot(SoundFiles[ClipIndex[clip]], volume);
        }
        public void PlayRandomSound(string[] Clips, float volume = 1, bool PitchChange = false)
        {
            if (PitchChange)
                soundFx.pitch = Random.Range(0.8f, 1.2f);
            int index = ClipIndex[Clips[Random.Range(0, Clips.Length)]];
            soundFx.PlayOneShot(SoundFiles[index], volume);
        }
        public void PlayButtonSound()
        {
            Play("button", PitchChange: true);
        }

        public void PlayMenuOpen()
        {
            if (Sfx_PopOpen)
                soundFx.PlayOneShot(Sfx_PopOpen, 1);
        }

        public void PlayMenuClose()
        {
            if (Sfx_PopClose)
                soundFx.PlayOneShot(Sfx_PopClose, 1);
        }


        public void PlayCashRegister()
        {
            if (Sfx_CashRegister)
                soundFx.PlayOneShot(Sfx_CashRegister, 1);
        }

        public void PlayCharUnlock()
        {
            if (Sfx_CharUnlock)
                soundFx.PlayOneShot(Sfx_CharUnlock, 1);
        }


    }
}