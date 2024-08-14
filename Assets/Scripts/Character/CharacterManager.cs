using HoaxGames;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatgames
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffects;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isJumping = false;
        public bool isGrounded = false;

        public FootIK footIK;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffects = GetComponent<CharacterEffectsManager>();

            footIK = GetComponent<FootIK>();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);
            //  IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            //  IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                //  Position
                transform.position = Vector3.SmoothDamp
                (transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                //  Rotation
                transform.rotation = Quaternion.Slerp
                (transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
        }
    }
}