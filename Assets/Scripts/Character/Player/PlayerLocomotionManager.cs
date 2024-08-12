using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 4.5f;
    [SerializeField] float sprintingSpeed = 7.5f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("Dodge")]
    private Vector3 rollDirection;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
        //  AERIAL MOVEMENT
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;

        //  CLAMP THE MOVEMENTS
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;

        GetVerticalAndHorizontalInputs();
        //  OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERSPECTIVE & OUR MOVEMENT INPUTS
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction) return;

        if (PlayerInputManager.instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            rollDirection.y = 0;
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true, true, false);
            // Roll Animation 
        }
        else
        {
            // Perform Backstep Animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Backward_01", true, true, true, false);
        }
    }
    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            // Sprinting is false
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }
        Debug.Log("player.isPerformingAction s" + moveAmount);


        // If we are moving sprinting is true , false when stationary
        if (PlayerInputManager.instance.moveAmount >= 0.5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
        // out of stamina --> False
    }
}