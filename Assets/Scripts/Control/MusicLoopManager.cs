using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class MusicLoopManager : MonoBehaviour
    {
        [SerializeField] AudioSource audioMainMusic;
        [SerializeField] AudioClip audioClipFightLoop;


        void Start()
        {

        }

        public void ChangeMusicToAttack() {
            audioMainMusic.clip = audioClipFightLoop;
            audioMainMusic.Play();
        }

    }
}
