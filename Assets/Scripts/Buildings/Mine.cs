public class Mine : Building
{
    protected override int StartingMaxGarrisonSize { get { return 50; } }
    protected override int StartingTroopGenerationRate { get { return 1; } }
    protected override int StartingGoldGenerationRate { get { return 10; } }
}
