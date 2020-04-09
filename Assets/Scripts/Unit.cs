using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
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
            SetRendererColor(Color.gray);
        }
        else
        {
            if (teamName == Team.TeamName.Red)
            {
                SetRendererColor(Color.red);
            }
            if (teamName == Team.TeamName.Blue)
            {
                SetRendererColor(Color.blue);
            }
            if (teamName == Team.TeamName.Green)
            {
                SetRendererColor(Color.green);
            }
            if (teamName == Team.TeamName.Yellow)
            {
                SetRendererColor(Color.yellow);
            }
        }

        void SetRendererColor(Color color)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }
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
        unitComponent.origin = inOrigin;
        unitComponent.target = inTarget;
        unitComponent.SetTeam(inOrigin.GetTeamName());

        int numberOfDeployedTroops = (int) (inOrigin.GetArmySize() / 2);
        inOrigin.SetArmySize(inOrigin.GetArmySize() - numberOfDeployedTroops);
        unitComponent.SetArmySize(numberOfDeployedTroops);

        return unitComponent;
    }
}
