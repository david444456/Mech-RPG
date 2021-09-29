using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DesactiveAfterActive : MonoBehaviour
    {
        [SerializeField] float timeToDesactive = 2;

        private void OnEnable()
        {
            StartCoroutine(DesactiveAfterTime());
        }

        IEnumerator DesactiveAfterTime() {
            yield return new WaitForSeconds(timeToDesactive);

            DesactiveObject();
        }

        private void DesactiveObject() {
            gameObject.SetActive(false);
        }
    }
}
