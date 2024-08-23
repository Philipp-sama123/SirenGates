using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "A.I/States/Attack State")]
    public class AttackState : AIState
    {
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo = false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("Pivot After Attack")]
        [SerializeField] protected bool pivotAfterAttack;
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);
            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // Rotate towards while Attacking (!) 

            // Set Movement Values to 0 
            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            // Perform a Combo (!)
            if (willPerformCombo && !hasPerformedCombo)
            {
                if (currentAttack.comboAction != null)
                {
                    // ToDO: Add combo
                    // hasPerformedCombo = true;
                    // currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
                }
            }
            if (!hasPerformedAttack)
            {
                // if still recovering from an action --> wait before performing another
                if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                    return this;

                if (aiCharacter.isPerformingAction)
                    return this;

                PerformAttack(aiCharacter);
                return this;
            }
            if (pivotAfterAttack)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction((aiCharacter));
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }
        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);
            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}