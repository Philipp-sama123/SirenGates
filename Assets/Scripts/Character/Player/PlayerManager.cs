using UnityEngine;
using UnityEngine.SceneManagement;

namespace KrazyKatgames
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();

            //  DO MORE STUFF, ONLY FOR THE PLAYER

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();

            //  IF WE DO NOT OWN THIS GAMEOBJECT, WE DO NOT CONTROL OR EDIT IT
            if (!IsOwner)
                return;

            //  HANDLE MOVEMENT
            playerLocomotionManager.HandleAllMovement();

            //  REGEN STAMINA
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;

                // Update Maximum Health or Maximum Stamina when endurance or vitality is changed
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                
                // Updates UI for Stat Bars (!)
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUiHudManager.SetNewStaminaValue;
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUiHudManager.SetNewHealthValue;
                
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = characterNetworkManager.vitality.Value;
            currentCharacterData.endurance = characterNetworkManager.endurance.Value;

            currentCharacterData.currentHealth = characterNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = characterNetworkManager.currentStamina.Value;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
            
            characterNetworkManager.vitality.Value = currentCharacterData.vitality;
            characterNetworkManager.endurance.Value = currentCharacterData.endurance;
            
            characterNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            characterNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(currentCharacterData.vitality);

            PlayerUIManager.instance.playerUiHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            PlayerUIManager.instance.playerUiHudManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
        }
    }
}