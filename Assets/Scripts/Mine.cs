public class Mine : Building
{
    protected override int StartingMaxGarrisonSize { get { return 50; } }
    protected override int StartingTroopGenerationRate { get { return 2; } }

    public override int MaxGarrisonSize
    {
        get
        {
            return GetBuildingLevel() * StartingMaxGarrisonSize;
        }
    }

    public override int TroopGenerationRate
    {
        get
        {
            return GetBuildingLevel() * StartingTroopGenerationRate;
        }
    }
}
