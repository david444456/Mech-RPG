using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityEngine.UI;

namespace RPG.Saving {
    public class SavingPickup : MonoBehaviour
    {
        [Header("For save the party")]
        [SerializeField] GameObject UISaveSystem = null;
        [SerializeField] int distance = 0;
        [SerializeField] Image imageChange;
        [SerializeField] Sprite sprite;

        SavingSystem savingSystem;
        const string defaultSaveFile = "save";

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
                savingSystem = FindObjectOfType<SavingSystem>();
            if (PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.useTheJoystick) imageChange.sprite = sprite;
        }

        private void Update()
        {
            if (player == null) return;
            if (UISaveSystem.activeSelf) {
                if (Input.GetButtonDown("SaveParty"))
                {
                    player.GetComponent<Animator>().Play("Pray", 0);
                    savingSystem.Save(defaultSaveFile);
                }
                else if (Vector3.Distance(transform.position, player.transform.position) > distance)
                {
                    UISaveSystem.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                player = other.gameObject;
                UISaveSystem.SetActive(true);
            }
        }
    }
}
