using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;
        private LineRenderer aimingLineRenderer;

        public WeaponItem currentWeaponBeingUsed;
        public ProjectileSlot currentProjectileBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;
        // public bool canComboWithOffHandWeapon = false; 
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;

            if (aimingLineRenderer == null)
            {
                aimingLineRenderer = gameObject.AddComponent<LineRenderer>();
                aimingLineRenderer.startWidth = 0.02f;
                aimingLineRenderer.endWidth = 0.02f;
                aimingLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                aimingLineRenderer.startColor = Color.red;
                aimingLineRenderer.endColor = Color.red;
            }
        }
        protected  override void Update()
        {
            base.Update();
            if (player.playerNetworkManager.hasArrowNotched.Value)
                DrawAimingRay();
        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                //  PERFORM THE ACTION
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
                // ToDo: if you wanna keep this here to send info about the action --> buuut do all the checks if the RPC should be send before -> so just if the action actually is fired
                //  NOTIFY THE SERVER WE HAVE PERFORMED THE ACTION, SO WE PERFORM IT FROM THERE PERSPECTIVE ALSO}
                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(
                    NetworkManager.Singleton.LocalClientId,
                    weaponAction.actionID,
                    weaponPerformingAction.itemID
                );
            }
        }

        public override void AttemptRiposte(RaycastHit hit)
        {
            base.AttemptRiposte(hit);

            CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (!targetCharacter.characterNetworkManager.isRipostable.Value)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;
            Debug.LogWarning("Attempting riposte AFTER IF CHECKS (!)");

            // just riposte with melee weapon
            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

            //  isInvulnerable while riposting
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            // 1.Create a new Damage Effext
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            // 2. Apply Damage values
            damageEffect.physicalDamage = riposteCollider.physicalDamage;
            damageEffect.holyDamage = riposteCollider.holyDamage;
            damageEffect.fireDamage = riposteCollider.fireDamage;
            damageEffect.lightningDamage = riposteCollider.lightningDamage;
            damageEffect.magicDamage = riposteCollider.magicDamage;
            damageEffect.poiseDamage = riposteCollider.poiseDamage;

            // 3. multiply with riposte modifiers
            damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

            // 4. send server rpc to play the animation properly on the client side
            targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRpc(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId,
                "Riposted_01",
                riposteWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage);
        }
        public override void AttemptBackStab(RaycastHit hit)
        {
            base.AttemptBackStab(hit);

            CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (!targetCharacter.characterCombatManager.canBeBackStabbed)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            Debug.LogWarning("Attempting BackStab AFTER IF CHECKS (!)");

            // just riposte with melee weapon
            MeleeWeaponItem backStabWeapon;
            MeleeWeaponDamageCollider backStabCollider;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                backStabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                backStabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                backStabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                backStabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }
            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("BackStab_01", true);

            //  isInvulnerable while riposting
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            // 1.Create a new Damage Effext
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            // 2. Apply Damage values
            damageEffect.physicalDamage = backStabCollider.physicalDamage;
            damageEffect.holyDamage = backStabCollider.holyDamage;
            damageEffect.fireDamage = backStabCollider.fireDamage;
            damageEffect.lightningDamage = backStabCollider.lightningDamage;
            damageEffect.magicDamage = backStabCollider.magicDamage;
            damageEffect.poiseDamage = backStabCollider.poiseDamage;

            // 3. multiply with riposte modifiers
            damageEffect.physicalDamage *= backStabWeapon.backStab__Attack_01_Modifier;
            damageEffect.holyDamage *= backStabWeapon.backStab__Attack_01_Modifier;
            damageEffect.fireDamage *= backStabWeapon.backStab__Attack_01_Modifier;
            damageEffect.lightningDamage *= backStabWeapon.backStab__Attack_01_Modifier;
            damageEffect.magicDamage *= backStabWeapon.backStab__Attack_01_Modifier;
            damageEffect.poiseDamage *= backStabWeapon.backStab__Attack_01_Modifier;

            // 4. send server rpc to play the animation properly on the client side
            targetCharacter.characterNetworkManager.NotifyTheServerOfBackStabServerRpc(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId,
                "BackStabbed_01",
                backStabWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage);
        }
        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);
            if (IsOwner)
            {
                PlayerCamera.instance.SetLockCameraHeight();
            }
        }

        #region Animation Events
        public override void EnableCanDoCombo()
        {
            base.EnableCanDoCombo();

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                // Enable off hand combo
            }
        }
        public override void DisableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = false;
            }
            else
            {
                // Enable off hand combo
                // canComboWithOffHandWeapon = false; 
            }
        }
        public void ReleaseArrow()
        {
            if (player.IsOwner)
                player.playerNetworkManager.hasArrowNotched.Value = false;

            if (player.playerEffectsManager.activeDrawnProjectileFX != null)
                Destroy(player.playerEffectsManager.activeDrawnProjectileFX);
            // Play Sfx
            player.characterSoundFXManager.PlaySoundFX(
                WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.releaseArrowSFX));
            // Animate Bow
            Animator bowAnimator;
            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }
            bowAnimator.SetBool("IsDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            // 
            if (!player.IsOwner)
                return;

            RangedProjectileItem projectileItem = null;
            switch (currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = player.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = player.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }
            if (projectileItem == null)
                return;

            if (projectileItem.currentAmmoAmount <= 0)
                return;

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            //  SUBTRACT AMMO
            projectileItem.currentAmmoAmount -= 1;
            //  (TODO MAKE AND UPDATE ARROW COUNT UI)

            projectileInstantiationLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            //  (TODO MAKE FORMULA TO SET RANGE PROJECTILE DAMAGE)
            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            //  FIRE AN ARROW BASED ON 1 OF 3 VARIATIONS
            // 1. LOCKED ONTO A TARGET
            if (player.playerCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(
                    player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position
                    - projectileGameObject.transform.position);
                projectileGameObject.transform.rotation = arrowRotation;
            }
            // 2.2. UNLOCKED 
            else
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.playerAimCameraFollowTransform.transform.forward);
                projectileRigidbody.transform.rotation = arrowRotation;
            }


            //  GET ALL CHARACTER COLLIDERS AND IGNORE SELF
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnore = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);

            foreach (Collider hitBox in collidersArrowWillIgnore)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;

            //  TO DO (SYNC ARRROW FIRE WITH SERVER RPC)
        }

        public void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;
            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0f;
            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack03:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack04:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack03:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack04:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack_01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack_01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightJumpingAttack_01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyJumpingAttack_01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;

                    break;
                default:
                    break;
            }
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }
        #endregion
        public void InstantiateSpellWarmUpFX()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);
        }
        public void SuccessfullyCastSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpell(player);
        }
        public void SuccessfullyChargeSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyChargeSpell(player);
        }
        public void SuccessfullyCastSpellFullCharged()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpellFullCharged(player);
        }
        public WeaponItem SelectWeaponToPerformAshOfWar()
        {
            // ToDo: select actually a weapon (!)
            WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerNetworkManager.SetCharacterActionHand(false);
            player.playerNetworkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;
            return selectedWeapon;
        }
        private void DrawAimingRay()
        {
            if (player == null || player.playerAimCameraFollowTransform == null) return;

            // Starting position of the ray
            Vector3 start = player.playerCombatManager.lockOnTransform.position;

            // Direction vector from the forward transform
            Vector3 direction = player.playerAimCameraFollowTransform.transform.forward;

            // Adjust the length of the ray
            float rayLength = 100f;
            Vector3 end = start + direction * rayLength;

            // Set positions in LineRenderer
            aimingLineRenderer.positionCount = 2;
            aimingLineRenderer.SetPosition(0, start);
            aimingLineRenderer.SetPosition(1, end);
        }
        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }
}