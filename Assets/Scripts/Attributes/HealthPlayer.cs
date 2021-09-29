using RPG.Combat;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthPlayer : Health
    {
        [SerializeField] float regenerationPercentage = 70;

        [Header("Body")]
        [Tooltip("Head of the player for the armor")]
        [SerializeField] Transform head;

        float _actualArmor = 0;
        float timeBetweenReceiveDamage = 0;
        WeaponConfig weaponEnemyInstigator;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            isPlayer = true;
        }

        private void OnParticleCollision(GameObject other)
        {
            // if(tag == "Player")
            if ((-timeBetweenReceiveDamage + Time.time) > 0.6f)
            {
                timeBetweenReceiveDamage = Time.time;
                weaponEnemyInstigator = other.GetComponentInParent<Projectile>().weaponConfig;
                TakeDamage(weaponEnemyInstigator.instigatorCharacterWeapon, weaponEnemyInstigator.damageArea);
            }

        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public override void Die(string animatorStringDeath)
        {
            base.Die(animatorStringDeath);
            GetComponent<PlayerController>().enabled = false;
        }

        public override void TakeDamage(GameObject instigator, float damage)
        {
            base.TakeDamage(instigator, damage);

            if (healthPoints.value <= 0)
            {
                takeDamage.Invoke(-damage);
                Die("Death");
                GetComponent<PlayerUI>().TextDeadEnemy(instigator.GetComponent<BaseStats>().nameCharacter, instigator.GetComponent<BaseStats>().stringTextDead());
            }
            else {

                if (ReceiveHit && damage >= damageToReceiveHit)
                {
                    animator.Play("HitHard", 0);
                }
                else {
                    animator.Play("Hit1", 0);
                }
                print(GetActualDamage(damage) + " " + damage);
                takeDamage.Invoke(-GetActualDamage(damage));
            }
        
        }

        public void ReceivedNewArmor(int newArmor) => _actualArmor += newArmor;

        public void EliminatedArmor(int newArmor) => _actualArmor -= newArmor;

        public float GetArmor() => _actualArmor;

        private float GetActualDamage(float damage) => Mathf.Max( 0, damage - GetReduceDamageArmor());

        private int GetReduceDamageArmor() => (int) (_actualArmor * 0.1f);

        private void RegenerateHealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealth);
        }
    }
}
