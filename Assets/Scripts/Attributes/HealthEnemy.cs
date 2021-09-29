using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Attributes
{
    public class HealthEnemy : Health
    {
        [Header("Enemy")]
        public GameObject enemyObjectAnimatorGeneral = null;
        public bool isUsedForReviving = false;
        [SerializeField] bool blockAttack;
        [SerializeField] public bool receiveExecute;
        [SerializeField] public bool readyToExecute;

        public override void Start()
        {
            //enemy
            if (isDead)
            {
                animator = GetComponent<Animator>();
                //animator.Play("Death", -1);
                OnDieSave.Invoke();
                if(GetComponent<AIController>() != null)
                    if (GetComponent<AIController>().effectTreeCut != null)
                    {
                        Destroy(GetComponent<AIController>().effectTreeCut);
                    }
            }

            base.Start();
        }

        public override void TakeDamage(GameObject instigator, float damage)
        {

            base.TakeDamage(instigator, damage); //take damage

            if (healthPoints.value <= 0)
            {
                if (instigator.tag == "Player")
                {
                    //enemy
                    int valueAnimatorDeath = UnityEngine.Random.Range(0, 3);
                    Die("Death" + valueAnimatorDeath.ToString());

                    AwardExperience(instigator);
                }
            }
            else
            {
                if (ReceiveHit && damage >= damageToReceiveHit)
                {
                    if (animator.GetBool("Hit1"))
                    {
                        animator.Play("Hit2", 0);
                        animator.SetBool("Hit1", false);
                    }
                    else {
                        animator.Play("Hit1", 0);
                        animator.SetBool("Hit1", true);
                    }
                }
                
                takeDamage.Invoke(-damage);
            }

        }

        public override void Die(string animatorStringDeath)
        {
            base.Die(animatorStringDeath);

            //enemy
            NavMeshAgent thisNavMesh = GetComponent<NavMeshAgent>();
            if (thisNavMesh != null)
            {
                print("Dieee");
                thisNavMesh.enabled = false;
            }
            else
            {
                GetComponent<NavMeshObstacle>().enabled = false;
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

    }
}
