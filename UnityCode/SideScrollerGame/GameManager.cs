using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	//********** EVENTS **************
	
	//Event to Clear/Destroy Memory Cards of a certain Type.
	public delegate void ClearMatchedType(CardController.CARD_FRONT CardType);
	public static event ClearMatchedType OnClearMatchedCards;

	//Event to Flip all Memory Cards back to 'face-down'
	public delegate void AllCardsFaceDown();
	public static event AllCardsFaceDown OnAllCardsFaceDown;

	public delegate void ResetCardGame();
	public static event ResetCardGame OnResetCardGame;

	public delegate void UpdateHumanTiral(int humanId);
	public static event UpdateHumanTiral OnUpdateHumanTrial;

	//*********END EVENTS*************

	//*********SHARED VARIABLES*************

	private static GameManager _instance;
    public static GameManager Instance { get { return _instance;  } }

	public GameState currentGameState;
	public bool inventoryOpen;
	public InventoryController InventoryController;

	public InputControls mControls;

	public bool isDialogueActive = false;

	//*********END SHARED VARIABLES***********

	//*********CARD MINI GAME VARS***********************
	public bool MemoryGameActive = false;

	public Dictionary<CardController.CARD_FRONT, int> Flipped = new Dictionary<CardController.CARD_FRONT, int>();
	public int MaxCardsFlipped;
	public int RequiredMatches = 2;
	public int CardsCleared = 0;
	public int MaxCardsCleared = 4;
	public int CurrentMatch;

	public int currentHumanTrial = -1;

	public GameObject Four_X_Four_CardBoard;

	//*********END CARD MINI GAME VARS********************
	/// ///////////////////////////////////////////////////
	//*********SIDE SCROLL GAME VARS***********************

	//Human Counter
	public int HumansInCurrentLevel = 0;
	public int HumansKilled_CurrentLevel = 0;
	public int HumansSaved_CurrentLevel = 0;

	public int CurrentLevel = -1;

	//*********END SIDE SCROLL GAME VARS*******************

	private void Awake()
	{
		if( _instance != null && _instance != this)
		{
            Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		mControls = new InputControls();
		mControls.Player.Inventory.performed += __ => ToggleInventory();
		//mControls.Player.MemoryGame.performed += __ => ToggleCardGame();
		
		currentGameState = SaveSystem.LoadData();
		if (currentGameState == null)
		{
			currentGameState = new GameState();
		}

	}

	public void ToggleInventory()
	{
		if(!InventoryController.gameObject.activeSelf) 
		{
			mControls.Disable();
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TogglePauseMovements();
		}
		else
		{
			mControls.Enable();
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TogglePauseMovements();
		}
		InventoryController.gameObject.SetActive(!InventoryController.gameObject.activeSelf);
		
	}

	private void OnEnable()
	{
		CardController.OnCardFlipped += HandleCardFlipped;
		DialogueManager.OnSetDialogueState += setDialogueActive;
		mControls.Enable();
	}

	private void OnDisable()
	{
		CardController.OnCardFlipped -= HandleCardFlipped;
		DialogueManager.OnSetDialogueState -= setDialogueActive;
		mControls.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void RegisterHuman( int ID )
	{
		//Check Save Game State
		if(currentGameState.LevelStates[CurrentLevel].HumanStates[ID] != null)
		{
			//Return Human state to Caller?
		}
		else
		{
			//Update Save Data?
		}
	}

	void setDialogueActive(bool isActive, bool worldPause)
	{
		isDialogueActive = isActive;
	}

	void HumanKilled( bool wasPlayer, bool passedOut )
	{
		HumansInCurrentLevel--;
		HumansKilled_CurrentLevel++;

		if (wasPlayer)
		{
			if (passedOut)
			{
				currentGameState.HumansKilledByPlayer_PassOut++;
			}
			else
			{
				currentGameState.HumansKilledByPlayer_Directly++;
			}
			
		}
	}

	void SaveHuman()
	{
		HumansInCurrentLevel--;
		HumansSaved_CurrentLevel++;

		currentGameState.HumansSaved++;
	}

	void HandleCardFlipped(bool isFront, CardController.CARD_FRONT CardType )
	{
		if( !Flipped.ContainsKey(CardType))
		{
			Flipped.Add(CardType, 0);
		}

		if (isFront)
		{
			Flipped[CardType] = Flipped[CardType] + 1;
		}
		else
		{
			Flipped[CardType] = Mathf.Max(Flipped[CardType] - 1,0);
		}

		int totalFlipped = 0;
		foreach( KeyValuePair<CardController.CARD_FRONT,int> kvp in Flipped)
		{
			totalFlipped ++;
		}

		if (Flipped[CardType] >= RequiredMatches)
		{
			//Clear the cards....
			OnClearMatchedCards(CardType);
			CardsCleared += 2;
			if( CardsCleared >= MaxCardsCleared )
			{
				ToggleCardGame();
			}

			if (currentHumanTrial > -1)
			{
				OnUpdateHumanTrial(currentHumanTrial);
			}
		}

		if ( totalFlipped >= MaxCardsFlipped)
		{
			//Flip all cards back
			OnAllCardsFaceDown();
			Flipped.Clear();
		}

		
		
	}

	public void SaveGame()
	{
		SaveSystem.SaveData(currentGameState);
	}

	public void ToggleCardGame()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TogglePauseMovements();
		if (Four_X_Four_CardBoard != null)
		{
			Four_X_Four_CardBoard.SetActive(!Four_X_Four_CardBoard.activeSelf);
		}
	}
}
