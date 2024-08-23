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
                //  RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET (KEEP THE STATE HERE, UNTIL A TARGET IS FOUND)
                Debug.Log("SearchingTarget ... ");
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                return this;
            }
        }
    }
}