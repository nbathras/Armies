using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public abstract class Building : MonoBehaviour
{
    protected abstract int StartingMaxGarrisonSize { get; }
    protected abstract int StartingTroopGenerationRate { get; }

    public abstract int MaxGarrisonSize { get; }
    public abstract int TroopGenerationRate { get; }

    [SerializeField]
    private int buildingLevel = 1;
    [SerializeField]
    private int armySize = 40;
    [SerializeField]
    private Team team = Team.Netural;
    // Controls selectability, troop generation, attack and defense
    [SerializeField]
    private bool isPaused = false;

    // Inspector Set Game Objects
    [SerializeField]
    private GameObject[] buildingModelStages;
    [SerializeField]
    private TextMeshPro armySizeText;
    [SerializeField]
    private GameObject selectionCircle;

    /* Unity Methods */
    private void Awake() {
        // Unpauses building
        isPaused = false;

        // Remove selection circle
        selectionCircle.SetActive(false);

        // Sets the intial size of the army
        SetArmySize(armySize);

        // Sets the intial level of the building
        SetBuildingLevel(buildingLevel);

        // Sets the intial team
        SetTeam(team);

        // Start troop generation
        StartCoroutine(GenerateUnitsCoroutine());

        IEnumerator GenerateUnitsCoroutine()
        {
            while (!isPaused)
            {
                yield return new WaitForSeconds(1f);

                if (!isPaused && team != Team.Netural &&
                    GetArmySize() < MaxGarrisonSize)
                {
                    SetArmySize(armySize + TroopGenerationRate);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
        {
            AttemptAttackOnBuilding(collision.gameObject.GetComponent<Unit>());
        }
    }



    /* Getters */
    public int GetArmySize()
    {
        return armySize;
    }

    public int GetBuildingLevel()
    {
        return buildingLevel;
    }

    public Team GetTeam()
    {
        return team;
    }



    /* Setters */
    public void SetArmySize(int inArmySize)
    {
        armySize = inArmySize;
        armySizeText.SetText(GetArmySize().ToString());
    }

    private void SetBuildingLevel(int inBuildingLevel)
    {
        buildingLevel = inBuildingLevel;

        for (int i = 0; i < buildingModelStages.Length; i++)
        {
            buildingModelStages[i].SetActive(i == buildingLevel - 1);
        }
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
            for(int i = 0; i < buildingModelStages.Length; i++)
            {
                buildingModelStages[i].GetComponentInChildren<Renderer>().material.color = color;
            }
        }
    }



    /* Other methods */
    public bool AttemptUpgrade() {
        if (GetArmySize() >= MaxGarrisonSize / 2 && GetBuildingLevel() < buildingModelStages.Length) {
            SetArmySize(GetArmySize() - MaxGarrisonSize / 2);

            SetBuildingLevel(GetBuildingLevel() + 1);

            return true;
        }

        return false;
    }

    private bool AttemptAttackOnBuilding(Unit attacker)
    {
        // Check to see if the building is the correct target
        if (attacker.GetTarget() == this)
        {
            int newArmySize;
            // Army Transfer
            if (attacker.GetTeam().Equals(team))
            {
                newArmySize = attacker.GetArmySize() + GetArmySize();
            }
            // Army Attack 
            else
            {
                newArmySize = GetArmySize() - attacker.GetArmySize();
                if (newArmySize < 0)
                {
                    SetTeam(attacker.GetTeam());
                    newArmySize *= -1;

                    if (GetBuildingLevel() > 1)
                    {
                        SetBuildingLevel(GetBuildingLevel() - 1);
                    }
                }
            }

            // Sets the newly calculated army size for the buildig
            SetArmySize(newArmySize);

            // Destorys attacking unit
            Destroy(attacker.gameObject);

            return true;
        }

        return false;
    }

    public void Select() {
        selectionCircle.SetActive(true);
    }

    public void DeSelect() {
        selectionCircle.SetActive(false);
        SetTeam(team);
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void UnPause()
    {
        isPaused = false;
    }
}
