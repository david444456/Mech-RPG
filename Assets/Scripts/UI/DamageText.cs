using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText = null;
    [SerializeField] Color colorHardDamage;

    public void DestroyText() {
        Destroy(gameObject);
    }

    public void SetValue(float amount) {
        //set the text
        if (amount > 0) damageText.text = "+" + Mathf.Round(amount).ToString() + " exp";
        else if (amount < -60) {
            damageText.text = Mathf.CeilToInt(amount).ToString();
            damageText.color = colorHardDamage;
            damageText.fontSize = 50;
        }
        else damageText.text = Mathf.CeilToInt(amount).ToString();

        //orientation
        //transform.rotation = transformCamera.rotation;

    }

}
