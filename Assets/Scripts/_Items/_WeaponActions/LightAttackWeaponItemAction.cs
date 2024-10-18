using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        // Main Hand
        [Header("Main Hand Light Attacks")]
        [SerializeField] private string light_Attack_01 = "Main_Light_Attack_01"; // Main Hand (Right) Light Attack
        [SerializeField] private string light_Attack_02 = "Main_Light_Attack_02";
        [SerializeField] private string light_Attack_03 = "Main_Light_Attack_03";
        [SerializeField] private string light_Attack_04 = "Main_Light_Attack_04";

        [Header("Main Hand Running Attacks")]
        [SerializeField] private string run_attack_01 = "Main_Run_Attack_01";

        [Header("Main Hand Rolling Attacks")]
        [SerializeField] private string roll_attack_01 = "Main_Roll_Attack_01";

        [Header("Main Hand Backstep Attacks")]
        [SerializeField] private string backstep_attack_01 = "Main_Backstep_Attack_01";

        // Two Hand
        [Header("Main Hand Light Attacks")]
        [SerializeField] private string th_light_Attack_01 = "TH_Light_Attack_01"; // Main Hand (Right) Light Attack
        [SerializeField] private string th_light_Attack_02 = "TH_Light_Attack_02";
        [SerializeField] private string th_light_Attack_03 = "TH_Light_Attack_03";
        [SerializeField] private string th_light_Attack_04 = "TH_Light_Attack_04";

        [Header("Two Hand Running Attacks")]
        [SerializeField] private string th_run_attack_01 = "TH_Run_Attack_01";

        [Header("Two Hand Rolling Attacks")]
        [SerializeField] private string th_roll_attack_01 = "TH_Roll_Attack_01";

        [Header("Two Hand Backstep Attacks")]
        [SerializeField] private string th_backstep_attack_01 = "TH_Backstep_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (!playerPerformingAction.IsOwner)
                return;

            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetworkManager.isAttacking.Value = true;

            if (playerPerformingAction.playerNetworkManager.isSprinting.Value)
            {
                PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }
            if (playerPerformingAction.playerCombatManager.canPerformRollingAttack)
            {
                PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }
            if (playerPerformingAction.playerCombatManager.canPerformBackstepAttack)
            {
                PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            playerPerformingAction.characterCombatManager.AttemptCriticalAttack();

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.characterCombatManager.DisableCanDoBackstepAttack();

            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack_01,
                    th_backstep_attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack_01,
                    backstep_attack_01, true);
            }
        }
        private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.characterCombatManager.DisableCanDoRollingAttack();

            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack_01,
                    th_roll_attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack_01,
                    roll_attack_01, true);
            }
        }
        private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01,
                    th_run_attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01,
                    run_attack_01, true);
            }
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandLightAttack(playerPerformingAction, weaponPerformingAction);
            }
        }
        private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02,
                        light_Attack_02, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack03,
                        light_Attack_03, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_03)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack04,
                        light_Attack_04, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction) // remove this if you want to attack while doing an action
                // also ToDo here: dodge attack, jump attack 
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01,
                    light_Attack_01, true);
            }
        }
        private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02,
                        th_light_Attack_02, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_light_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack03,
                        th_light_Attack_03, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_light_Attack_03)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack04,
                        th_light_Attack_04, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction) // remove this if you want to attack while doing an action
                // also ToDo here: dodge attack, jump attack 
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01,
                    th_light_Attack_01, true);
            }
        }
    }
}