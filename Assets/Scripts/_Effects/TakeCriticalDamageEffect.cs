using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {
        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;
            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            // optional (?) CalculateStanceDamage(character); 
            character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;
        }
        protected override void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null)
            {
                // ToDo: Check for Damage modifiers and modify base damage (Physical or Elemental Damage Buff)
            }

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            
            if (finalDamageDealt <= 0)
                finalDamageDealt = 1;

            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage; // store previous poise damage taken for future interactions

            float remainingPoise = character.characterStatsManager.basePoiseDefense
                                   + character.characterStatsManager.offensivePoiseBonus
                                   + character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            // since Character is hit --> Reset Poise Timer (!)
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

    }
}