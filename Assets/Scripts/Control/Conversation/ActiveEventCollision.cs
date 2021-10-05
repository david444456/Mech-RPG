using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveEventCollision : MonoBehaviour
{
    [SerializeField] UnityEvent EventActiveCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            EventActiveCollision.Invoke();
        }
    }
}
