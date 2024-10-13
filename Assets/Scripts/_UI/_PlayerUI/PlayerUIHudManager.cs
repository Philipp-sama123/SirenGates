using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatgames
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] canvasGroups;

        [Header("Stat Bars")]
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar healthBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;


        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;


        public void ToggleHUD(bool status)
        {
            // ToDo: Fade in and out (!)
            if (status)
            {
                foreach (var canvasGroup in canvasGroups)
                {
                    canvasGroup.alpha = 1;
                }
            }
            else
            {
                foreach (var canvasGroup in canvasGroups)
                {
                    canvasGroup.alpha = 0;
                }
            }
        }
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }
        public void SetMaxHealthValue(int maxStamina)
        {
            healthBar.SetMaxStat(maxStamina);
        }
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is null!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }
        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is null!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }
}