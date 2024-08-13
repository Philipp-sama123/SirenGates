using UnityEngine;

namespace KrazyKatgames
{
    public class PlayerUiHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar healthBar;

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
        public void SetNewHealthValue(float oldValue, float newValue)
        {
            healthBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxHealthValue(int maxStamina)
        {
            healthBar.SetMaxStat(maxStamina);
        }
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
    }
}