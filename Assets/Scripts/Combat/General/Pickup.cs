using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Saving;
using UnityEngine.Events;
using RPG.Movement;

namespace RPG.Combate {
    public class Pickup : MonoBehaviour
    {
        [SerializeField] int coinValue;
        [SerializeField] int[] resources;
        [SerializeField] Animator animator;
        [SerializeField] AudioSource audioSource;
        [SerializeField] bool changeParentOnEnable = false;

        [Header("Event pickupManager")]
        [SerializeField] UnityEvent OnPickup;
        [SerializeField] int staminaToRestore = 25;

        bool isUsed = false;


        private void OnEnable()
        {
            if (changeParentOnEnable) {
                transform.parent = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isUsed)
            {
                isUsed = true;
                PickupCoin(other.gameObject);

            }
        }

        private void PickupCoin(GameObject subject)
        {
            /*if (coinValue > 0)
            {
                //GameManager.gameManager.AugmentCoins(coinValue);
                if (ControlCoinsGeneral.Instance.ResourcesMaxCap(resources)) {
                    print("Is pme buy");
                    isUsed = false;
                    return;
                }
                animator.Play("DestroyWithEffect");
                audioSource.Play();
                ControlCoinsGeneral.Instance.UpdateResources(resources);
                OnPickup.Invoke();
            }
            else if (usedForStamina) {
                //subject.GetComponent<MoverPlayer>().augmentStamina(staminaToRestore);
                animator.Play("DestroyWithEffect");
                audioSource.Play();
                DestroyCoin();
            }*/
            DestroyCoin();
        }

        void DestroyCoin() {
            Destroy(gameObject, audioSource.clip.length-1.6f);
        }
    }
}
