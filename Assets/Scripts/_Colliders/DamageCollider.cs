using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        public Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0; // (TO DO, SPLIT INTO "Standard", "Strike", "Slash" and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Poise Damage")]
        public float poiseDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        [Header("Block")]
        protected Vector3 directionFromAttackToDamageTarget;
        protected float dotValueFromAttackToDamageTarget;

        protected virtual void Awake()
        {
            if (damageCollider == null)
                damageCollider = GetComponent<Collider>();
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            // Check for character controller collider
            // if (damageTarget == null)
            //     damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                
                CheckForBlock(damageTarget);
                CheckForParry(damageTarget);
                DamageTarget(damageTarget);
            }
        }
        protected virtual void CheckForBlock(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            GetBlockingDotValues(damageTarget);
            // 1. Check if the Character is blocking
            // 2. If Character is Blocking -> Check for the correct Direction to block (!)
            if (damageTarget.characterNetworkManager.isBlocking.Value && dotValueFromAttackToDamageTarget < 0.3f) // ToDo: extract to variable
            {
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect blockedDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);
                blockedDamageEffect.physicalDamage = physicalDamage;
                blockedDamageEffect.magicDamage = magicDamage;
                blockedDamageEffect.fireDamage = fireDamage;
                blockedDamageEffect.holyDamage = holyDamage;
                blockedDamageEffect.staminaDamage = poiseDamage; // ToDo: you can use a own variable (!)
                blockedDamageEffect.poiseDamage = poiseDamage;
                blockedDamageEffect.contactPoint = contactPoint;

                // 3. Process Effect
                damageTarget.characterEffectsManager.ProcessInstantEffect(blockedDamageEffect);
            }
        }
        
        protected virtual void CheckForParry(CharacterManager damageTarget)
        {
            Debug.LogWarning("CheckForParry!!");
        }
        protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
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

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear(); //  WE RESET THE CHARACTERS THAT HAVE BEEN HIT WHEN WE RESET THE COLLIDER, SO THEY MAY BE HIT AGAIN
        }
    }
}