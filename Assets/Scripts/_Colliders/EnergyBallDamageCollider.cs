using UnityEngine;

namespace KrazyKatGames
{
    public class EnergyBallDamageCollider : SpellProjectileDamageCollider
    {
        private EnergyBallManager energyBallManager;
        protected override void Awake()
        {
            base.Awake();

            energyBallManager = GetComponentInParent<EnergyBallManager>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                if (damageTarget == spellCaster)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                //  Friendly Fire
                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(spellCaster.characterGroup, damageTarget.characterGroup))
                    return;

                CheckForParry(damageTarget);
                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(damageTarget);

                //    energyBallManager.WaitThenInstantiateSpellDestructionFX(.5f); or
                energyBallManager.InstantiateSpellDestructionFX();
            }
        }
        protected override void CheckForParry(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;
            if (!spellCaster.characterNetworkManager.isParryable.Value)
                return;
            if (!damageTarget.IsOwner)
                return;
            if (damageTarget.characterNetworkManager.isParrying.Value)
            {
                charactersDamaged.Add(damageTarget);
                damageTarget.characterNetworkManager.NotifyServerOfParryServerRpc(spellCaster.NetworkObjectId);
                damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
            }
        }
        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = transform.position - spellCaster.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            //  WE DON'T WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            //  SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageEffect.poiseDamage = poiseDamage;

            damageEffect.angleHitFrom = Vector3.SignedAngle(spellCaster.transform.forward, damageTarget.transform.forward, Vector3.up);


            if (spellCaster.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    spellCaster.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    contactPoint.y,
                    contactPoint.z
                );
            }
            //  damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}