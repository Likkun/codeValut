using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIState
{
    Idle,
    itemSelected,
}

public class InventoryController : MonoBehaviour
{

    GameObject[] Slots;
    GameObject[] SlotImgs;
    List<Sprite> ItmImages;

    public GameObject SelectedDialogue;
    public GameObject SelectCarrat;
    public GameObject UseText;
    public GameObject EquipText;
    public GameObject InfoText;

    public InventoryUIState currentState = InventoryUIState.Idle;

    public int SelectedSlot = 0;    
    public string SelectedState = "USE";

    public Color selected;
    public Color unselected;

    public InputControls mControls;

    public bool dialougePause = false;

	private void Awake()
	{
        mControls = new InputControls();

        mControls.UI.Navigate.performed += __ => HandleUINavigate(__.ReadValue<Vector2>());
        mControls.UI.Submit.performed += __ => HandleSelect();
        mControls.UI.Cancel.performed += __ => HandleCancel();
        mControls.UI.Inventory.performed += __ => HandleInventoryToggle();
    }

	public void HandleInventoryToggle()
	{
        if (this.gameObject.activeSelf)
        {
            GameManager.Instance.ToggleInventory();
        }
	}

	// Start is called before the first frame update
	void Start()
    {
        Slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        SlotImgs = GameObject.FindGameObjectsWithTag("InventorySlotImg");
        Sprite[] temp = Resources.LoadAll<Sprite>("Items");

        ItmImages = new List<Sprite>();

        foreach (Sprite s in temp)
        {
            ItmImages.Add(s);
        }

       
    }

	public void HandleSelect()
	{
        if (currentState == InventoryUIState.Idle)
        {
            //
            //BASE INVENTORY STATE
            //
            currentState = InventoryUIState.itemSelected;
            SelectedDialogue.SetActive(true);
            SelectedDialogue.transform.position = Slots[SelectedSlot].gameObject.transform.position;
        }
        else if (currentState == InventoryUIState.itemSelected)
		{
            InventoryItem selectedItem = GameManager.Instance.currentGameState.CurrentInventory[SelectedSlot];
            switch (SelectedState)
            {
                case "USE":
                    if (selectedItem.UseScript != null && selectedItem.UseScript.DialogueList.Count > 0)
                    {
                        DialogueManager.Instance.StartDialogue(selectedItem.UseScript.GetCurrentDialogue());
                        InventoryItem temp = GameManager.Instance.currentGameState.CurrentInventory.Find(itm => itm.Name == selectedItem.Name);
                        temp.Quantity--;
                        if (temp.Quantity == 0)
                        {
                            GameManager.Instance.currentGameState.CurrentInventory.Remove(temp);
                        }
                        currentState = InventoryUIState.Idle;
                        SelectedDialogue.SetActive(false);
                    }
                    break;
                case "EQUIP":
                    break;
                case "INFO":
                    if (selectedItem.InfoScript != null && selectedItem.InfoScript.DialogueList.Count > 0)
                    {
                        DialogueManager.Instance.StartDialogue(selectedItem.InfoScript.GetCurrentDialogue());
                    }

                    break;
            }
        }
    }

	public void HandleCancel()
	{
        if (currentState == InventoryUIState.itemSelected)
        {
            currentState = InventoryUIState.Idle;
            SelectedDialogue.SetActive(false);
        }
        else if( currentState == InventoryUIState.Idle)
		{
            //Should close Invenotory...
            GameManager.Instance.ToggleInventory();
		}
    }

	public void HandleUINavigate(Vector2 direction)
	{
        if (currentState == InventoryUIState.Idle)
        {
            if (direction == Vector2.up)
            {
                SelectedSlot = Mathf.Max(SelectedSlot - 5, 0);
            }
            else if (direction == Vector2.left)
            {
                SelectedSlot = Mathf.Max(SelectedSlot - 1, 0);
            }
            else if (direction == Vector2.down)
            {
                SelectedSlot = Mathf.Min(SelectedSlot + 5, Slots.Length - 1);
            }
            else if (direction == Vector2.right)
            {
                SelectedSlot = Mathf.Min(SelectedSlot + 1, Slots.Length - 1);
            }
        }
        else if (currentState == InventoryUIState.itemSelected)
		{
            if (direction == Vector2.up)
            {
                if (SelectedState == "EQUIP" || SelectedState == "USE")
                {
                    SelectCarrat.transform.position = new Vector3(SelectCarrat.transform.position.x,
                                                                    UseText.transform.position.y,
                                                                    0);
                    SelectedState = "USE";
                }
                else
                {
                    SelectCarrat.transform.position = new Vector3(SelectCarrat.transform.position.x,
                                                                    EquipText.transform.position.y,
                                                                    0);
                    SelectedState = "EQUIP";
                }
            }
            else if (direction == Vector2.down)
            {
                if (SelectedState == "INFO" || SelectedState == "EQUIP")
                {
                    SelectCarrat.transform.position = new Vector3(SelectCarrat.transform.position.x,
                                                                    InfoText.transform.position.y,
                                                                    0);
                    SelectedState = "INFO";
                }
                else
                {
                    SelectCarrat.transform.position = new Vector3(SelectCarrat.transform.position.x,
                                                                    EquipText.transform.position.y,
                                                                    0);
                    SelectedState = "EQUIP";
                }
            }
        }

    }

	private void OnEnable()
    {
        SelectedSlot = 0;
        SelectedState = "USE";

        mControls.Enable();

        DialogueManager.OnSetDialogueState += HandleDialogueState;

    }

	private void OnDisable()
	{
        mControls.Enable();
        DialogueManager.OnSetDialogueState -= HandleDialogueState;
    }

    public void HandleDialogueState(bool active, bool pause)
	{
        dialougePause = pause;
	}

	// Update is called once per frame
	void Update()
    {

        //handling input for inventory
        if( GameManager.Instance.inventoryOpen && DialogueManager.Instance.DialoguePauseWorld == false)
        {
           
        }

        foreach( GameObject g in Slots)
        {
            g.GetComponent<Image>().color = unselected;
        }

        Slots[SelectedSlot].GetComponent<Image>().color = selected;

        //this logic should be moved to an event???
        foreach ( GameObject go in SlotImgs)
        {
            go.GetComponent<Image>().enabled = false;
        }

        List<string> stackedNames = new List<string>();

        for( int i=0; i<= GameManager.Instance.currentGameState.CurrentInventory.Count - 1; i++)
        {
            InventoryItem invitm = GameManager.Instance.currentGameState.CurrentInventory[i];
            
            SlotImgs[i].GetComponentInChildren<Image>().enabled = true;
            SlotImgs[i].GetComponentInChildren<Image>().sprite = ItmImages.Find(img => img.name == invitm.SpriteName);

            stackedNames.Add(invitm.SpriteName);

            if( invitm.Quantity > 1)
            {
                Slots[i].GetComponentInChildren<Text>().enabled = true;
                Slots[i].GetComponentInChildren<Text>().text = invitm.Quantity.ToString();
            }
            else
            {
                Slots[i].GetComponentInChildren<Text>().enabled = false;
                Slots[i].GetComponentInChildren<Text>().text = "1";
            }

                       
        }
    }
}
