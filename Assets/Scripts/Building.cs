﻿using System.Collections;
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
    private Team.TeamName teamName = Team.TeamName.Netural;
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

    [SerializeField]
    private List<string> teamSharedMaterialList;
    private List<Material> teamMaterialList;

    /* Unity Methods */
    private void Start() {
        // Unpauses building
        isPaused = false;

        /*
        teamMaterialList = new List<Material>();
        for (int i = 0; i < buildingModelStages.Length; i++) {
            foreach (MeshRenderer mr in buildingModelStages[i].GetComponentsInChildren<MeshRenderer>()) {
                foreach (Material m in mr.materials) {
                    if (m.name == "Flag (Instance)") {
                        teamMaterialList.Add(m);
                    }
                }
            }
        }
        */
        teamMaterialList = new List<Material>();
        foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
            foreach(Material m in mr.materials) {
                // Debug.Log("test1: " + m.name);
                if (teamSharedMaterialList.Contains(m.name.Replace(" (Instance)", ""))) {
                    Debug.Log("test");
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
        SetTeam(teamName);

        // Start troop generation
        StartCoroutine(GenerateUnitsCoroutine());

        IEnumerator GenerateUnitsCoroutine()
        {
            while (!isPaused)
            {
                yield return new WaitForSeconds(1f);

                if (!isPaused && teamName != Team.TeamName.Netural)
                {
                    Team buildingTeam = GameManager.instance.GetTeam(teamName);

                    if (GetArmySize() < MaxGarrisonSize)
                    {
                        SetArmySize(armySize + TroopGenerationRate);
                    }
                    buildingTeam.SetGold(buildingTeam.GetGold() + GoldGenerationRate);
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

    public Team.TeamName GetTeamName()
    {
        return teamName;
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

    public void SetTeam(Team.TeamName inTeam)
    {
        teamName = inTeam;

        if (teamName == Team.TeamName.Netural)
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
            foreach (Material m in teamMaterialList) {
                m.color = color;
            }

            /*
            for(int i = 0; i < buildingModelStages.Length; i++)
            {
                foreach (Renderer renderer in buildingModelStages[i].GetComponentsInChildren<Renderer>())
                {
                    renderer.material.color = color;
                }
            }
            */
        }
    }


    private GameObject dustParticle;
    /* Other methods */
    public bool AttemptUpgrade() {
        if (GetBuildingLevel() < buildingModelStages.Length && // check for max level 
            GameManager.instance.GetTeam(teamName).GetGold() >= GetBuildingLevel() * 100)
        {
            StartCoroutine(UpgradeBuilding());

            return true;
        }

        return false;
    }

    private IEnumerator UpgradeBuilding() {
        if (dustParticle == null) {
            dustParticle = Instantiate(Blueprints.DustParticleStaticPrefab);
            dustParticle.transform.position = transform.position;
        }
        dustParticle.SetActive(true);

        yield return new WaitForSeconds(.5f);

        GameManager.instance.GetTeam(teamName).SetGold(GameManager.instance.GetTeam(teamName).GetGold() - GetBuildingLevel() * 100);
        SetBuildingLevel(GetBuildingLevel() + 1);

        yield return new WaitForSeconds(.5f);

        dustParticle.SetActive(false);
    }

    private bool AttemptAttackOnBuilding(Unit attacker)
    {
        // Check to see if the building is the correct target
        if (attacker.GetTarget() == this)
        {
            int newArmySize;
            // Army Transfer
            if (attacker.GetTeamName().Equals(teamName))
            {
                newArmySize = attacker.GetArmySize() + GetArmySize();
            }
            // Army Attack 
            else
            {
                newArmySize = GetArmySize() - attacker.GetArmySize();
                if (newArmySize < 0)
                {
                    SetTeam(attacker.GetTeamName());
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
        SetTeam(teamName);
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
