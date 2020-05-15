using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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
    private Team team;
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

    /* Spells Cast */
    public void HitUnit() {
        if (armySize <= 25) {
            Destroy(gameObject);
        } else {
            SetArmySize(armySize - 25);
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

    public static int GetTroopsDeployedNum(Building inOrigin)
    {
        float percentage = 0.5f;
        // If the building is player-owned, then the troop percentage is set to whichever button is currently toggled
        if (inOrigin.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam()))
        {
            percentage = GameManager.instance.GetTroopPercentage();
        }
        return (int)(inOrigin.GetArmySize() * percentage);
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

        switch (team.GetColor()) {
            case Team.Colors.Netural:
                throw new Exception("Custom Error: Netural Units cannot be created");
            case Team.Colors.Red:
                SetRendererColor(Color.red, Blueprints.RedStaticMaterial);
                break;
            case Team.Colors.Blue:
                SetRendererColor(Color.blue, Blueprints.BlueStaticMaterial);
                break;
            case Team.Colors.Green:
                SetRendererColor(Color.green, Blueprints.GreenStaticMaterial);
                break;
            case Team.Colors.Yellow:
                SetRendererColor(Color.yellow, Blueprints.YellowStaticMaterial);
                break;
        }

        void SetRendererColor(Color c, Material m) {
            gameObject.GetComponentInChildren<Renderer>().material = m;
            armySizeText.color = c;
        }
    }



    /* Other methods */
    public void Pause()
    {
        // ToDo: Store old velocity and set it in the UnPause method
        agent.velocity = new Vector3(0f, 0f, 0f);
        agent.isStopped = true;
    }

    public void UnPause() {
        agent.isStopped = false;
    }

    public static float spawnRadius = .5f;
    public static Unit ConstructUnit(Building inOrigin, Building inTarget)
    {
        Vector3 heading = inOrigin.transform.position - inTarget.transform.position;
        Vector3 headingScaled = (spawnRadius / heading.magnitude) * heading;
        Vector3 unitPosition = inOrigin.transform.position - headingScaled;

        Unit unitComponent = Instantiate(Blueprints.UnitStaticPrefab, unitPosition, Quaternion.identity).GetComponent<Unit>();
        unitComponent.transform.SetParent(GameManager.instance.GetUnitContainer());
        unitComponent.transform.LookAt(inTarget.transform);
        unitComponent.origin = inOrigin;
        unitComponent.target = inTarget;
        unitComponent.SetTeam(inOrigin.GetTeam());

        int numberOfDeployedTroops = GetTroopsDeployedNum(inOrigin);
        inOrigin.SetArmySize(inOrigin.GetArmySize() - numberOfDeployedTroops);
        unitComponent.SetArmySize(numberOfDeployedTroops);

        return unitComponent;
    }
}
