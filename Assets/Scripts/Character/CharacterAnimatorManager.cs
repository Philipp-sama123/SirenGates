using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;
        if (isSprinting)
        {
            verticalAmount = 2f;
        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }
    public virtual void PlayTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        //  CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
        //  FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        //  THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        //  WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        //  TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation,
            applyRootMotion);
    }

    protected virtual void OnAnimatorMove()
    {
        if (!character.applyRootMotion) return;

        Debug.Log("character.applyRootMotion" + character.applyRootMotion);
        Vector3 velocity = character.animator.deltaPosition;
        character.characterController.Move(velocity);
        character.transform.rotation *= character.animator.deltaRotation;
    }
}