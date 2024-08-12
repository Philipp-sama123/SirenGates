using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    private CharacterManager character;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<CharacterManager>();
        }
        Debug.Log("OnStateEnter");
        //  THIS IS CALLED WHEN AN ACTION ENDS, AND THE STATE RETURNS TO "EMPTY"
        character.isPerformingAction = false;
        character.applyRootMotion = false;
        character.canRotate = true;
        character.canMove = true;
    }
}