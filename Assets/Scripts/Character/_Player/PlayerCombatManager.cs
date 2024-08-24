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

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;
        // public bool canComboWithOffHandWeapon = false; 
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
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

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);
            if (IsOwner)
            {
                PlayerCamera.instance.SetLockCameraHeight();
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
            Debug.LogWarning("For " + player.name + " staminaDeducted -" + staminaDeducted);
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }
    }
}