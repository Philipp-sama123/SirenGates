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
            character.applyRootMotion = false;
            character.canRotate = true;
            character.canMove = true;
            
            // ToDo: maybe remove
            character.isJumping = false;
        }
    }
}