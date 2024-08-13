using Unity.Collections;
using Unity.Netcode;

namespace KrazyKatgames
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;
        public NetworkVariable<FixedString64Bytes> characterName =
            new("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUiHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newEndurance);
            PlayerUIManager.instance.playerUiHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }
    }
}