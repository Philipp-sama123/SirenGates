using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatgames
{
    public class DurkStompCollider : DamageCollider
    {
        [SerializeField] private AIDurkCharacterManager aiDurkCharacterManager;
        protected override void Awake()
        {
            base.Awake();
            aiDurkCharacterManager = GetComponentInParent<AIDurkCharacterManager>();
        }
        public void StompAttack()
        {
            GameObject stompVFX = Instantiate(aiDurkCharacterManager.durkCombatManager.durkImpactVFX, transform);

            Collider[] colliders = Physics.OverlapSphere(transform.position,
                aiDurkCharacterManager.durkCombatManager.stompAttackAOERadius,
                WorldUtilityManager.Instance.GetCharacterLayers());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>(); // new list each time

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character))
                        continue;
                    // don`t hurt himself (!)
                    if (character == aiDurkCharacterManager)
                        continue;
                    charactersDamaged.Add(character);

                    //  WE ONLY PROCESS DAMAGE IF THE CHARACTER "ISOWNER" SO THAT THEY ONLY GET DAMAGED IF THE COLLIDER CONNECTS ON THEIR CLIENT
                    //  MEANING IF YOU ARE HIT ON THE HOSTS SCREEN BUT NOT ON YOUR OWN, YOU WILL NOT BE HIT
                    if (character.IsOwner)
                    {
                        //  CHECK FOR BLOCK

                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                        damageEffect.physicalDamage = aiDurkCharacterManager.durkCombatManager.stompDamage;
                        damageEffect.poiseDamage = aiDurkCharacterManager.durkCombatManager.stompDamage;

                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }
            }
        }
    }
}