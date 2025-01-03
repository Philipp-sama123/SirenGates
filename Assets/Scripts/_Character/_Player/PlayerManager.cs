using System.Collections;
using MyNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;


namespace KrazyKatGames
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool respawnCharacter = false;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
        [HideInInspector] public PlayerAimCameraFollowTransform playerAimCameraFollowTransform;

        protected override void Awake()
        {
            base.Awake();

            //  DO MORE STUFF, ONLY FOR THE PLAYER
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerBodyManager = GetComponent<PlayerBodyManager>();

            playerAimCameraFollowTransform = GetComponentInChildren<PlayerAimCameraFollowTransform>();
        }

        protected override void Update()
        {
            base.Update();

            //  IF WE DO NOT OWN THIS GAMEOBJECT, WE DO NOT CONTROL OR EDIT IT
            if (!IsOwner)
                return;

            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();
            DebugMenu();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;

                // Update Maximum Health or Maximum Stamina when endurance or vitality is changed
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointsValue;

                // Updates UI for Stat Bars (!)
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointsBarValue;

                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
            }

            // Only for other Players - show HP Bar
            if (!IsOwner)
                playerNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

            // Stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

            // LockOn
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged += playerNetworkManager.OnCurrentSpellIDChange;
            // Projectiles
            playerNetworkManager.mainProjectileID.OnValueChanged += playerNetworkManager.OnCurrentMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged += playerNetworkManager.OnCurrentSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged += playerNetworkManager.OnIsHoldingArrowChanged;
            // Spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged += playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged += playerNetworkManager.OnIsChargingLeftSpellChanged;

            // Blocking
            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;

            // Armor
            playerNetworkManager.cloakEquipmentID.OnValueChanged += playerNetworkManager.OnCloakEquipmentChanged;
            playerNetworkManager.pantsEquipmentID.OnValueChanged += playerNetworkManager.OnPantsEquipmentChanged;
            playerNetworkManager.outfitEquipmentID.OnValueChanged += playerNetworkManager.OnOutfitEquipmentChanged;
            playerNetworkManager.underwearEquipmentID.OnValueChanged += playerNetworkManager.OnUnderwearEquipmentChanged;
            playerNetworkManager.hoodEquipmentID.OnValueChanged += playerNetworkManager.OnHoodEquipmentChanged;
            playerNetworkManager.shoesAndGlovesEquipmentID.OnValueChanged += playerNetworkManager.OnShoesAndGlovesEquipmentChanged;

            // Two Handing 
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            // Flags
            playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;

            // on connecting --> reload character data to this new instantiated character
            // not run if server --> bc the host should already have loaded this 
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            if (IsOwner)
            {
                playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointsValue;

                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointsBarValue;

                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
            }

            if (!IsOwner)
                playerNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;
            // LockOn
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged -= playerNetworkManager.OnCurrentSpellIDChange;
            // Projectiles
            playerNetworkManager.mainProjectileID.OnValueChanged -= playerNetworkManager.OnCurrentMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged -= playerNetworkManager.OnCurrentSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged -= playerNetworkManager.OnIsHoldingArrowChanged;

            // Spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged -= playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged -= playerNetworkManager.OnIsChargingLeftSpellChanged;
            // Blocking
            playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;
            // Armor
            playerNetworkManager.cloakEquipmentID.OnValueChanged -= playerNetworkManager.OnCloakEquipmentChanged;
            playerNetworkManager.pantsEquipmentID.OnValueChanged -= playerNetworkManager.OnPantsEquipmentChanged;
            playerNetworkManager.outfitEquipmentID.OnValueChanged -= playerNetworkManager.OnOutfitEquipmentChanged;
            playerNetworkManager.underwearEquipmentID.OnValueChanged -= playerNetworkManager.OnUnderwearEquipmentChanged;
            playerNetworkManager.hoodEquipmentID.OnValueChanged -= playerNetworkManager.OnHoodEquipmentChanged;
            playerNetworkManager.shoesAndGlovesEquipmentID.OnValueChanged -= playerNetworkManager.OnShoesAndGlovesEquipmentChanged;
            // Two Handing 
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;
            // Flags
            playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
            playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;
        }
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if (IsOwner)
            {
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            }
        }
        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
            }
            // ToDo: check for alive Players, if 0 --> Respawn all Characters (!)
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.mind = playerNetworkManager.mind.Value;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;

            // Equipment
            currentCharacterData.pantsEquipment = playerNetworkManager.pantsEquipmentID.Value;
            currentCharacterData.underwearEquipment = playerNetworkManager.underwearEquipmentID.Value;
            currentCharacterData.outfitEquipment = playerNetworkManager.outfitEquipmentID.Value;
            currentCharacterData.hoodEquipment = playerNetworkManager.hoodEquipmentID.Value;
            currentCharacterData.cloakEquipment = playerNetworkManager.cloakEquipmentID.Value;
            currentCharacterData.shoesAndGlovesEquipment = playerNetworkManager.shoesAndGlovesEquipmentID.Value;
            currentCharacterData.bagpackEquipment = playerNetworkManager.bagpackEquipmentID.Value;
            currentCharacterData.maskEquipment = playerNetworkManager.maskEquipmentID.Value;

            // Weapons
            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
            currentCharacterData.rightWeapon01 = playerInventoryManager.weaponsInRightHandSlots[0].itemID; // They should always default to (unarmed)
            currentCharacterData.rightWeapon02 = playerInventoryManager.weaponsInRightHandSlots[1].itemID; // They should always default to (unarmed)
            currentCharacterData.rightWeapon03 = playerInventoryManager.weaponsInRightHandSlots[2].itemID; // They should always default to (unarmed)

            currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
            currentCharacterData.leftWeapon01 = playerInventoryManager.weaponsInLeftHandSlots[0].itemID; // They should always default to (unarmed)
            currentCharacterData.leftWeapon02 = playerInventoryManager.weaponsInLeftHandSlots[1].itemID; // They should always default to (unarmed)
            currentCharacterData.leftWeapon03 = playerInventoryManager.weaponsInLeftHandSlots[2].itemID; // They should always default to (unarmed)

            // Spells
            if (playerInventoryManager.currentSpell)
                currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            playerNetworkManager.mind.Value = currentCharacterData.mind;

            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(currentCharacterData.vitality);
            playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(currentCharacterData.mind);

            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;

            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointsValue(playerNetworkManager.maxFocusPoints.Value);

            // Equipment
            if (WorldItemDatabase.Instance.GetUnderwearEquipmentByID(currentCharacterData.underwearEquipment))
            {
                UnderwearWearableItem underwear =
                    Instantiate(WorldItemDatabase.Instance.GetUnderwearEquipmentByID(currentCharacterData.underwearEquipment));
                playerInventoryManager.underwearWearable = underwear;
            }
            else
            {
                playerInventoryManager.underwearWearable = null;
            }
            if (WorldItemDatabase.Instance.GetCloakEquipmentByID(currentCharacterData.cloakEquipment))
            {
                CloakWearableItem cloak = Instantiate(WorldItemDatabase.Instance.GetCloakEquipmentByID(currentCharacterData.cloakEquipment));
                playerInventoryManager.cloakWearable = cloak;
            }
            else
            {
                playerInventoryManager.cloakWearable = null;
            }
            if (WorldItemDatabase.Instance.GetHoodEquipmentByID(currentCharacterData.hoodEquipment))
            {
                HoodWearableItem hood = Instantiate(WorldItemDatabase.Instance.GetHoodEquipmentByID(currentCharacterData.hoodEquipment));
                playerInventoryManager.hoodWearable = hood;
            }
            else
            {
                playerInventoryManager.hoodWearable = null;
            }
            if (WorldItemDatabase.Instance.GetOutfitEquipmentByID(currentCharacterData.outfitEquipment))
            {
                OutfitWearableItem outfit = Instantiate(WorldItemDatabase.Instance.GetOutfitEquipmentByID(currentCharacterData.outfitEquipment));
                playerInventoryManager.outfitWearable = outfit;
            }
            else
            {
                playerInventoryManager.outfitWearable = null;
            }
            if (WorldItemDatabase.Instance.GetPantsEquipmentByID(currentCharacterData.pantsEquipment))
            {
                PantsWearableItem pants = Instantiate(WorldItemDatabase.Instance.GetPantsEquipmentByID(currentCharacterData.pantsEquipment));
                playerInventoryManager.pantsWearable = pants;
            }
            else
            {
                playerInventoryManager.pantsWearable = null;
            }
            if (WorldItemDatabase.Instance.GetShoesAndGlovesEquipmentByID(currentCharacterData.shoesAndGlovesEquipment))
            {
                ShoesAndGlovesWearableItem shoesAndGloves =
                    Instantiate(WorldItemDatabase.Instance.GetShoesAndGlovesEquipmentByID(currentCharacterData.shoesAndGlovesEquipment));
                playerInventoryManager.shoesAndGlovesWearable = shoesAndGloves;
            }
            else
            {
                playerInventoryManager.shoesAndGlovesWearable = null;
            }

            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon01))
            {
                WeaponItem rightWeapon01 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon01));
                playerInventoryManager.weaponsInRightHandSlots[0] = rightWeapon01;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[0] = null;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon02))
            {
                WeaponItem rightWeapon02 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon02));
                playerInventoryManager.weaponsInRightHandSlots[1] = rightWeapon02;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[1] = null;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon03))
            {
                WeaponItem rightWeapon03 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon03));
                playerInventoryManager.weaponsInRightHandSlots[2] = rightWeapon03;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[2] = null;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon01))
            {
                WeaponItem leftWeapon01 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon01));
                playerInventoryManager.weaponsInLeftHandSlots[0] = leftWeapon01;
            }
            else
            {
                playerInventoryManager.weaponsInLeftHandSlots[0] = null;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon02))
            {
                WeaponItem leftWeapon02 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon02));
                playerInventoryManager.weaponsInLeftHandSlots[1] = leftWeapon02;
            }
            else
            {
                playerInventoryManager.weaponsInLeftHandSlots[1] = null;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon03))
            {
                WeaponItem leftWeapon03 = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon03));
                playerInventoryManager.weaponsInLeftHandSlots[2] = leftWeapon03;
            }
            else
            {
                playerInventoryManager.weaponsInLeftHandSlots[2] = null;
            }
            // Spells
            if (WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell))
            {
                SpellItem spellItem = Instantiate(WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell));
                playerNetworkManager.currentSpellID.Value = spellItem.itemID;
            }
            else
            {
                playerNetworkManager.currentSpellID.Value = -1;
            }
            playerEquipmentManager.EquipWearables();

            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;

            if (currentCharacterData.rightWeaponIndex >= 0)
            {
                playerNetworkManager.currentRightHandWeaponID.Value =
                    playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = 0;
            }

            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
            
            if (currentCharacterData.leftWeaponIndex >= 0)
            {
                playerNetworkManager.currentLeftHandWeaponID.Value =
                    playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = 0;
            }
        }
        private void OnClientConnectedCallback(ulong clientID)
        {
            WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);
            // ToDo: keep a list of the active players 

            // If we are the server -->Host , so don't load other Players Equipment
            if (!IsServer && IsOwner)
            {
                foreach (var player in WorldGameSessionManager.instance.players)
                {
                    if (player != null)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }
            }
            //   LoadGameDataFromCurrentCharacterData();
        }
        private void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            // Sync Weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentSpellIDChange(0, playerNetworkManager.currentSpellID.Value);
            // Sync Two Hand Status
            playerNetworkManager.OnIsTwoHandingWeaponChanged(false, playerNetworkManager.isTwoHandingWeapon.Value);
            playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, playerNetworkManager.isTwoHandingRightWeapon.Value);
            playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, playerNetworkManager.isTwoHandingLeftWeapon.Value);
            // Sync Projectiles
            playerNetworkManager.OnCurrentMainProjectileIDChange(-1, playerNetworkManager.mainProjectileID.Value);
            playerNetworkManager.OnCurrentSecondaryProjectileIDChange(-1, playerNetworkManager.secondaryProjectileID.Value);
            playerNetworkManager.OnIsHoldingArrowChanged(false, playerNetworkManager.isHoldingArrow.Value);
            // Sync Blocking Status
            playerNetworkManager.OnIsBlockingChanged(false, playerNetworkManager.isBlocking.Value);
            // Sync Armor
            playerNetworkManager.OnCloakEquipmentChanged(-1, playerNetworkManager.cloakEquipmentID.Value);
            playerNetworkManager.OnOutfitEquipmentChanged(-1, playerNetworkManager.outfitEquipmentID.Value);
            playerNetworkManager.OnUnderwearEquipmentChanged(-1, playerNetworkManager.underwearEquipmentID.Value);
            playerNetworkManager.OnPantsEquipmentChanged(-1, playerNetworkManager.pantsEquipmentID.Value);
            playerNetworkManager.OnHoodEquipmentChanged(-1, playerNetworkManager.hoodEquipmentID.Value);
            playerNetworkManager.OnShoesAndGlovesEquipmentChanged(-1, playerNetworkManager.shoesAndGlovesEquipmentID.Value);

            // Update Lock on
            if (playerNetworkManager.isLockedOn.Value)
            {
                playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
            }
        }
        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
        }
    }
}