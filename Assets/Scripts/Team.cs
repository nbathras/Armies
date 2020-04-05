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

    public TeamOption teamOption;
    public AIController aIController;

    public Team(TeamOption inTeamOption, bool isPlayerControlled)
    {
        teamOption = inTeamOption;
        if (!isPlayerControlled)
        {
            aIController = new AIController(inTeamOption);
        }
    }
}
