using HoaxGames;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -15f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (Jumping or Falling)
        [SerializeField] float groundedYVelocity = -20f; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED
        [SerializeField] float fallStartYVelocity = -5;
        // THE FORCE AT WHICH OUR CHARACTER BEGINS TO FALL WHEN THEY BECOME UNGROUNDED (RISES AS THEY FALL LONGER)
        protected bool fallingVelocityHAsBeenSet = false;
        [SerializeField] protected float inAirTimer = 0;
        public FootIK footIk;

        [Header("Flags")]
        public bool isRolling = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool canRun = true;
        public bool isGrounded = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {
            footIk = GetComponent<FootIK>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.characterLocomotionManager.isGrounded)
            {
                //  IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHAsBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //  IF WE ARE NOT JUMPING, AND OUR FALLING VELOCITY HAS NOT BEEN SET
                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHAsBeenSet)
                {
                    fallingVelocityHAsBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            //  THERE SHOULD ALWAYS BE SOME FORCE APPLIED TO THE Y VELOCITY
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            // character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);

            if (footIk&& footIk.getGroundedResult() != null)
            {
                character.characterLocomotionManager.isGrounded = footIk.getGroundedResult().isGrounded;
            }
            else
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
            }
        }

        //  DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        #region Animation Events
        public void EnableCanRotate()
        {
            canRotate = true;
        }
        public void DisableCanRotate()
        {
            canRotate = false;
        }
        #endregion
    }
}