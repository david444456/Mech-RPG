using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class MusicIntroMenu : MonoBehaviour
    {
        [SerializeField] AudioClip clipIntroMusic;
        [SerializeField] AudioClip clipLoopMusic;
        [SerializeField] float timeAddedToLenghtLoopMusic = 2f;

        AudioSource audioSource;


        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(StartMusicLoopInAFewSeconds());
        }

        IEnumerator StartMusicLoopInAFewSeconds() {
            print(clipIntroMusic.length);
            yield return new WaitForSeconds(clipIntroMusic.length);
            audioSource.clip = clipLoopMusic;
            audioSource.Play();
        }
    }
}
