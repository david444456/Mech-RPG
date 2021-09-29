using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        print(other.name);
    }
    private void OnParticleTrigger()
    {
        print("Trigger");
    }

}

