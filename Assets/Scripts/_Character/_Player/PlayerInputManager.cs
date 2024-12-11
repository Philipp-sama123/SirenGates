using UnityEngine;
using UnityEngine.SceneManagement;

namespace KrazyKatGames
{
    public class PlayerInputManager : MonoBehaviour
    {
        //  INPUT CONTROLS
        private PlayerControls playerControls;

        //  SINGLETON
        public static PlayerInputManager instance;

        //  LOCAL PLAYER
        public PlayerManager player;

        [Header("Camera Movement Inputs")]
        [SerializeField] Vector2 camera_Input;
        public float cameraVertical_Input;
        public float cameraHorizontal_Input;

        [Header("LockOn Inputs")]
        [SerializeField] bool lockOn_Input;
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        private Coroutine lockOnCoroutine;

        [Header("Movement Inputs")]
        [SerializeField] Vector2 movementInput;
        public float vertical_Input;
        public float horizontal_Input;
        public float moveAmount;

        [Header("Action Inputs")]
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool interaction_Input = false;
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;

        [Header("Bumper Inputs")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool LB_Input = false;

        [SerializeField] bool Hold_RB_Input = false;
        [SerializeField] bool Hold_LB_Input = false;

        [Header("Trigger Inputs")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

        [SerializeField] bool LT_Input = false;

        [Header("Two Hand Inputs")]
        [SerializeField] bool two_Hand_Input = false;
        [SerializeField] bool two_Hand_Right_Weapon_Input = false;
        [SerializeField] bool two_Hand_Left_Weapon_Input = false;

        [Header("Input queue")]
        [SerializeField] private bool input_Que_Is_Active = false;
        [SerializeField] float que_Input_Timer = 0.0f;
        [SerializeField] float default_Que_Input_Time = 0.25f;

        [SerializeField] bool que_RB_Input = false;
        [SerializeField] bool que_RT_Input = false;

        [Header("UI Inputs")]
        [SerializeField] bool closeMenuInput = false;
        [SerializeField] bool openMenuInput = false;


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
                // Movement
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => camera_Input = i.ReadValue<Vector2>();
                // Actions
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

                // Bumpers
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;
                // hold
                playerControls.PlayerActions.Hold_RB.performed += i => Hold_RB_Input = true;
                playerControls.PlayerActions.Hold_RB.canceled += i => Hold_RB_Input = false;
                playerControls.PlayerActions.Hold_LB.performed += i => Hold_LB_Input = true;
                playerControls.PlayerActions.Hold_LB.canceled += i => Hold_LB_Input = false;

                // Triggers
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.LT.performed += i => LT_Input = true;

                // Interactions
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;

                // Two Handing 
                playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
                playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;

                playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;

                playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;

                // Lock On
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.LockOnLeft.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.LockOnRight.performed += i => lockOn_Right_Input = true;

                //  HOLDING THE INPUT, SETS THE BOOL TO TRUE
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                playerControls.PlayerActions.Hold_RT.performed += i => Hold_RT_Input = true;
                //  RELEASING THE INPUT, SETS THE BOOL TO FALSE
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
                playerControls.PlayerActions.Hold_RT.canceled += i => Hold_RT_Input = false;

                // Queued Inputs
                playerControls.PlayerActions.Que_RB.performed += i => QueInput(ref que_RB_Input);
                playerControls.PlayerActions.Que_RB.performed += i => QueInput(ref que_RT_Input);

                // UI Inputs 
                playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
                playerControls.PlayerActions.OpenCharacterMenu.performed += i => openMenuInput = true;
            }

            playerControls.Enable();
        }
        private void OnDestroy()
        {
            //  IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        //  IF WE MINIMIZE OR LOWER THE WINDOW, STOP ADJUSTING INPUTS
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();

            HandlePlayerMovementInput();
            HandleCameraMovementInput();

            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleInteractionInput();

            HandleRBInput();
            HandleLBInput();

            HandleHoldRBInput();
            HandleHoldLBInput();

            HandleRTInput();
            HandleChargeRTInput();

            HandleLTInput();

            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();

            HandleTwoHandInput();

            // TODO when this is active the 3rd Combo goes in the charged attack (!)
            // HandleQuedInputs();

            HandleCloseUIInput();
            HandleOpenCharacterMenuInput();
        }
        private void HandleHoldRBInput()
        {
            if (Hold_RB_Input)
            {
                player.playerNetworkManager.isChargingRightSpell.Value = true;
                player.playerNetworkManager.isHoldingArrow.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingRightSpell.Value = false;
                player.playerNetworkManager.isHoldingArrow.Value = false;
            }
        }
        private void HandleHoldLBInput()
        {
            if (Hold_LB_Input)
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = false;
            }
        }
        // Input Queing
        private void QueInput(ref bool quedInput)
        {
            que_RB_Input = false;
            que_RT_Input = false;
            // que_LT_Input = false; 
            // que_LB_Input = false; 

            // Check for open UI Windows (!)
            if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
            {
                quedInput = true;
                // Attempt this new Input for x amount of time
                que_Input_Timer = default_Que_Input_Time;
                input_Que_Is_Active = true;
            }
        }
        private void ProcessQuedInputs()
        {
            if (player.isDead.Value)
                return;

            if (que_RB_Input)
                RB_Input = true;

            if (que_RT_Input)
                RT_Input = true;
        }
        private void HandleQuedInputs()
        {
            if (input_Que_Is_Active)
            {
                // While time > 0, keep attempting to press the input
                if (que_Input_Timer > 0)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInputs();
                }
                else
                {
                    // Reset Qued Inputs
                    que_RB_Input = false;
                    que_RT_Input = false;

                    input_Que_Is_Active = false;
                }
            }
        }

        // Two Handing Weapons 
        private void HandleTwoHandInput()
        {
            if (!two_Hand_Input)
                return;

            if (two_Hand_Right_Weapon_Input)
            {
                RB_Input = false;
                two_Hand_Right_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;
                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    // --> trigger "onValueChanged" function which un-two-hands the current weapon
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // --> trigger "onValueChanged" function which triggers a function to two hand right weapon
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    return;
                }
            }
            if (two_Hand_Left_Weapon_Input)
            {
                LB_Input = false;
                two_Hand_Left_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    // --> trigger "onValueChanged" function which un-two-hands the current weapon
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // --> trigger "onValueChanged" function which triggers a function to two hand right weapon
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    return;
                }
            }
        }
        //  LOCK ON
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

                //  THIS ASSURES US THAT THE COROUTINE NEVER RUNS MUILTPLE TIMES OVERLAPPING ITSELF
                if (lockOnCoroutine != null)
                    StopCoroutine(lockOnCoroutine);

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }


            if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
                //  DISABLE LOCK ON
                return;
            }

            if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;

                //  IF WE ARE AIMING USING RANGED WEAPONS RETURN (DO NOT ALLOW LOCK WHILST AIMING)

                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.nearestLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }
        }
        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
            }
        }

        //  MOVEMENT
        private void HandlePlayerMovementInput()
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

            if (moveAmount != 0)
                player.playerNetworkManager.isMoving.Value = true;
            else
                player.playerNetworkManager.isMoving.Value = false;

            //  IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT

            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input,
                    player.playerNetworkManager.isSprinting.Value);
            }

            //  IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL
        }
        private void HandleCameraMovementInput()
        {
            cameraVertical_Input = camera_Input.y;
            cameraHorizontal_Input = camera_Input.x;
        }

        //  ACTION
        private void HandleDodgeInput()
        {
            if (dodge_Input)
            {
                dodge_Input = false;

                //  FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;

                player.playerInteractionManager.Interact();
            }
        }
        private void HandleSprintInput()
        {
            if (sprint_Input)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
        private void HandleJumpInput()
        {
            if (jump_Input)
            {
                jump_Input = false;

                //  IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING
                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                //  ATTEMPT TO PERFORM JUMP
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleRBInput()
        {
            if (two_Hand_Input)
                return;

            if (RB_Input)
            {
                RB_Input = false;

                //  TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(true);

                //  TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }
        private void HandleLBInput()
        {
            if (two_Hand_Input)
                return;

            if (LB_Input)
            {
                LB_Input = false;

                //  TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(false);

                //  TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action,
                    player.playerInventoryManager.currentLeftHandWeapon);
            }
        }
        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                //  TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(true);

                //  TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }
        private void HandleChargeRTInput()
        {
            if (player.isPerformingAction)
            {
                if (player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
        }
        private void HandleLTInput()
        {
            if (LT_Input)
            {
                LT_Input = false;


                WeaponItem weaponPerformingAshOfWar = player.playerCombatManager.SelectWeaponToPerformAshOfWar();

                weaponPerformingAshOfWar.ashOfWarAction.AttemptToPerformAction(player);
            }
        }
        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                //  IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING
                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                switch_Right_Weapon_Input = false;
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }
        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                //  IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING
                if (PlayerUIManager.instance.menuWindowIsOpen)
                    return;

                switch_Left_Weapon_Input = false;
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }
        private void HandleCloseUIInput()
        {
            if (closeMenuInput)
            {
                closeMenuInput = false;
                if (PlayerUIManager.instance.menuWindowIsOpen)
                {
                    PlayerUIManager.instance.CloseAllMenuWindows();
                }
            }
        }
        private void HandleOpenCharacterMenuInput()
        {
            if (openMenuInput)
            {
                openMenuInput = false;

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();
                PlayerUIManager.instance.CloseAllMenuWindows();

                PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
            }
        }
    }
}