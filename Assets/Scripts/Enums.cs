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
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    BackSlot
    //Right Hips
    //Left Hips
    //BackSlot
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
    BackstepAttack_01,
    LightJumpingAttack_01,
    HeavyJumpingAttack_01
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}

public enum WeaponModelType
{
    Weapon,
    Shield
    // maybe Item (?)
}
public enum WearableModelType
{
    Underwear,
    Mask,
    Attachment,
    Pants,
    Outfit,
    Hood,
    Cloak,
    Bagpack,
    ShoesAndGloves,
}
public enum WeaponClass
{
    Blade,
    MediumShield,
    StraightSword,
    Fist,
    LightShield,
    // ... todo more (!)
}

public enum ItemPickUpType
{
    WorldSpawn,
    CharacterDrop,
}

public enum EquipmentType
{
    RightWeapon01, // 0
    RightWeapon02, // 1
    RightWeapon03, // 2
    LeftWeapon01, // 3
    LeftWeapon02, // 4
    LeftWeapon03, // 5
    Underwear, // 6
    Pants, // 7
    Outfit, // 8
    Hood, // 9 
    Cloak, // 10
    ShoesAndGloves, // 11
    // more to come
}

public enum SpellClass
{
    Incantation,
    Sorcery
}
public enum ProjectileClass
{
    Arrow,
    Bolt
}