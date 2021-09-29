using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class FighterPlayer : Fighter
    {
        public Action<float> EventAttackCostStamina = delegate { }; 

        [Tooltip("Weapons player, in game set for GameManager. Please use only three")]
        public WeaponConfig[] weaponsEquip;

        [Tooltip("Using in game, not HideInInspector to do internal testing. Not attacking")]
        public bool attacking = false;
        [SerializeField] float timeBetweenCombo = 0.1f;
        [SerializeField] float _costBasicAttack = 5;

        [HideInInspector] public int indexWeaponActual = 0;

        RaycastHit[] hits;
        bool AttackInSphere = false;
        float gunDistance = 0;
        PlayerController playerController;
        PlayerUI playerUI;
        PlayerEnergy playerEnergy;
        Movement.MoverPlayer moverPlayer;

        //combat
        KeyCode[] key = { KeyCode.Q, KeyCode.W };
        bool WeaponInAbility = false;

        public override void Start()
        {
            playerHealth = GetComponent<HealthPlayer>();
            playerUI = GetComponent<PlayerUI>();
            moverPlayer = GetComponent<Movement.MoverPlayer>();
            animator = GetComponent<Animator>();
            playerEnergy = GetComponent<PlayerEnergy>();

            // gameManager = FindObjectOfType<PlayerInformationBetweenScenes>();
            // gameManager.UpdateThisWeapon();
            playerController = GetComponent<PlayerController>();
            //playerController.SetTheWeapon(ControlInventoryBetweenScenes.Instance.GetWeaponByIndex(0)); //= currentWeapon.value;

            base.Start();
        }

        void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;

            if (Input.GetButtonDown("Fire4") && playerHealth.IsDead())
            {
                GetComponentInParent<LevelManager>().RestartScenes();
            }


        }

        public override void Shoot(float shoutArea, bool AreaZero)
        {
            damage = DamageReturn();
            //if attack sphere
            AttackInSphere = currentWeaponConfig.AttackAreaWeapon();

            //cost attack
            EventAttackCostStamina.Invoke(-_costBasicAttack);

            //??
            if (AreaZero)
            {
                targetPosition = transform.position;
                shoutArea += shoutDistance;
            }
            else
            {
                shoutArea = weaponsEquip[indexWeaponActual].GetRange();
            }

            base.Shoot(shoutArea, AreaZero);

            if (currentWeaponConfig.HasPorjectile())
            {
                print(shoutArea + " 1");
                Vector3 direction;
                if (playerController != null)
                {
                    if (playerController.moveWithUPDOWN)
                    {
                        //if move with UPDown, then shoot forward
                        direction = transform.forward * gunDistance + transform.localPosition;
                        LaunchProyectile(direction, currentWeaponConfig.weaponDamage + damage);
                        return;
                    }
                }
            }
            else if (AttackInSphere)
            {
                print(shoutArea + " 2");
                //sphere raycast with targetposition
                hits = Physics.SphereCastAll(AttackAreaTF.position, shoutArea, Vector3.up, 0);
                damage += currentWeaponConfig.damageArea;
                foreach (RaycastHit hit in hits)
                {
                    Health ht = hit.collider.GetComponent<Health>();
                    if (ht == null) continue;
                    if (ht.IsDead()) continue;
                    if (tag == ht.gameObject.tag) continue;
                    attacking = true;
                    ht.TakeDamage(this.gameObject, damage + SetOnHit(ht));
                }
            }
            else
            {
                print(shoutArea + " " + shoutDistance);
                hits = Physics.SphereCastAll(AttackAreaTF.position, shoutArea, Vector3.up, 1);
                foreach (RaycastHit hit in hits)
                {
                    Health ht = hit.collider.GetComponent<Health>();
                    if (ht == null) continue;
                    if (ht.IsDead()) continue;
                    if (ht.gameObject.tag == "Player") continue;
                    damage += currentWeaponConfig.weaponDamage;
                    ht.TakeDamage(this.gameObject, damage + SetOnHit(ht));
                    attacking = true;
                    return;
                }
            }
        }

        public override void AttackBehaviour()
        {
            if (TimeSinceLastAttack >= timeBetweenAttack)
            {
                TriggetAttack();

                targetPosition = AttackAreaTF.position;
                
                TimeSinceLastAttack = 0;
            }
        }

        public override void StopAttack()
        {
            animator.ResetTrigger("ComboE");
            animator.ResetTrigger("Attack");
            base.StopAttack();

        }

        public void changeWeaponIndex(WeaponConfig weapon)
        {
            weaponsEquip[indexWeaponActual] = weapon;
        }

        public void SetTheWeapon(WeaponConfig weapon)
        {
            //PlayerC
            changeWeaponIndex(weapon);

            //equip a weapon in fighter and PlayerC
            EquipWeapon(weapon, indexWeaponActual);
            weaponActual = GetWeapon();

            //ui
            playerUI.ChangeImageWeaponSlot(indexWeaponActual, weapon.spriteWeapon);
            playerUI.ChangeValueUIWeapon(weapon);

            //if using combo (weapon bool)
            if (currentWeaponConfig.AttackWithCombo)
            {
                animator.SetBool("InAbility", false);
                WeaponInAbility = false;
            }
            else
            {
                animator.SetBool("InAbility", true);
                WeaponInAbility = true;
            }
        }

        public void SetDataTheWeapon(WeaponConfig weapon, int index) => weaponsEquip[index] = weapon;

        public WeaponConfig GetWeaponByIndex(int index) => weaponsEquip[index];

        public float GetDamage() {
            float value = DamageReturn();

            if (currentWeaponConfig.AttackAreaWeapon()) value += currentWeaponConfig.damageArea;
            else value += currentWeaponConfig.weaponDamage;

            return value ; 
        }

        public bool IntectWithCombat()
        {
            playerUI.UpdateCooldownValue();

            if ((Input.GetButtonDown("Fire1")) && playerUI.GetTimeAbilityByIndexIsMostBigThanValueCooldown(0))
            {

                if (weaponsEquip[indexWeaponActual].AttackWithCombo) StartCoroutine(Combos(0.65f, key));
                animator.ResetTrigger("ComboE");
                return AbilitiesTouch(0, "attack");
            }
            if (!WeaponInAbility) return attacking;
            if (Input.GetButtonDown("Fire3") && playerUI.GetTimeAbilityByIndexIsMostBigThanValueCooldown(1))
            {
                weaponActual.OnHitE();
                return AbilitiesTouch(1, "ComboE");

            }
            if (Input.GetButtonDown("Fire4") && playerUI.GetTimeAbilityByIndexIsMostBigThanValueCooldown(2))
            {
                weaponActual.OnHitR();
                return AbilitiesTouch(2, "ComboR");

            }
            /*if (Input.GetButtonDown("Fire2") && timeCooldown[3] >= valueCooldown[3])
            {
                weaponActual.OnHitQ();
                return AbilitiesTouch(3, "ComboW");
                
            }*/

            return attacking;
        }

        public bool InteractWithSelectWeapon()
        {
            /*if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("SelectWeapon1") <= -1)
            {
                ChangeSelectWeapon(0);
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("SelectWeapon1") >= 1)
            {
                ChangeSelectWeapon(1);
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("SelectWeapon2") <= -1)
            {
                ChangeSelectWeapon(2);
                return true;
            }*/
            return false;
        }

        public void spawnActualWeaponPlayer()
        {
            if (weaponsEquip[indexWeaponActual].name == "Unarmed") return;
            Instantiate(weaponsEquip[indexWeaponActual].spawnWeaponPickup, transform.position, Quaternion.identity);
        }

        public void ChangeToUnarmedWeapon() => EquipWeapon(defaultWeapon , 0);

        private bool AbilitiesTouch(int index, string nameAnimator)
        {
            targetPosition = playerController.GetTFPosition();
            TimeSinceLastAttack = 0;

            mover.Cancel();

            float costAttackStamina = 0;

            playerUI.SetTimeValueCooldownByIndex(index, 0);

            if (index == 0)
            {
                costAttackStamina = playerUI.GetValueCooldownByIndex(index) * 3;
                EventAttackCostStamina.Invoke( (-1)* costAttackStamina);
            }
            else
            {
                costAttackStamina = playerUI.GetValueCooldownByIndex(index);
                EventAttackCostStamina.Invoke((-1) * costAttackStamina);
            }

            if (CanAttack(costAttackStamina))
            {
                SetTriggerAnimationAttack(nameAnimator);
                attacking = true;
            }
            return attacking;
        }

        private IEnumerator Combos(float time, KeyCode[] code)
        {
            float comboTime = 0;
            int timeTouchFire1 = 0;
            yield return new WaitForSeconds(timeBetweenCombo);

            while (comboTime < time)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (timeTouchFire1 == 1)
                    {
                        SetTriggerAnimationAttack("ComboE");
                    }
                    else if (timeTouchFire1 == 0)
                    {
                        SetTriggerAnimationAttack("ComboQ");
                        moverPlayer.Cancel();
                        attacking = true;
                        timeTouchFire1++;
                    }
                }
                else if (Input.GetButtonDown("Fire2") && timeTouchFire1 == 1)
                {
                    SetTriggerAnimationAttack("ComboW");
                    timeTouchFire1++;
                }


                comboTime += Time.deltaTime;
                yield return Time.deltaTime;
            }
        }

        private void ChangeSelectWeapon(int newIndex)
        {
            indexWeaponActual = newIndex;
            playerUI.ChangeActualWeapon(newIndex);
            SetTheWeapon(weaponsEquip[newIndex]);
        }

        private void SetTriggerAnimationAttack(string nameAnimation) {
            if (CanAttack(_costBasicAttack))
            {
                moverPlayer.MoveRotateToMousePosition();
                animator.SetTrigger(nameAnimation);
            }
        }

        private bool CanAttack(float costStamina) => playerEnergy.CanAttackCostStamina(Mathf.Abs( costStamina));
    }
}
