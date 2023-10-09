using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// This class should hold all data about the game
/// Story Points
/// Inventory Info
/// Health / Player State Info
/// </summary>
[Serializable]
public class GameState
{

    public List<string> CompletedStoryPoints;
    public List<InventoryItem> CurrentInventory;
    public PlayerState PlayerState;

    public List<LevelState> LevelStates;

    public int HumansKilledByEnemies = 0;
    public int HumansKilledByPlayer_Directly = 0;
    public int HumansKilledByPlayer_PassOut = 0;
    public int HumansSaved = 0;

    public GameState()
    {
        CompletedStoryPoints = new List<string>();
        CurrentInventory = new List<InventoryItem>();
        LevelStates = new List<LevelState>();
        PlayerState = new PlayerState();
    }

    public GameState( GameState newState )
    {
        this.CompletedStoryPoints = newState.CompletedStoryPoints;
        this.CurrentInventory = newState.CurrentInventory;
        this.PlayerState = newState.PlayerState;
    }
}

[Serializable]
public class InventoryItem
{
    public int ID;
    public string Name;
    public int Quantity;
    public bool IsKeyItem;
    public string SpriteName;
    public bool AutoUse;
    public DialogueControl UseScript;
    public DialogueControl EquipScript;
    public DialogueControl InfoScript;
}

[Serializable]
public class PlayerState
{
    public int CurrentHealth;
    public int MaxHealth;

    public int CurrentWillPower;
    public int MaxWillPower;

    public PlayerState()
    {
        CurrentHealth = 10;
        MaxHealth = 10;

        CurrentWillPower = 10;
        MaxWillPower = 10;
    }    
}

public enum HumanKillReason
{
    None,
    Enemy,
    Player,
    PlayerPassout,
};

[Serializable]
public class HumanState
{
    public int ID;
    public string Level;
    public bool Alive;
    public bool Saved;
    public HumanKillReason CauseOfDeath;

    public HumanState()
	{
        ID = -1;
        Level = "-0";
        Alive = true;
        Saved = false;
        CauseOfDeath = HumanKillReason.None;
	}
}

[Serializable]
public class LevelState
{
    public string ID;
    public List<HumanState> HumanStates;

    public LevelState()
    {
        ID = "-0";
        HumanStates = new List<HumanState>();
    }
}