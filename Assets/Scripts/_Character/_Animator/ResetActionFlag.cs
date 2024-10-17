using UnityEngine;

namespace KrazyKatgames
{
    public class ResetActionFlag : StateMachineBehaviour
    {
        private CharacterManager character;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }
            //  THIS IS CALLED WHEN AN ACTION ENDS, AND THE STATE RETURNS TO "EMPTY"
            character.isPerformingAction = false;

            character.characterAnimatorManager.applyRootMotion = false;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.isRolling = false;

            character.characterCombatManager.DisableCanDoCombo();
            character.characterCombatManager.DisableCanDoBackstepAttack();
            character.characterCombatManager.DisableCanDoRollingAttack();

            if (character.IsOwner)
            {
                character.characterNetworkManager.isJumping.Value = false;
                character.characterNetworkManager.isInvulnerable.Value = false;
                character.characterNetworkManager.isAttacking.Value = false;
                character.characterNetworkManager.isRipostable.Value = false;
            }
        }
    }
}