using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Abilities : MonoBehaviour, ISaveable
    {
        public bool[] AbilitiesPlayer;

        public object CaptureState()
        {
            return AbilitiesPlayer;
        }

        public void RestoreState(object state)
        {
            AbilitiesPlayer = (bool[])state;
        }
    }
}
