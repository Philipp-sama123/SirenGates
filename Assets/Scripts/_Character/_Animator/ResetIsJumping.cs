using UnityEngine;

namespace KrazyKatGames
{
    public class ResetIsJumping : StateMachineBehaviour
    {
        private CharacterManager character;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }
            character.characterNetworkManager.isJumping.Value = false;
        }
    }
}