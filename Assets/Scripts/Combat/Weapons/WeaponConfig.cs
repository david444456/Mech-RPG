using UnityEngine;
using RPG.Core;
using System;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/ Make new weapon", order = 0)]
    public class WeaponConfig : ScriptableObject {

        [HideInInspector] public GameObject instigatorCharacterWeapon;

        public int UniqueIdentifier = 0;
        public bool AttackArea = false;
        public bool AttackWithCombo = false;

        public float damageArea = 3;
        public float weaponDamage = 5f;
        public Sprite spriteWeapon = null;
        public float[] CooldownAbilities = null;
        public GameObject spawnWeaponPickup = null;

        [Header("Animation")]
        [SerializeField] public string attackAnimationWeapon = "attack";

        [Header("Type weapon")]
        public TypeWeapon typeWeapon;
        public int damageForTypeWeapon = 6;

        [Header("Store")]
        public int priceWeaponStore = 50;
        [SerializeField] int[] resourcesCostsInStore;
        [TextArea] public string descriptionWeapon;
        public Vector3 positionStore;
        public Vector3 rotationStore;
        public GameObject spawnSimpleWeapon = null;

        [Header("Values")]
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] public Weapon weaponObject = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";


        /// <summary>
        /// Spawn a new weapon with animatorController, range and damage
        /// <paramref name="animator"/>
        /// </summary>
        public Weapon Spawn(Transform rightHad, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHad, leftHand);

            Weapon weapon = null;
            if (weaponObject != null)
            {
                Transform handTransform = GetTransform(rightHad, leftHand);
                weapon = Instantiate(weaponObject, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else {
                if (overrideController != null) {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
            return weapon;
        }

        /// <summary>
        /// Destroy old weapon to spawn new weapon
        /// </summary>
        private void DestroyOldWeapon(Transform rightHad, Transform leftHand)
        {
            if (rightHad == null || leftHand == null) return;
            Transform oldWeapon = rightHad.Find(weaponName);
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "D";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHad, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHad;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasPorjectile() {
            return projectile != null;
        }

        public void LauchProjectile(Transform rightHand, Transform leftHand, Vector3 direction, GameObject instigator, float calculatedDamage) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(direction, instigator, calculatedDamage);
        }

        /// <summary>
        /// Return the weapon damage
        /// </summary>
        public float GetDamage()
        {
            return weaponDamage;
        }

        public int[] GetResourcesCostInStore() => resourcesCostsInStore;

        public float GetPorcentageBonus() {
            return percentageBonus;
        }

        /// <summary>
        /// Return the range of the attack
        /// </summary>
        public float GetRange()
        {

            return weaponRange;
        }

        public bool AttackAreaWeapon() {
            return AttackArea;
        }

        
    }

    public enum TypeWeapon{
        Fire,
        Ice,
        Insecs,
        Water,
        Shovel,
        None
    }
}