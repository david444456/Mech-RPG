using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DestroyAfterAnimation : MonoBehaviour
    {
        void DestroyObject() {
            Destroy(gameObject);
        }

        void DesactiveObject() {
            gameObject.SetActive(false);
        }
    }
}
