namespace KrazyKatGames
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        private PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public override void PlayBlockSoundFX()
        {
            base.PlayBlockSoundFX();
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));
        }
    }
}