using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Combat Stance State")]
    public class CombatStanceState : AIState
    {
        //  1. Select an Attack for the Attack State, depending on distance and Angle of the target in relation to the Character
        //  2. Process any Combat Logic here (Blocking, strafing and dodging) 

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;
        public List<AICharacterAttackAction> potentialAttacks;

        public AICharacterAttackAction choosenAttack;
        public AICharacterAttackAction previousAttack;

        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int chanceToPerformCombo = 25;
        [SerializeField] protected bool hasRolledForComboChance = false;


        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5; // Distance away from target before entering pursue state


        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                // if face and turn towards its target include this 
                if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    {
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                    }
                }
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            //  4. If the target is no longer present, switch to idle state 
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                return SwitchState(aiCharacter, aiCharacter.attack);
                // check Recovery Timer, 
                // Roll for Combo Chance 
            }

            //  3. If target moves out of combat range, switch to pursue state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);
            return this;
        }

        /**
         *      1. Sort through possible Attacks
         *      2. Remove Attacks that can not be used in this situation (based on angle and distance)
         *      3. Place remaining attacks into a List
         *      4. pick one of the remaining attacks randomly, based on weight
         *      5. Select this attack and pass it to the attack state
         */
        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();
            foreach (var potentialAttack in aiCharacterAttacks)
            {
                // character is too close, check next attack
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                // character is too far away, check next attack
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                // character outside min FOV, check next attack
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                // character outside max FOV, check next attack
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);
            }
            if (potentialAttacks.Count <= 0)
            {
                Debug.LogWarning("No Potential Attack found!");
                return;
            }
            var totalWeight = 0;
            foreach (var potentialAttack in potentialAttacks)
            {
                totalWeight += potentialAttack.attackWeight;
            }
            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var potentialAttack in potentialAttacks)
            {
                processedWeight += potentialAttack.attackWeight;

                // this is the actual attack (!)
                if (randomWeightValue <= processedWeight)
                {
                    choosenAttack = potentialAttack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerfomed = false;
            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
                outcomeWillBePerfomed = true;

            return outcomeWillBePerfomed;
        }
        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasRolledForComboChance = false;
            hasAttack = false;
        }
    }
}