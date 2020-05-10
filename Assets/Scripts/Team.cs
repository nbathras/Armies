using UnityEngine;

public class Team
{
    public enum TeamName
    {
        Netural,
        Red,
        Blue,
        Green,
        Yellow,
    }

    private readonly TeamName teamName;
    public AIController aIController;

    private int gold;

    public Team(TeamName inTeamName, bool isPlayerControlled)
    {
        SetGold(50);

        teamName = inTeamName;
        if (!isPlayerControlled)
        {
            aIController = new AIController(inTeamName);
        }
    }

    /* Getters */
    public int GetGold()
    {
        return gold;
    }

    public TeamName GetTeamOption()
    {
        return teamName;
    }

    public void SetGold(int inGold)
    {
        gold = inGold;
        if (aIController == null)
        {
            GameManager.instance.uIController.SetGoldResourceText(gold);
        }
    }
}
