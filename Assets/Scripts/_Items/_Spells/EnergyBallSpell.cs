using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Items/Spells/Energy Ball")]
    public class EnergyBallSpell : SpellItem
    {
        [Header("Projectile Velocity")]
        [SerializeField] float upwardVelocity = 3;
        [SerializeField] float forwardVelocity = 15;

        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
                return;

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);
            }
        }

        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            // 1. Determine which Hand the Player is using 
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellCastWarmUpFX);

            // 2. Instantiate Warm up on the proper place (staff or hand (?))
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            instantiatedWarmUpSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedWarmUpSpellFX.transform.localPosition = Vector3.zero;
            instantiatedWarmUpSpellFX.transform.localRotation = Quaternion.identity;

            // 3. "Save" the WarmUpFX as a Variable so it can be destroyed if the player is knocked out of the Animation
            player.playerEffectsManager.activeSpellWarmUpFX = instantiatedWarmUpSpellFX;
        }
        public override void SuccessfullyChargeSpell(PlayerManager player)
        {
            base.SuccessfullyChargeSpell(player);
            // TODO: find out why its crashing
            // if (player.IsOwner)
            //     player.playerCombatManager.DestroyAllCurrentActionFX();

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedChargeSpellFX = Instantiate(spellChargeFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            // Save the charge up fx 
            player.playerEffectsManager.activeDrawnProjectileFX = instantiatedChargeSpellFX;

            instantiatedChargeSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedChargeSpellFX.transform.localPosition = Vector3.zero;
            instantiatedChargeSpellFX.transform.localRotation = Quaternion.identity;
        }
        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            // 1. Destroy ANY warm up fx from this spell 
            // TODO: find out why its crashing
            // if (player.IsOwner)
            //     player.playerCombatManager.DestroyAllCurrentActionFX();

            // 2. Instantiate the Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleaseSpellFX = Instantiate(spellCastReleaseFX);

            // 3. Instantiate Warm up on the proper place (staff or hand (?))
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiatedReleaseSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleaseSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleaseSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleaseSpellFX.transform.parent = null;

            // 4. Apply Damage to the Projectiles Damage Collider 
            EnergyBallManager energyBallManager = instantiatedReleaseSpellFX.GetComponent<EnergyBallManager>();
            energyBallManager.InitializeEnergyBall(player);

            #region Optional Ignore Collision
            // Use the List of Colliders from the Caster and apply the ignorePhysics thingy with the Collider from the Projectile 
            // ... for now not needed actually 
            // Get any Colliders from the Caster 
            // Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            // Collider characterCollisionCollider = player.GetComponent<Collider>();

            // Physics.IgnoreCollision(characterCollisionCollider, energyBallManager.damageCollider.damageCollider, true);
            // foreach (var collider in characterColliders)
            // {
            //     Physics.IgnoreCollision(characterCollisionCollider, collider, true);
            // }
            #endregion

            // 5. Set the Projectiles Velocity and Direction
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleaseSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiatedReleaseSpellFX.transform.forward = forwardDirection;
            }
            Rigidbody spellRigidbody = instantiatedReleaseSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedReleaseSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleaseSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            spellRigidbody.linearVelocity = totalVelocity;
        }
        public override void SuccessfullyCastSpellFullCharged(PlayerManager player)
        {
            base.SuccessfullyCastSpellFullCharged(player);

            // 1. Destroy ANY warm up fx from this spell 
            // ToDo: find out why this breaks
            // if (player.IsOwner)
            //     player.playerCombatManager.DestroyAllCurrentActionFX();

            // 2. Instantiate the Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleaseSpellFX = Instantiate(spellCastReleaseFXFullCharged);

            // 3. Instantiate Warm up on the proper place (staff or hand (?))
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiatedReleaseSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleaseSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleaseSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleaseSpellFX.transform.parent = null;

            // 4. Apply Damage to the Projectiles Damage Collider 
            EnergyBallManager energyBallManager = instantiatedReleaseSpellFX.GetComponent<EnergyBallManager>();
            energyBallManager.isFullyCharged = true;
            energyBallManager.InitializeEnergyBall(player);

            // 5. Set the Projectiles Velocity and Direction
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleaseSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiatedReleaseSpellFX.transform.forward = forwardDirection;
            }
            Rigidbody spellRigidbody = instantiatedReleaseSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedReleaseSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleaseSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            spellRigidbody.linearVelocity = totalVelocity;
        }

        public override bool CanICastThisSpell(PlayerManager player)
        {
            if (player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isJumping.Value)
                return false;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return false;

            return true;
        }
    }
}