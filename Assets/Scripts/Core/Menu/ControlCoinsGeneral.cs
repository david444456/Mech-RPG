using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Core
{
    public class ControlCoinsGeneral : MonoBehaviour, ISaveable
    {
        public static ControlCoinsGeneral Instance;
        public int CoinPlayer { get; private set; }
        public int[] resources = { 50,50};
        public int[] limitStorageResources = { 100, 100 };

        [Header("UI ")]
        [SerializeField] TextMeshProUGUI[] textCoins = null;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            //UpdateTextManager();
        }

        public void AugmentCoins(int Augment)
        {
            CoinPlayer = +Augment;
            UpdateTextManager();
        }

        public bool ResourcesMaxCap(int[] newResources) {
            for (int i = 0; i < resources.Length; i++) {
                if (resources[i] >= limitStorageResources[i] && newResources[i] > 0) {
                    return true;
                }
            }
            return false;
        }

        public void UpdateResources(int[] newResources) {
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] += newResources[i];

                if (limitStorageResources[i] < resources[i]) {
                    resources[i] = limitStorageResources[i];
                }

                textCoins[i].text = resources[i].ToString() + "/" + limitStorageResources[i].ToString();
            }

        }

        public void UpdateLimitResources(int[] newLimitResources)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                limitStorageResources[i] += newLimitResources[i];
                textCoins[i].text = resources[i].ToString() + "/" + limitStorageResources[i].ToString();
            }

        }

        public int[] GetActualResources() => resources;

        private void UpdateTextManager() {
            for (int i = 0; i < resources.Length; i++) {
                textCoins[i].text = resources[i].ToString() + "/" + limitStorageResources[i].ToString();
            }
        }

        public object CaptureState()
        {
            return resources;
        }

        public void RestoreState(object state)
        {

            resources = (int[])state;
            UpdateTextManager();
            print(" coins: " + resources[0].ToString());
            //textCoins.text = CoinPlayer.ToString();
        }
    }
}
