using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] private string heavy_Attack_01 = "Main_Heavy_Attack_01"; // Main Hand (Right) Heavy Attack
        [SerializeField] private string heavy_Attack_02 = "Main_Heavy_Attack_02"; // Main Hand (Right) Heavy Attack
        [SerializeField] private string heavy_Attack_03 = "Main_Heavy_Attack_03"; // Main Hand (Right) Heavy Attack
        [SerializeField] private string heavy_Attack_04 = "Main_Heavy_Attack_04"; // Main Hand (Right) Heavy Attack
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            
            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            
            if (!playerPerformingAction.playerLocomotionManager.isGrounded) 
                return;
            
            if (playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetworkManager.isAttacking.Value = true;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction,AttackType.HeavyAttack02, heavy_Attack_02, true);
                }
                // else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_02)
                // {
                //     playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack03, heavy_Attack_03, true);
                // }
                // else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_03)
                // {
                //     playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack04, heavy_Attack_04, true);
                // }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction,AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }
    }
}