using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Contact Point")]
        private Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            // Check if its a Layer 
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                CharacterManager damageTarget = other.GetComponent<CharacterManager>();

                if (damageTarget != null)
                {
                    contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    DamageTarget(damageTarget);
                }
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return; // remove if it should hit multiple times

            charactersDamaged.Add(damageTarget);
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;

            damageEffect.contactPoint = contactPoint;
            
            damageTarget.characterEffects.ProcessInstantEffect(damageEffect);
        }
    }
}