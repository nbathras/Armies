using UnityEngine;

public class Team : MonoBehaviour
{
    public enum Colors
    {
        Netural,
        Red,
        Blue,
        Green,
        Yellow,
    }

    [SerializeField]
    private Colors color;
    [SerializeField]
    private int gold = 80; // initial value is the starting gold
    // [SerializeField]
    // private AIDifficulity aIDifficulty = AIDifficulty.Medium; // ToDo: Impelement AI difficulty based on reaction time and abilities and stringable commands 

    private AIController aIController; // will be created at runtime

    private void Start() {
        if (!(GameManager.instance.GetPlayerControlledTeam().Equals(this) || IsNeutral())) {
            aIController = new AIController(this);
        }
    }

    public bool HasBuildings() {
        foreach (Building building in GameManager.instance.GetBuildingsList()) {
            if (building.GetTeam().Equals(this)) {
                return true;
            }
        }
        return false;
    }


    /* Getters */
    public int GetGold() {
        return gold;
    }

    public Colors GetColor() {
        return color;
    }

    public bool IsNeutral() {
        return color == Colors.Netural;
    }

    /* Setters */
    public void SetGold(int inGold) {
        // set the gold
        gold = inGold;

        // Update UI if team is player controlled
        if (GameManager.instance.GetPlayerControlledTeam().Equals(this)) {
            GameManager.instance.uIController.SetGoldResourceText(gold);
        }
    }

    /* Object override methods */
    public override bool Equals(object other) {
        if ((other == null) || !this.GetType().Equals(other.GetType())) {
            return false;
        } else {
            Team team = (Team) other;
            return team.color == color;
        }
    }

    public override int GetHashCode() {
        return (int) color;
    }

    public override string ToString() {
        return color.ToString() + " Team";
    }
}
