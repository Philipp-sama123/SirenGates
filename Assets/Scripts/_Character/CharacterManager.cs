using System;
using System.Collections;
using System.Collections.Generic;
using HoaxGames;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterUIManager characterUIManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;

        [Header("Flags")]
        public bool isPerformingAction = false;

        [Header("Character Group")]
        public CharacterGroup characterGroup;

        public FootIK footIK;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterUIManager = GetComponent<CharacterUIManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();

            footIK = GetComponent<FootIK>();
        }
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected virtual void FixedUpdate()
        {
        }
        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", characterLocomotionManager.isGrounded);
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

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // if in air -- air death animation()!
                if (!manuallySelectDeathAnimation && !characterNetworkManager.isBeingCriticallyDamaged.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
                }
                yield return new WaitForSeconds(5);

                // Award Player with Runes (!)
            }
        }

        public virtual void ReviveCharacter()
        {
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }
            ignoreColliders.Add(characterControllerCollider);


            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            animator.SetBool("IsMoving", characterNetworkManager.isMoving.Value);
            characterNetworkManager.OnIsActiveChanged(false, characterNetworkManager.isActive.Value);

            isDead.OnValueChanged += characterNetworkManager.OnIsDeadChanged;
            characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
            characterNetworkManager.isActive.OnValueChanged += characterNetworkManager.OnIsActiveChanged;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isDead.OnValueChanged -= characterNetworkManager.OnIsDeadChanged;
            characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
            characterNetworkManager.isActive.OnValueChanged -= characterNetworkManager.OnIsActiveChanged;
        }
    }
}