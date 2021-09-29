using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;
using UnityEngine.AI;
using RPG.Control;
using RPG.Combat;
using RPG.Movement;


namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public abstract class Health : MonoBehaviour, ISaveable
    {
        public TypeWeapon typeWeapon; //for damage

        [SerializeField] public TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent OnDie;
        [SerializeField] public UnityEvent OnDieSave;
        [SerializeField] public bool ReceiveHit = true;
        [SerializeField] public int damageToReceiveHit = 20;

        [HideInInspector] public Fighter fighter;
        [HideInInspector] public Mover mover;

        [HideInInspector] public LazyValue<float> healthPoints;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool isDead = false;
        [HideInInspector] public bool isPlayer = false;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            animator = GetComponent<Animator>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        public virtual void Start()
        {
            healthPoints.ForceInit();
        }

        /// <summary>
        /// Take damage in the health gameobject
        /// </summary>
        /// <param name="instigator"> GameObject who does the damage</param>
        /// <param name="damage">damage value (float) </param>
        public virtual void TakeDamage(GameObject instigator, float damage) {
            //if dead return
            if (isDead) return;

            //take damage
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (healthPoints.value <= 0) //die general
            {
                //OnDieSave.Invoke();
                OnDie.Invoke();
                if(mover!= null) mover.Cancel();
            }
        }

        /// <summary>
        /// Augment the health with heal parameter
        /// </summary>
        /// <param name="heal"></param>
        public void Heal(float heal) {
            healthPoints.value = Mathf.Min(healthPoints.value + heal, GetMaxHealthPoints());
            print(healthPoints.value);
        }

        /// <returns>Return health points</returns>
        public float GetHealthPoints() => healthPoints.value;

        public float GetInitialHealth() => GetComponent<BaseStats>().GetStat(Stat.Health);

        /// <returns>Return health max points</returns>
        public float GetMaxHealthPoints() {

            return GetComponent<BaseStats>().GetStat(Stat.Health);
            
        }
        
        /// <returns>If is dead</returns>
        public bool IsDead()
        {
            return isDead;
        }

        public float GetFraction() {

            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetLevel()
        {
            return GetComponent<BaseStats>().GetLevel();
        }

        public virtual void Die(string animatorStringDeath)
        {
            //dead
            if (isDead) return;
            isDead = true;

            //animator
            Animator animator = GetComponent<Animator>();
            animator.Play(animatorStringDeath, -1);

            //cancel action
            //GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        //captureState save the different variables
        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Die("DeathSave");
            }
        }

    }
}
