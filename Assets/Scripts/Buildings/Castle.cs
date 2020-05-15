public class Castle : Building
{
    protected override int StartingMaxGarrisonSize { get { return 100; } }
    protected override int StartingTroopGenerationRate { get { return 10; } }
    protected override int StartingGoldGenerationRate { get { return 2; } }
}
