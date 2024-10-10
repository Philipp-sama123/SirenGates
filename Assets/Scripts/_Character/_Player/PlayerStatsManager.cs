namespace KrazyKatgames
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            //  WHY CALCULATE THESE HERE?
            //  WHEN WE MAKE A CHARACTER CREATION MENU, AND SET THE STATS DEPENDING ON THE CLASS, THIS WILL BE CALCULATED THERE
            //  UNTIL THEN HOWEVER, STATS ARE NEVER CALCULATED, SO WE DO IT HERE ON START, IF A SAVE FILE EXISTS THEY WILL BE OVER WRITTEN WHEN LOADING INTO A SCENE
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }

        public void CalculateTotalArmorAbsorption()
        {
            // Reset all Values to 0 
            armorPhysicalDamageAbsorption = 0;
            armorMagicDamageAbsorption = 0;
            armorFireDamageAbsorption = 0;
            armorHolyDamageAbsorption = 0;
            armorLightningDamageAbsorption = 0;

            armorImmunity = 0;
            armorRobustness = 0;
            armorFocus = 0;
            armorVitality = 0;

            basePoiseDefense = 0;

            //  Head Equipment
            if (player.playerInventoryManager.cloakWearable != null)
            {
                //  Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.cloakWearable.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.cloakWearable.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.cloakWearable.fireDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.cloakWearable.holyDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.cloakWearable.lightningDamageAbsorption;

                //  Status Effect Resistance
                armorRobustness += player.playerInventoryManager.cloakWearable.robustness;
                armorVitality += player.playerInventoryManager.cloakWearable.vitality;
                armorImmunity += player.playerInventoryManager.cloakWearable.immunity;
                armorFocus += player.playerInventoryManager.cloakWearable.focus;

                //  Poise
                basePoiseDefense += player.playerInventoryManager.cloakWearable.poise;
            }
            //  Body Equipment
            if (player.playerInventoryManager.outfitWearable != null)
            {
                //  Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.outfitWearable.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.outfitWearable.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.outfitWearable.fireDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.outfitWearable.holyDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.outfitWearable.lightningDamageAbsorption;

                //  Status Effect Resistance
                armorRobustness += player.playerInventoryManager.outfitWearable.robustness;
                armorVitality += player.playerInventoryManager.outfitWearable.vitality;
                armorImmunity += player.playerInventoryManager.outfitWearable.immunity;
                armorFocus += player.playerInventoryManager.outfitWearable.focus;

                //  Poise
                basePoiseDefense += player.playerInventoryManager.outfitWearable.poise;
            }
            //  Leg Equipment
            if (player.playerInventoryManager.underwearWearable != null)
            {
                //  Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.underwearWearable.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.underwearWearable.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.underwearWearable.fireDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.underwearWearable.holyDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.underwearWearable.lightningDamageAbsorption;

                //  Status Effect Resistance
                armorRobustness += player.playerInventoryManager.underwearWearable.robustness;
                armorVitality += player.playerInventoryManager.underwearWearable.vitality;
                armorImmunity += player.playerInventoryManager.underwearWearable.immunity;
                armorFocus += player.playerInventoryManager.underwearWearable.focus;

                //  Poise
                basePoiseDefense += player.playerInventoryManager.underwearWearable.poise;
            }
            //  Hand Equipment
            if (player.playerInventoryManager.pantsWearable != null)
            {
                //  Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.pantsWearable.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.pantsWearable.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.pantsWearable.fireDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.pantsWearable.holyDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.pantsWearable.lightningDamageAbsorption;

                //  Status Effect Resistance
                armorRobustness += player.playerInventoryManager.pantsWearable.robustness;
                armorVitality += player.playerInventoryManager.pantsWearable.vitality;
                armorImmunity += player.playerInventoryManager.pantsWearable.immunity;
                armorFocus += player.playerInventoryManager.pantsWearable.focus;

                //  Poise
                basePoiseDefense += player.playerInventoryManager.pantsWearable.poise;
            }
        }
    }
}