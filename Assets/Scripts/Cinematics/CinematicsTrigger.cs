using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour, ISaveable
    {

        [Header("Normal")]
        [SerializeField] GameObject[] animatorPlayables;
        [SerializeField] bool activeInAwake = false;

        PlayableDirector playableDirector;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
            if (activeInAwake) PlayTimeline();
        }

        bool alreadyTriggered = false;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !alreadyTriggered) {
                PlayTimeline();
            }
        }

        public void PlayTimeline() {
            if (alreadyTriggered) return;
            if(playableDirector == null) playableDirector = GetComponent<PlayableDirector>();
            alreadyTriggered = true;
            GetComponent<CinematicsControlRemove>().DisableControl(playableDirector);
            playableDirector.Play();
        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
            print(alreadyTriggered);
        }
    }
}
