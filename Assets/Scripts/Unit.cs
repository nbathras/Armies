using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Material yellowMaterial;
    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material blueMaterial;

    [SerializeField]
    private int armySize;
    [SerializeField]
    private float locomationAnimationSmoothTime = .1f;

    // Inspector Set Game Objects
    [SerializeField]
    private TextMeshPro armySizeText;

    // Script Set Game Objects
    private NavMeshAgent agent;
    private Animator animator;
    private Team.TeamName teamName;
    private Building origin;
    private Building target;

    // The max amount of troops leaving from the players' garrisons in a single click
    private static int maxTroopBlockSize;

    // The player's team
    private static Team.TeamName playerTeamName;

    private void Awake() {
        // Get NavMeshAgent Component
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.gameObject.transform.position);
        }

        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
    }

    /* Getters */
    public int GetArmySize()
    {
        return armySize;
    }

    public Team.TeamName GetTeamName()
    {
        return teamName;
    }

    public Building GetOrigin()
    {
        return origin;
    }

    public Building GetTarget()
    {
        return target;
    }

    public int getMaxTroopBlockSize()
    {
        return maxTroopBlockSize;
    }


    /* Setters */
    public void SetArmySize(int inArmySize)
    {
        armySize = inArmySize;
        armySizeText.SetText(GetArmySize().ToString());
    }

    public void SetTeam(Team.TeamName inTeam)
    {
        teamName = inTeam;

        if (teamName== Team.TeamName.Netural)
        {
            // SetRendererColor(Color.gray);
        }
        else
        {
            if (teamName == Team.TeamName.Red)
            {
                // SetRendererColor(Color.red);
                gameObject.GetComponentInChildren<Renderer>().material = redMaterial;
                armySizeText.color = Color.red;
            }
            if (teamName == Team.TeamName.Blue)
            {
                // SetRendererColor(Color.blue);
                gameObject.GetComponentInChildren<Renderer>().material = blueMaterial;
                armySizeText.color = Color.blue;
            }
            if (teamName == Team.TeamName.Green)
            {
                // SetRendererColor(Color.green);
                gameObject.GetComponentInChildren<Renderer>().material = greenMaterial;
                armySizeText.color = Color.green;
            }
            if (teamName == Team.TeamName.Yellow)
            {
                // SetRendererColor(Color.yellow);
                gameObject.GetComponentInChildren<Renderer>().material = yellowMaterial;
                armySizeText.color = Color.yellow;
            }
        }

        /*
        void SetRendererColor(Color color)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }
        */
    }
    
    // Sets the player's max troop block size
    public static void setMaxTroopBlockSize(int blockSize)
    {
        maxTroopBlockSize = blockSize;
    }

    // Sets the player's team
    public static void setPlayerTeam(Team.TeamName team)
    {
        playerTeamName = team;
    }


    /* Other methods */
    public void StopUnit()
    {
        agent.velocity = new Vector3(0f, 0f, 0f);
        agent.isStopped = true;
    }

    public static float spawnRadius = .5f;
    public static Unit ConstructUnit(Building inOrigin, Building inTarget)
    {
        Vector3 heading = inOrigin.transform.position - inTarget.transform.position;
        Vector3 headingScaled = (spawnRadius / heading.magnitude) * heading;
        Vector3 unitPosition = inOrigin.transform.position - headingScaled;

        Unit unitComponent = Instantiate(Blueprints.unitStaticPrefab, unitPosition, Quaternion.identity).GetComponent<Unit>();
        unitComponent.transform.SetParent(GameManager.instance.unitContainer.transform);
        unitComponent.transform.LookAt(inTarget.transform);
        unitComponent.origin = inOrigin;
        unitComponent.target = inTarget;
        unitComponent.SetTeam(inOrigin.GetTeamName());

        int numberOfDeployedTroops = (int) (inOrigin.GetArmySize() / 2);
        inOrigin.SetArmySize(inOrigin.GetArmySize() - numberOfDeployedTroops);
        unitComponent.SetArmySize(numberOfDeployedTroops);

        return unitComponent;
    }
}
