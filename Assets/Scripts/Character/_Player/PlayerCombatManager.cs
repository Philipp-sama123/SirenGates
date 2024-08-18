using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatgames
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                //  PERFORM THE ACTION
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

                //  NOTIFY THE SERVER WE HAVE PERFORMED THE ACTION, SO WE PERFORM IT FROM THERE PERSPECTIVE ALSO}
                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(
                    NetworkManager.Singleton.LocalClientId,
                    weaponAction.actionID,
                    weaponPerformingAction.itemID
                );
            }
        }
        /**
         * Animation Event
         */
        public void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;
            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0f;
            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            Debug.Log("DrainStaminaBasedOnAttack - staminaDeducted -" + staminaDeducted);
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }
    }
}