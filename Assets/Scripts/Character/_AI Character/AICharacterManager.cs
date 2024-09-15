using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatgames
{
    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string characterName = "";
        
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

        [Header("NavMesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] protected AIState currentState;


        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            // Fixes Character Moving when 
            if (navMeshAgent.stoppingDistance <= 0)
                navMeshAgent.stoppingDistance = 1;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsOwner)
                ProcessStateMachine();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                idle = Instantiate(idle);
                pursueTarget = Instantiate(pursueTarget);
                currentState = idle;
            }
        }
        protected override void Update()
        {
            base.Update();
            aiCharacterCombatManager.HandleActionRecovery(this);
        }
        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }
            // the position/rotation should be reset after the state machine has processed it
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(
                    transform,
                    aiCharacterCombatManager.targetsDirection
                );
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(
                    transform.position,
                    aiCharacterCombatManager.currentTarget.transform.position
                );
            }
            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
    }
}