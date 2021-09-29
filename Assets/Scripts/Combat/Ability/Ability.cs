using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability
{
    public abstract class Ability : MonoBehaviour, IAbility
    {
        public abstract void OnActivatedAbility();
    }

    public interface IAbility {
        void OnActivatedAbility();
    }

}