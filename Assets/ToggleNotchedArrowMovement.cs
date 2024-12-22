using UnityEngine;

namespace KrazyKatGames
{
    public class ToggleNotchedArrowMovement : StateMachineBehaviour
    {
        PlayerManager playerManager;
        [SerializeField] bool allowMovement = true;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (playerManager == null)
                playerManager = animator.GetComponent<PlayerManager>();
            if (playerManager == null)
                return;

            playerManager.playerLocomotionManager.canMove = allowMovement;
            playerManager.playerLocomotionManager.canRotate = allowMovement;
            playerManager.playerLocomotionManager.canRun = !allowMovement;
            playerManager.isPerformingAction = !allowMovement;
        }
    }
}