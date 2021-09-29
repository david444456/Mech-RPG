using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public abstract class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        //this script controls the player attack and cancel attack 
        public float damageIncreased = 0;
        public WeaponConfig currentWeaponConfig = null;

        [SerializeField] public float timeBetweenAttack = 1f;
        [SerializeField] public float timeToMove = 2f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform LeftHandTransform = null;
        [SerializeField] protected WeaponConfig defaultWeapon;

        [Header("Attack area")]
        public Vector3 targetPosition;
        public float shoutDistance = 2f;
        public Transform AttackAreaTF = null;

        [Header("Enemy")]
        [HideInInspector] public bool EnemyBossAttack = false;
        [SerializeField] public float distanceToAttack = -1;
        [HideInInspector] public Weapon weaponActual;

        [Header("ONLY USED TO NOT MOVE")]
        [HideInInspector] public bool CantMove = false;
        [HideInInspector] public float damage;
        [HideInInspector] public HealthPlayer playerHealth = null; 
        [HideInInspector] public PlayerInformationBetweenScenes gameManager;
        [HideInInspector] public Animator animator;
        [HideInInspector] public float TimeSinceLastAttack = Mathf.Infinity;
        [HideInInspector] public Mover mover;
        [HideInInspector] public Health target;

        public LazyValue<Weapon> currentWeapon;

        protected string stringAnimationAttack = "attack";

        private void Awake()
        {
            //currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        Weapon SetupDefaultWeapon() {

            return AttachWeapon(defaultWeapon);
        }

        public virtual void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            currentWeapon.ForceInit();

        }

        public void EquipWeapon(WeaponConfig weapon, int index) {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
            UpdateGameManagerWeapon(weapon, index);


            //gun 
            if (weapon.HasPorjectile()) weapon.instigatorCharacterWeapon = gameObject;

            //animation
            stringAnimationAttack = weapon.attackAnimationWeapon;
        }

        /// <summary>
        /// spawn the weapon (blade)
        /// </summary>  
        public Weapon AttachWeapon(WeaponConfig weapon)
        {
            //currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, LeftHandTransform, animator);
        }

        /// <summary>
        /// Get the enemy health. The one who is currently attacking
        /// </summary>
        public Health GetTarget() {
            return target;
        }

        public Weapon GetWeapon()
        {
            return currentWeapon.value;
        }

        //"Attack" it sends its IAction and changes the target
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            targetPosition = AttackAreaTF.position;
            if (combatTarget == null) return;
            target = combatTarget.GetComponent<Health>();

        }

        /// <summary>
        /// Return the damage with stat + damage
        /// </summary>
        public float DamageReturn()
        {
            
            return GetComponent<BaseStats>().GetStat(Stat.Damage) + damageIncreased;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPorcentageBonus();
            }
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();

        }

        public virtual void StopAttack()
        {
            //animator.ResetTrigger("ComboQ");
            animator.SetTrigger("stopAttack");
        }

        public void ChangeActiveColliderWeapon(bool enabledOr) {
            print(enabledOr + " " + weaponActual.boxCollider.isTrigger);
            weaponActual.boxCollider.isTrigger = enabledOr;
        }

        public virtual void AttackBehaviour()
        {
            
        }

        public float SetOnHit(Health health)
        {
            //Event OnHit
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
             //for special damage to every weapon
            if (health.typeWeapon == currentWeaponConfig.typeWeapon)
            {
                return currentWeaponConfig.damageForTypeWeapon;
            }
            else
            {
                return 0;
            }
        }

        // Animation event (principal)
        public virtual void Shoot(float shoutArea, bool AreaZero)
        {
            //cancel movement
            mover.Cancel();
            
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)
                && !GetIsInRange(combatTarget.transform)
                ) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        /// <summary>
        /// controls animations the player
        /// </summary>
        public void TriggetAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger(stringAnimationAttack);
        }

        public void LaunchProyectile(Vector3 direction, float damage)
        {
            if (target != null) transform.LookAt(target.transform.position);
            currentWeaponConfig.LauchProjectile(rightHandTransform, LeftHandTransform, direction, gameObject, damage);
        }

        //this method "GetIsInRange" keeps the distance between Enemy and Player.
        public bool GetIsInRange(Transform targetTransform)
        {
            if (distanceToAttack > 0)
            {
                return Vector3.Distance(AttackAreaTF.transform.position, targetTransform.position) < distanceToAttack;
            }
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        void UpdateGameManagerWeapon(WeaponConfig weapon, int index)
        {
            if (index >= 0)
            {
                ControlInventoryBetweenScenes.Instance.ChangeWeaponDefinitiveByIndex(index, weapon);
            }
        }

        void Hit(float AttackArea)
        {
            Shoot(AttackArea, AttackArea > 0);
        }

        #region Save System
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;

            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);

            //EquipWeapon(weapon);
        }
        #endregion
    }
}
