using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName = new("Character", NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Actions")]
        public NetworkVariable<bool> isUsingRightHand = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentSpellID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Two Handing")]
        public NetworkVariable<int> currentWeaponBeingTwoHanded =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingWeapon = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingRightWeapon =
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingLeftWeapon =
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> isChargingRightSpell = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingLeftSpell = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Armor")] // ToDo (!)
        public NetworkVariable<int> cloakEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> pantsEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> outfitEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> underwearEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> hoodEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> shoesAndGlovesEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maskEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> bagpackEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> hairEquipmentID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Projectiles")]
        public NetworkVariable<int> mainProjectileID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> secondaryProjectileID = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> hasArrowNotched = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isHoldingArrow = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);
            if (IsOwner)
            {
                player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
                player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
                player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
                player.playerStatsManager.blockingLightningAbsorption =
                    player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
                player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
            }
        }
        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingLeftHand.Value = true;
                isUsingRightHand.Value = false;
            }
        }
        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }
        public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
        {
            maxFocusPoints.Value = player.playerStatsManager.CalculateFocusPointsBasedOnMindLevel(newMind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointsValue(maxFocusPoints.Value);
            currentFocusPoints.Value = maxFocusPoints.Value;
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadRightWeapon();
            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);
            }
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);
            }
        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if (!player.IsOwner)
                return;

            if (player.playerCombatManager.currentWeaponBeingUsed != null)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }
        public void OnCurrentSpellIDChange(int oldID, int newID)
        {
            SpellItem newSpell = null;

            if (WorldItemDatabase.Instance.GetSpellByID(newID))
                newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newID));

            if (newSpell != null)
                player.playerInventoryManager.currentSpell = newSpell;

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetSpellItemQuickSlotIcon(newID);
            }
        }
        public void OnCurrentMainProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.mainProjectile = newProjectile;
        }
        public void OnCurrentSecondaryProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.secondaryProjectile = newProjectile;
        }

        public void OnIsHoldingArrowChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsHoldingArrow", isHoldingArrow.Value);
            if (isHoldingArrow.Value)
            {
                // player.playerNetworkManager.isLockedOn.Value = true;
                // player.playerCombatManager.currentAimTarget = player.playerAimCameraFollowTransform.transform;
                // TODO :   make the player strafe around the aim target 
                // TODO:    make the aim 
            }
            else
            {
                // player.playerNetworkManager.isLockedOn.Value = false;
                // player.playerCombatManager.currentAimTarget = null;

            }
        }
        
        public void OnIsChargingRightSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsChargingRightSpell", isChargingRightSpell.Value);
        }
        public void OnIsChargingLeftSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsChargingLeftSpell", isChargingLeftSpell.Value);
        }
        public void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingWeapon.Value)
            {
                if (IsOwner)
                {
                    isTwoHandingLeftWeapon.Value = false;
                    isTwoHandingRightWeapon.Value = false;
                }

                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.instance.twoHandingEffect.staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.instance.twoHandingEffect);
                player.playerEffectsManager.AddStaticEffect(twoHandEffect);
            }

            player.animator.SetBool("IsTwoHandingWeapon", isTwoHandingWeapon.Value);
        }
        public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingRightWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
            player.playerEquipmentManager.TwoHandRightWeapon();
        }
        public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingLeftWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerEquipmentManager.TwoHandLeftWeapon();
        }
        public void OnCloakEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            CloakWearableItem equipment = WorldItemDatabase.Instance.GetCloakEquipmentByID(cloakEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadCloakEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadCloakEquipment(null);
            }
        }
        public void OnPantsEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            PantsWearableItem wearable = WorldItemDatabase.Instance.GetPantsEquipmentByID(pantsEquipmentID.Value);
            if (wearable != null)
            {
                player.playerEquipmentManager.LoadPantsEquipment(Instantiate(wearable));
            }
            else
            {
                player.playerEquipmentManager.LoadUnderwearEquipment((UnderwearWearableItem)null);
            }
        }
        public void OnOutfitEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            OutfitWearableItem equipment = WorldItemDatabase.Instance.GetOutfitEquipmentByID(pantsEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadOutfitEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadOutfitEquipment(null);
            }
        }
        public void OnUnderwearEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            UnderwearWearableItem equipment = WorldItemDatabase.Instance.GetUnderwearEquipmentByID(underwearEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadUnderwearEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadUnderwearEquipment(null);
            }
        }
        public void OnHoodEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            HoodWearableItem equipment = WorldItemDatabase.Instance.GetHoodEquipmentByID(hoodEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHoodEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHoodEquipment(null);
            }
        }
        public void OnShoesAndGlovesEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            ShoesAndGlovesWearableItem equipment = WorldItemDatabase.Instance.GetShoesAndGlovesEquipmentByID(shoesAndGlovesEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadShoesAndGlovesEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadShoesAndGlovesEquipment(null);
            }
        }

        [ServerRpc] // Is a function called from a client, to the Server
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            // If Server -> notify Client (!)
            if (IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }
        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            // Do NOT play the Action for the Character who called it, because they already played it locally (!) 
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }
        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);
            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action is null, cannot be performed!");
            }
        }
        [ServerRpc]
        public void NotifyServerOfDrawnProjectileServerRpc(int projectileID)
        {
            if (IsServer)
            {
                NotifyServerOfDrawnProjectileClientRpc(projectileID);
            }
        }
        [ClientRpc]
        public void NotifyServerOfDrawnProjectileClientRpc(int projectileID)
        {
            Animator bowAnimator;
            if (isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }
            if (bowAnimator != null)
            {
                // Animate the bow
                bowAnimator.SetBool("IsDrawn", true);
                bowAnimator.Play("Bow_Draw_01");

                GameObject arrow = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(projectileID).drawProjectileModel,
                    player.playerEquipmentManager.leftHandWeaponSlot.transform);

                player.playerEffectsManager.activeDrawnProjectileFX = arrow;

                player.characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.notchArrowSFX));
            }
            else
            {
                Debug.LogError("bowAnimator is null!");
            }
        }

    }
}