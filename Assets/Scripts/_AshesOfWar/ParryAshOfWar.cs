using System;
using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Items/Ash of War/Parry")]
    public class ParryAshOfWar : AshOfWar
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction);

            if (!CanIUseThisAbility(playerPerformingAction))
                return;

            DeductFocusPointCost(playerPerformingAction);
            DeductStaminaCost(playerPerformingAction);
            PerformParryTypeBasedOnWeapon(playerPerformingAction);
        }
        private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
        {
            WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;
            string animationToPlay = "Slow_Parry_01";
           
            switch (weaponBeingUsed.weaponClass)
            {
                case WeaponClass.Blade:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.StraightSword:
                    break;
                case WeaponClass.Fist:
                    break;
                case WeaponClass.LightShield:
                    animationToPlay = "Fast_Parry_01";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation(animationToPlay, true);
        }

        public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
            {
                Debug.LogWarning("can not perform AshOfWar (!) isPerformingAction (!)");
                return false;
            }
            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
            {
                Debug.LogWarning("can not perform AshOfWar (!) Jumping (!)");
                return false;
            }
            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                Debug.LogWarning("can not perform AshOfWar (!) is not GROUNDED (!)");
                return false;
            }
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                Debug.LogWarning("can not perform AshOfWar (!) out of STAMINA (!)");
                return false;
            }
            return true;
        }
    }
}