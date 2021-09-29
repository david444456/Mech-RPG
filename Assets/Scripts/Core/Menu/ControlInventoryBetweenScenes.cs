using RPG.Combat;
using RPG.Control;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ControlInventoryBetweenScenes : MonoBehaviour, ISaveable
    {
        public static ControlInventoryBetweenScenes Instance;


        [HideInInspector] public GameObject player;

        [Header("Weapons")]
        public WeaponConfig[] weaponsConfig;
        [SerializeField] public int[] UniqueIdentifierWeapon;
        [SerializeField] public WeaponConfig[] AllTypesWeapons;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void ChangeWeaponByIndex(int index, WeaponConfig newWeapon) {
            if (index < 0 || index >= weaponsConfig.Length) return;
            weaponsConfig[index] = newWeapon;
        }

        public void ChangeWeaponDefinitiveByIndex(int index, WeaponConfig newWeapon) {
            if (index < 0 || index >= weaponsConfig.Length) return;
            weaponsConfig[index] = newWeapon;
            UniqueIdentifierWeapon[index] = newWeapon.UniqueIdentifier;
        }

        public void LoadWeaponsIdentifierByIndex()
        {
            player = GameObject.FindWithTag("Player");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < AllTypesWeapons.Length; j++)
                {
                    if (UniqueIdentifierWeapon[i] == AllTypesWeapons[j].UniqueIdentifier)
                    {
                        weaponsConfig[i] = AllTypesWeapons[j];
                    }
                }
                player.GetComponent<PlayerController>().SetTheWeaponByIndex(weaponsConfig[i], i);
                //UniqueIdentifierWeapon[i] = weaponsConfig[i].UniqueIdentifier;
            }
        }

        public void UpdateActualWeaponsByPlayerInThaGame()
        {
            player = GameObject.FindWithTag("Player");
            for (int i = 0; i < weaponsConfig.Length; i++)
            {
                player.GetComponent<PlayerController>().SetTheWeaponByIndex(weaponsConfig[i], i);

            }
            player.GetComponent<PlayerController>().SetPrimaryWeaponInPlayer();
        }

        public WeaponConfig GetWeaponByIndex(int index) => weaponsConfig[index];

        public WeaponConfig GetWeaponByUniqueIdent(int indexUnique) {
            for (int j = 0; j < AllTypesWeapons.Length; j++)
            {
                if (indexUnique == AllTypesWeapons[j].UniqueIdentifier)
                {
                    return AllTypesWeapons[j];
                }
                print("Devuelvo un arma " + j);
            }
            return null;
        }

        public object CaptureState()
        {
            return UniqueIdentifierWeapon;
        }

        public void RestoreState(object state)
        {
            UniqueIdentifierWeapon = (int[])state;
            for (int i = 0; i < weaponsConfig.Length; i++)
            {
                print(UniqueIdentifierWeapon[i]);
            }
            LoadWeaponsIdentifierByIndex();
            //textCoins.text = CoinPlayer.ToString();
        }
    }
}
