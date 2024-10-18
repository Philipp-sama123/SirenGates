namespace KrazyKatGames
{
    public class AIBossCharacterNetworkManager : AICharacterNetworkManager
    {
        private AIBossCharacterManager aiBossCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIBossCharacterManager>();
        }
        public override void CheckHP(int oldValue, int newValue)
        {
            base.CheckHP(oldValue, newValue);
            
            if (aiBossCharacter.IsOwner)
            {
                if (currentHealth.Value <= 0)
                    return;
                
                float healthNeededForShift = 0;
                
                healthNeededForShift = maxHealth.Value * (aiBossCharacter.minimumHealthPercentToShift / 100);
                if (currentHealth.Value <= healthNeededForShift)
                {
                    aiBossCharacter.PhaseShift();
                }
            }
        }
    }
}