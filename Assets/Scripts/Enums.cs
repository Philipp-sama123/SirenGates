using UnityEngine;

public class Enums : MonoBehaviour
{
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHand,
    //Right Hips
    //Left Hips
    //Back
}
public enum CharacterGroup
{
    Team01,
    Team02,
}
// used to calculate Damage
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    LightAttack03,
    LightAttack04,
    HeavyAttack01,
    HeavyAttack02,
    HeavyAttack03,
    HeavyAttack04,
    ChargedAttack01,
    ChargedAttack02,
    RunningAttack01,
    RollingAttack_01,
    BackstepAttack_01
}

public enum DamageIntensity
{
    Ping,
    Light, 
    Medium, 
    Heavy, 
    Colossal
}