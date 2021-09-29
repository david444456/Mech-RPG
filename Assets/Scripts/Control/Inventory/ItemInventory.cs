using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{

    public class ItemInventory : ScriptableObject
    {
        [SerializeField] protected int _index = 0;
        [SerializeField] protected TypeItemInventory _typeItemInventory;
        [SerializeField] protected GameObject _gameObjectToSpawn = null;
        [SerializeField] protected string _nameItem = null;
        [SerializeField] [TextArea] protected string description = null;
        [SerializeField] protected Sprite _spriteImageInInventory = null;

        public virtual WeaponConfig GetWeaponConfig() => null;

        public virtual string GetInfoItemString() => "";

        public virtual TypeArmor GetTypeItemArmor() => TypeArmor.Chest;

        public virtual int GetActualArmorItem() => 0;

        public virtual GameObject GetGameObjectToSpawn() => _gameObjectToSpawn;

        public TypeItemInventory GetTypeItemInventory() => _typeItemInventory;

        public Sprite GetSpriteInventory() => _spriteImageInInventory;

        public int GetActualIndexItem() => _index;
    }

    public enum TypeItemInventory { 
        Weapon,
        Armor
    }
}
