using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Combat { 
    public class PickupManager : MonoBehaviour, ISaveable
    {
        [SerializeField] GameObject[] gameObjectsPickup;

        [SerializeField] bool[] boolPickup;


        void Start()
        {
            for (int i = 0; i < gameObjectsPickup.Length; i++) {
                if (boolPickup[i]) {
                    gameObjectsPickup[i].SetActive(false);
                }

            }
        }

        public void SetTheBool(int value) {
            boolPickup[value] = true;

        }

        public object CaptureState()
        {
            return boolPickup;
        }

        public void RestoreState(object state)
        {
            boolPickup = (bool[])state;
        }
    }
}
