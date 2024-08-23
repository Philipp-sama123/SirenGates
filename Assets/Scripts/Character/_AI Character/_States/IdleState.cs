using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "A.I/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                //  Return the Pursue Target
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }
            else
            {
                // stay in this State until a target is found
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                return this;
            }
        }
    }
}