using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    // ToDo:
    // Should create a Building Controller that handles all Coroutines.
    // I think to many Coroutines are running at the moment

    protected abstract int StartingMaxGarrisonSize { get; }
    protected abstract int StartingTroopGenerationRate { get; }
    protected abstract int StartingGoldGenerationRate { get; }

    public int MaxGarrisonSize
    {
        get
        {
            return GetBuildingLevel() * StartingMaxGarrisonSize;
        }
    }

    public int TroopGenerationRate
    {
        get
        {
            return GetBuildingLevel() * StartingTroopGenerationRate;
        }
    }

    public int GoldGenerationRate
    {
        get
        {
            return GetBuildingLevel() * StartingGoldGenerationRate;
        }
    }

    [SerializeField]
    private int buildingLevel = 1;
    [SerializeField]
    private int armySize = 40;
    [SerializeField]
    private Team team;
    // Controls selectability, troop generation, attack and defense

    // ToDo: Allow for individual buildings to be paused?
    // [SerializeField]
    // private bool isPaused = false;

    // Inspector Set Game Objects
    [SerializeField]
    private GameObject[] buildingModelStages;
    [SerializeField]
    private TextMeshPro armySizeText;
    [SerializeField]
    private GameObject selectionCircle;
    [SerializeField]
    private GameObject dustParticle;

    [SerializeField]
    private List<string> teamSharedMaterialList;
    private List<Material> teamMaterialList;

    /* Unity Methods */
    private void Start() {
        teamMaterialList = new List<Material>();
        foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
            foreach(Material m in mr.materials) {
                if (teamSharedMaterialList.Contains(m.name.Replace(" (Instance)", ""))) {
                    teamMaterialList.Add(m);
                }
            }
        }

        // Remove selection circle
        selectionCircle.SetActive(false);

        // Sets the intial size of the army
        SetArmySize(armySize);

        // Sets the intial level of the building
        SetBuildingLevel(buildingLevel);

        // Sets the intial team
        if (team == null) {
            throw new Exception("Custom Exception: team must be assigned in inspector");
        }
        SetTeam(team);

        // Start troop generation
        StartCoroutine(GenerateUnitsCoroutine());

        IEnumerator GenerateUnitsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                // only generate troops if game is unpaused and the building is not neutral
                if (!GameManager.instance.IsGamePaused() && !team.IsNeutral())
                {
                    // Do not add anymore troops if at max garrison, but do not remove troops
                    if (GetArmySize() < MaxGarrisonSize)
                    {
                        // only generate up to the max garrison
                        int newArmySize = armySize + TroopGenerationRate;
                        if (newArmySize > MaxGarrisonSize) {
                            SetArmySize(MaxGarrisonSize);
                        } else {
                            SetArmySize(newArmySize);
                        }
                    }

                    team.SetGold(team.GetGold() + GoldGenerationRate);
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

        switch(team.GetColor()) {
            case Team.Colors.Netural:
                SetRendererColor(Color.gray);
                break;
            case Team.Colors.Red:
                SetRendererColor(Color.red);
                break;
            case Team.Colors.Blue:
                SetRendererColor(Color.blue);
                break;
            case Team.Colors.Green:
                SetRendererColor(Color.green);
                break;
            case Team.Colors.Yellow:
                SetRendererColor(Color.yellow);
                break;
        }

        void SetRendererColor(Color color)
        {
            foreach (Material m in teamMaterialList) {
                m.color = color;
            }
            armySizeText.color = color;
        }
    }

    /* Other methods */
    public bool Upgrade() {
        int upgradeCost = 100 + (buildingLevel - 1) * 50;

        if (GetBuildingLevel() < buildingModelStages.Length && // check for max level 
            team.GetGold() >= upgradeCost)
        {
            StartCoroutine(UpgradeCoroutine(upgradeCost));

            return true;
        }

        return false;

        IEnumerator UpgradeCoroutine(int cost) {
            dustParticle.SetActive(true);

            team.SetGold(team.GetGold() - cost);

            yield return new WaitForSeconds(.5f);

            SetBuildingLevel(buildingLevel + 1);

            yield return new WaitForSeconds(.5f);

            dustParticle.SetActive(false);
        }
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
        // SetTeam(teamName);
    }

    public void Pause() { }

    public void UnPause() { }
}
