using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {

        [SerializeField] float experiencePoints = 0;
        [SerializeField] TakeExpEvent takeExp;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;

        [Serializable]
        public class TakeExpEvent : UnityEvent<float>
        {

        }

        public float GetPoints() {
            return experiencePoints;
            
        }


        public void GainExperience(float experience)
        {

            experiencePoints += experience;
            takeExp.Invoke(experience);
            onExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
