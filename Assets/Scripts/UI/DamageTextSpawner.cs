using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText = null;
        [SerializeField] Transform transformCamera;
        [SerializeField] float timeToDestroy = 3f;


        /// <summary>
        /// Spawn damage text in the gameobject
        /// </summary>
        /// <param name="damageAmount"></param>
        public void Spawn(float damageAmount) {
            DamageText instance=  Instantiate<DamageText>(damageText, transform);
            instance.SetValue(damageAmount);
            Destroy(instance.gameObject, timeToDestroy);
        }
    }
}