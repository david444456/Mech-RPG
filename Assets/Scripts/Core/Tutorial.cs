using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] GameObject Tuto1;
        [SerializeField] GameObject Tuto2;
        [SerializeField] GameObject Tuto4;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                Tuto1.SetActive(false);
            }
            if (Tuto1.activeSelf) return;
            if (!Tuto1.activeSelf && Input.GetKeyDown(KeyCode.W)) {
                Tuto2.SetActive(false);
            }
            if (Tuto2.activeSelf) return;
            if (Input.GetKeyDown(KeyCode.F)) {
                Tuto4.SetActive(false);
            }
        }
    }
}
