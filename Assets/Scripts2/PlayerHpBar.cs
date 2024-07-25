using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerHud : MonoBehaviour
{
    public Slider hpSlider;
    public TMP_Text idText;
    public Wizard wizard;

#if !UNITY_SERVER
    private void Update()
    {
        hpSlider.value = wizard.hp / wizard.maxHp;
        idText.SetText("ID: {0}", wizard.netId);
    }
#endif
}