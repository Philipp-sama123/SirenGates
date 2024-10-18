using System.Collections;
using System.Collections.Generic;
using KrazyKatGames;
using UnityEngine;

public class AIUndeadCombatManager : AICharacterCombatManager
{
    [Header("Damage Collider")]
    [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
    [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] int basePoiseDamage = 15;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;

    #region Animation Events
    public void SetAttack01Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
    }
    public void SetAttack02Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
        leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
    }
    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
    }
    public void DisableRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }
    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
    }
    public void DisableLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion
}