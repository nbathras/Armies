using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Building
{
    protected override int StartingMaxGarrisonSize { get { return 100; } }
    protected override int StartingTroopGenerationRate { get { return 10; } }

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
