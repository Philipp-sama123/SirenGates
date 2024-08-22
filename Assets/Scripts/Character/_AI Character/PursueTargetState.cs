using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // 1st Option
            // Performance better but weird results ToDo: INVESTIGATE
            // aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);
            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            // 2nd Option
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            // ToDo: fix this somehow like this(!)
            // float agentSpeed = Mathf.Abs(aiCharacter.navMeshAgent.velocity.magnitude);
            // Debug.LogWarning("agent speeeeed - " + agentSpeed);
            // aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 1, false);
            return this;
            // make sure NavMeshAgent isActive, if not enable it 
            // if within combat range -> switch to combat stance 
            // if the target is not reachable
        }
    }
}