using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Aim Action")]
    public class AimAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            if (playerPerformingAction.playerLocomotionManager.isRolling)
                return;

            if (playerPerformingAction.playerNetworkManager.isLockedOn.Value)
                return;


            if (playerPerformingAction.IsOwner)
            {
                if (!playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
                    {
                        playerPerformingAction.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    }
                    else if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
                    {
                        playerPerformingAction.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    }
                }
                playerPerformingAction.playerNetworkManager.isAiming.Value = true;
            }
        }
    }
}