using UnityEngine;

public class Team
{
    public enum TeamOption
    {
        Netural,
        Red,
        Blue,
        Green,
        Yellow,
    }

    private TeamOption teamOption;
    public AIController aIController;

    private int food;
    private int gold;

    public Team(TeamOption inTeamOption, bool isPlayerControlled)
    {
        SetFood(100);
        SetGold(100);

        teamOption = inTeamOption;
        if (!isPlayerControlled)
        {
            aIController = new AIController(inTeamOption);
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

    public TeamOption GetTeamOption()
    {
        return teamOption;
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
