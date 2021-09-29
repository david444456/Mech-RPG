using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;
using UnityEngine.Events;
using UnityEngine.UI;
using RPG.Core;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] GameObject UIButtonShow;
        [SerializeField] float distanceMax = 5f;
        [SerializeField] GameObject ObjectToActiveDisable = null;

        [Header("Event pickupManager")]
        [SerializeField] UnityEvent OnPickup;

        [Header("UI Stats weapon")]
        [SerializeField] Image image;
        [SerializeField] Text textTitle = null;
        [SerializeField] Text descriTitle = null;
        [SerializeField] Text TypeTitle = null;
        [SerializeField] Text StatsTitle = null;

        GameObject player;
        bool healthIsUsed = false;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;

            //change sprite
            if (PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.useTheJoystick) image.sprite = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.spriteFJoystick;


            //update value text
            if (healthToRestore > 0) return;
                textTitle.text = weapon.name;
            descriTitle.text = weapon.descriptionWeapon;
            TypeTitle.text = weapon.typeWeapon.ToString();
            StatsTitle.text = "Daño area: " + weapon.damageArea.ToString() + "\n" +
                              "Daño: " + weapon.weaponDamage.ToString() + "\n" +
                              "Enfriamiento: " + weapon.CooldownAbilities[0].ToString() + ", " + weapon.CooldownAbilities[1].ToString() + 
                              ", " + weapon.CooldownAbilities[2].ToString() + ", " + weapon.CooldownAbilities[3].ToString() + "\n" +
                              "Daño por tipo: " + weapon.damageForTypeWeapon.ToString() + "\n" +
                              "Rango: " + weapon.GetRange().ToString();
                              
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.tag == "Player"))
            {
                UIButtonShow.SetActive(true);
               // Pickup(other.gameObject);
            }
        }

        private void Update()
        {
            if (UIButtonShow.activeSelf) {
                if (Input.GetButtonDown("Pickup"))
                {
                    Pickup(player);
                }
                else  if(Vector3.Distance(player.transform.position, transform.position) > distanceMax){
                    UIButtonShow.SetActive(false);
                }

            }
        }

        private void Pickup(GameObject subject)
        {
            subject.GetComponent<Animator>().Play("TakingItem", -1);
            if (ObjectToActiveDisable != null)
            {
                ObjectToActiveDisable.SetActive(false);
            }
            if (weapon != null) {
                PlayerController playerController = subject.GetComponent<PlayerController>();

                if (playerController.GetActualWeaponPlayer().name != "Unarmed")
                    playerController.spawnActualWeaponPlayer();

                playerController.SetTheWeapon(weapon);
                //playerController.changeWeaponIndex(weapon);
                Destroy(gameObject);
            }
            if(healthToRestore > 0 && !healthIsUsed) {
                //only one use
                healthIsUsed = true;

                //*augment life
                subject.GetComponent<Health>().Heal(healthToRestore);

                //destroy object
                Destroy(gameObject, 0.5f);
            }
            //event
            OnPickup.Invoke();

        }

        IEnumerator HideForSeconds(float seconds) {
            ShowPickup(false);

            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
           
        }


        private void HidePickup()
        {
            GetComponent<CapsuleCollider>().enabled = false;
            
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<CapsuleCollider>().enabled = shouldShow;
            
            foreach (Transform child in transform) {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}