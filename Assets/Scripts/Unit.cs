using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Team team = Team.Netural;
    [SerializeField]
    private int armySize;

    // Inspector Set Game Objects
    [SerializeField]
    private TextMeshPro armySizeText;

    // Script Set Game Objects
    private NavMeshAgent agent;
    private Building origin;
    private Building target;

    private void Awake() {
        // Get NavMeshAgent Component
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.gameObject.transform.position);
        }
    }

    /* Getters */
    public int GetArmySize()
    {
        return armySize;
    }

    public Team GetTeam()
    {
        return team;
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

    public void SetTeam(Team inTeam)
    {
        team = inTeam;

        if (team == Team.Netural)
        {
            SetRendererColor(Color.gray);
        }
        else
        {
            if (team == Team.Red)
            {
                SetRendererColor(Color.red);
            }
            if (team == Team.Blue)
            {
                SetRendererColor(Color.blue);
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
        unitComponent.SetTeam(inOrigin.GetTeam());

        int numberOfDeployedTroops = (int) (inOrigin.GetArmySize() / 2);
        inOrigin.SetArmySize(inOrigin.GetArmySize() - numberOfDeployedTroops);
        unitComponent.SetArmySize(numberOfDeployedTroops);

        return unitComponent;
    }
}
