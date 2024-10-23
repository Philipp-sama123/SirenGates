using System;
using System.Collections;
using UnityEngine;

namespace KrazyKatGames
{
    public class EnergyBallManager : SpellManager
    {
        // ---------- what´s this bad boy for? ----------
        // 1.   Make the spell slightly "curve" or "follow" its lock Targets as they are moving 
        // 2.   Assigning Damage neatly with a function from this script
        // 3.   Enabling / Disabling VFX and SFX, such as "contact" 
        [Header("Colliders")]
        public EnergyBallDamageCollider damageCollider;

        [Header("InstantiatedFX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;

        private Rigidbody energyBallRigidbody;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();
            energyBallRigidbody = GetComponent<Rigidbody>();
            damageCollider = GetComponentInChildren<EnergyBallDamageCollider>();
        }
        protected override void Update()
        {
            base.Update();

            if (spellTarget != null)
                transform.LookAt(spellTarget.transform);

            if (energyBallRigidbody != null)
            {
                Vector3 currentVelocity = energyBallRigidbody.velocity;
                energyBallRigidbody.velocity = transform.forward + currentVelocity;
            }
        }

        public void InitializeEnergyBall(CharacterManager character)
        {
            damageCollider.spellCaster = character;
            damageCollider.fireDamage = 50;

            if (isFullyCharged)
                damageCollider.fireDamage *= 1.5f;

            // ToDo: 
            // setup DamageFormula to calculate Damage based on Character stats,spell Power and spell casting weapons spell buff
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
                return;

            if (!hasCollided)
            {
                hasCollided = true;
                InstantiateSpellDestructionFX();
            }
        }
        public void InstantiateSpellDestructionFX()
        {
            if (isFullyCharged)
            {
                instantiatedDestructionFX = Instantiate(impactParticleFullCharge, transform.position, Quaternion.identity);
            }
            else
            {
                instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        public void WaitThenInstantiateSpellDestructionFX(float timeToWait)
        {
            if (destructionFXCoroutine != null)
                StopCoroutine(destructionFXCoroutine);
            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }
        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            InstantiateSpellDestructionFX();
        }
    }
}