using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerEnergy : MonoBehaviour
    {
        [SerializeField] private float stamina = 50f;
        [SerializeField] private float multiplicatorStaminaAcelerator = 1f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] Slider sliderStamina;

        FighterPlayer fighterPlayer;
        private float _acelerationStaminaIncrease = 0;

        void Start()
        {
            fighterPlayer = GetComponent<FighterPlayer>();
            sliderStamina.maxValue = maxStamina;

            fighterPlayer.EventAttackCostStamina += augmentStamina;
        }

        private void Update() {
            if (stamina <= 100)
            {
                _acelerationStaminaIncrease += Time.deltaTime * multiplicatorStaminaAcelerator;
                stamina += _acelerationStaminaIncrease;
                sliderStamina.value = stamina;
            }
        }

        public bool CanAttackCostStamina(float costStamina) => stamina > costStamina;

        public void augmentStamina(float value)
        {
            if ((stamina >= 100 && value > 0) ||
                (stamina <= 0 && value < 0))
                return;

            if ((value + stamina) >= 100) stamina = 100;
            if ((value + stamina) < 0) stamina = 0;
            else stamina += value;


            //for aceleration energy
            _acelerationStaminaIncrease = 0;


            sliderStamina.value = stamina;
        }
    }
}
