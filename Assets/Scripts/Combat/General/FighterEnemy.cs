using RPG.Attributes;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class FighterEnemy : Fighter
    {
        [Header("Enemy")]
        [SerializeField] bool moveRandomEnemy = false;
        [SerializeField] float distanceToFrontPlayer;
        HealthEnemy healthEnemy;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            healthEnemy = GetComponent<HealthEnemy>();

            EquipWeaponEnemy(currentWeaponConfig);
            damage = DamageReturn();
            damage += currentWeaponConfig.damageArea;
            if (gameManager != null)
                playerHealth = gameManager.player.GetComponent<HealthPlayer>();
            else
                playerHealth = FindObjectOfType<HealthPlayer>();
        }

        void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (healthEnemy.IsDead()) return;
            if (TimeSinceLastAttack >= timeToMove) CantMove = false;
            if (!GetIsInRange(target.transform))
            {
                if (CantMove)
                {
                    return;
                }
                mover.MoveTo(target.transform.position, 6, 0.8f);
            }
            else
            {

                mover.Cancel();
                if (EnemyBossAttack) return;
                AttackBehaviour();
            }
        }

        public override void AttackBehaviour()
        {
            base.AttackBehaviour();

            //enemy
            if (TimeSinceLastAttack >= timeBetweenAttack)
            {
                CantMove = true;
                if ((distanceToAttack > 0))
                {
                    GetComponent<Mover>().StartMoveAction(target.transform.position, 1);
                }
                else
                {
                    transform.LookAt(target.transform.position);
                }

                TriggetAttack();
                TimeSinceLastAttack = 0;

            }
            else
            {
                if (!moveRandomEnemy) return;
                print("Para moverse ramdon?");

            }

        }

        public override void Shoot(float shoutArea, bool AreaZero)
        {
            base.Shoot(shoutArea, AreaZero);
            //is enemy

            shoutArea = shoutDistance;


            if (currentWeaponConfig.HasPorjectile())
            {
                Vector3 direction;


                direction = transform.position - target.transform.position;
                LaunchProyectile(target.transform.position, damage);
            }
            else
            {
                if (Vector3.Distance(targetPosition, playerHealth.transform.position) < shoutDistance)
                {
                    print("shoot enemt");
                    damage = currentWeaponConfig.weaponDamage;
                    playerHealth.TakeDamage(this.gameObject, damage);
                }
                /*//warn enemies not attack
                if (ht.gameObject.tag == "Enemy") ht.NotAttack();

                //take damage player
                if (ht.gameObject.tag == "Player")
                {
                    ht.TakeDamage(this.gameObject, damage);
                    break;
                }*/


            }
        }

        public void EquipWeaponEnemy(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);

            //gun
            if (weapon.HasPorjectile()) weapon.instigatorCharacterWeapon = gameObject;


        }

    }
}
