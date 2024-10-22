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

            Debug.Log("INSTANTIATED FX");
        }

        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            // 1. Destroy ANY warm up fx from this spell 
            if (player.IsOwner)
                player.playerCombatManager.DestroyAllCurrentActionFX();

            // 2. Get any Colliders from the Caster 
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            Collider characterCollisionCollider = player.GetComponent<Collider>();

            // 3. Instantiate the Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleaseSpellFX = Instantiate(spellCastReleaseFX);

            // 4. Instantiate Warm up on the proper place (staff or hand (?))
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            // 5. Use the List of Colliders from the Caster and apply the ignorePhysics thingy with the Collider from the Projectile 
            // ToDo (!)
            instantiatedReleaseSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleaseSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleaseSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleaseSpellFX.transform.parent = null;

            // 5. Apply Damage to the Projectiles Damage Collider 
            // 6. Set the Projectiles Velocity and Direction
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleaseSpellFX.transform.LookAt(player.playerCombatManager.transform.position);
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
            spellRigidbody.velocity = totalVelocity;

            Debug.Log("CASTED SPELL");
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