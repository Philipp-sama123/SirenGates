using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace KrazyKatgames
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        public PlayerManager player;

        PlayerControls playerControls;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVertical_Input;
        public float cameraHorizontal_Input;

        [Header("LockOn Input")]
        [SerializeField] bool lockOn_Input = false;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float vertical_Input;
        public float horizontal_Input;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] private bool dodge_Input = false;
        [SerializeField] private bool sprint_Input = false;
        [SerializeField] private bool jump_Input = false;
        [SerializeField] private bool RB_Input = false;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //  WHEN THE SCENE CHANGES, RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //  IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE OUR PLAYERS CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            //  OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYERS CONTROLS
            //  THIS IS SO OUR PLAYER CANT MOVE AROUND IF WE ENTER THINGS LIKE A CHARACTER CREATION MENU ECT
            else
            {
                instance.enabled = false;
                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                // LockOn Input
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;

                // Hold Input Action --> set bool to false
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                // Release Input Action --> set bool to false
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
            }

            playerControls.Enable();
        }
        private void Update()
        {
            HandleAllInput();
        }
        private void HandleAllInput()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleJumpInput();
            HandleSprinting();
            HandleRBInput();
            HandleLockOnInput();
        }
        private void HandleLockOnInput()
        {
            //  CHECK FOR DEAD TARGET
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;
 
                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                //  ATTEMPT TO FIND NEW TARGET
            }


            if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                //  DISABLE LOCK ON
                return;
            }

            if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;

                //  IF WE ARE AIMING USING RANGED WEAPONS RETURN (DO NOT ALLOW LOCK WHILST AIMING)

                PlayerCamera.instance.HandleLocatingLockOnTargets();
            }
        }
        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;
                //ToDo: If UI Window open return (!)

                player.playerNetworkManager.SetCharacterActionHand(true); // Right Weapon because --> right bumper 
                // ToDo: if 2 handed --> 2 Handed Action 

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandWeapon
                );
            }
        }
        private void HandleJumpInput()
        {
            if (jump_Input)
            {
                jump_Input = false;
                // Attempt To Perform Jump
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleSprinting()
        {
            if (sprint_Input)
            {
                // Handle Sprinting
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
        private void OnDestroy()
        {
            //  IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled)
            {
                if (hasFocus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        private void HandleMovementInput()
        {
            vertical_Input = movementInput.y;
            horizontal_Input = movementInput.x;

            //  RETURNS THE ABSOLUTE NUMBER, (Meaning number without the negative sign, so its always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

            //  WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (OPTIONAL)
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if (player == null)
                return;

            //  IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

            //  IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL
        }
        private void HandleCameraMovementInput()
        {
            cameraVertical_Input = cameraInput.y;
            cameraHorizontal_Input = cameraInput.x;
        }
        private void HandleDodgeInput()
        {
            if (dodge_Input)
            {
                dodge_Input = false;
                // Dont Dodge While Menu is open
                // Perform Dodge
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
    }
}