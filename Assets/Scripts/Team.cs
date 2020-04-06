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

    private int food;
    private int gold;

    public Team(TeamName inTeamName, bool isPlayerControlled)
    {
        SetFood(100);
        SetGold(100);

        teamName = inTeamName;
        if (!isPlayerControlled)
        {
            aIController = new AIController(inTeamName);
        }
    }

    /* Getters */
    public int GetFood()
    {
        return food;
    }

    public int GetGold()
    {
        return gold;
    }

    public TeamName GetTeamOption()
    {
        return teamName;
    }

    /* Setters */
    public void SetFood(int inFood)
    {
        food = inFood;
        if (aIController == null)
        {
            GameManager.instance.uIController.SetFoodResourceText(food);
        }
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
