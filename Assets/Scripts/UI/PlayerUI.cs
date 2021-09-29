using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Image imageDeadButton;

    [SerializeField] Text descriptionInventory;

    [Header("Weapon ")]
    [SerializeField] float[] valueCooldown = null;
    [SerializeField] float[] timeCooldown = null;
    [SerializeField] Slider[] sliderCooldown;
    [SerializeField] Image[] imageWeaponsSlot;
    [SerializeField] Image imageWeaponBackGround;
    [SerializeField] RectTransform imageWeaponCornerActualWeapon;

    [Header("Menu Dead")]
    [SerializeField] Image resumeWMenu;
    [SerializeField] Image backStartMenu;
    [SerializeField] Text textDead = null;

    #region Ability cooldown

    public void UpdateCooldownValue()
    {
        //sliderLeaf[0].value = 
        for (int i = 0; i < timeCooldown.Length; i++)
        {
            timeCooldown[i] += Time.deltaTime;
            sliderCooldown[i].value = timeCooldown[i];
        }
    }

    public void SetTimeValueCooldownByIndex(int index, float newValue)
    {
        timeCooldown[index] = newValue;
    }

    public bool GetTimeAbilityByIndexIsMostBigThanValueCooldown(int index) {
        return timeCooldown[index] >= valueCooldown[index];
    }

    public float GetValueCooldownByIndex(int index) {
        return valueCooldown[index];
    }

    #endregion

    #region Weapons
    public void ChangeActualWeapon(int index) {
        switch (index) {
            case 0:
                imageWeaponCornerActualWeapon.anchoredPosition = new Vector3(-125,0,0);
                break;
            case 1:
                imageWeaponCornerActualWeapon.anchoredPosition = new Vector3(0, 0, 0);
                break;
            case 2:
                imageWeaponCornerActualWeapon.anchoredPosition = new Vector3(125, 0, 0);
                break;
        }
    }

    public void ChangeImagesIfIUseJoystick() {
        imageDeadButton.sprite = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.spriteRJoystick;

        //menu
        resumeWMenu.sprite = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.fire1;
        backStartMenu.sprite = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.spriteEJoystick;
    }

    public void ChangeValueUIWeapon(WeaponConfig weapon) {
        //change value UI
        valueCooldown = weapon.CooldownAbilities;
        imageWeaponBackGround.sprite = weapon.spriteWeapon;
        for (int i = 0; i < valueCooldown.Length; i++)
        {
            sliderCooldown[i].maxValue = valueCooldown[i];
        }
    }

    public void ChangeImageWeaponSlot(int index, Sprite sprite) {
        imageWeaponsSlot[index].sprite = sprite;
    }

    #endregion

    public void UpdateTextInventoryStatsPlayer(float damage, float health, float armor) {
        descriptionInventory.text = "Damage: " + damage + "\n" +
                                    "Health: " + health + "\n" +
                                    "Armor: " + armor;
    }

    public void TextDeadEnemy(string instigatorString, string stringDead)
    {
        textDead.text = instigatorString + ": \n" + stringDead;
    }


}
