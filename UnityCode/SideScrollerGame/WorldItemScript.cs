using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldItemScript : MonoBehaviour
{
    public bool ClickToPickup = true;
    public int ItemID = 0;
    public string ItemName = "Pretzel";
    public string SpriteName = "Pretzel";
    private bool playerTouching = false;

    public GameObject InteractPrefab;
    public GameObject InteractUI;

    public float InteractUIYOffset = 0.0f;

    public TextAsset DialogueJSON;

    public string PickupScriptName;
    public string UseScriptName;
    public string InfoScriptName;
    public string EquipScriptName;

    public DialogueControl PickupDialogue;
    public DialogueControl UseDialogue;
    public DialogueControl InfoDialouge;
    public DialogueControl EquipDialogue;

    public InputControls mControls;
    private void Awake()
    {
        if (DialogueJSON != null)
        {
            PickupDialogue = JsonUtility.FromJson<DialogueControl>(DialogueJSON.text);
        }

        if(!string.IsNullOrEmpty(PickupScriptName))
        {
            string dJSON = Resources.Load<TextAsset>("Dialogues/Items/" + PickupScriptName).text;
            PickupDialogue = JsonUtility.FromJson<DialogueControl>(dJSON);

            Debug.Log(PickupDialogue.DialogueList[0].Dialogue[0].PauseWorld);

        }
        if (!string.IsNullOrEmpty(UseScriptName))
        {
            string dJSON = Resources.Load<TextAsset>("Dialogues/Items/" + UseScriptName).text;
            UseDialogue = JsonUtility.FromJson<DialogueControl>(dJSON);
        }
        if (!string.IsNullOrEmpty(InfoScriptName))
        {
            string dJSON = Resources.Load<TextAsset>("Dialogues/Items/" + InfoScriptName).text;
            InfoDialouge = JsonUtility.FromJson<DialogueControl>(dJSON);
        }
        if (!string.IsNullOrEmpty(EquipScriptName))
        {
            string dJSON = Resources.Load<TextAsset>("Dialogues/Items/" + EquipScriptName).text;
            EquipDialogue = JsonUtility.FromJson<DialogueControl>(dJSON);
        }

        mControls = new InputControls();

        mControls.Player.Interact.performed += __ => HandlePlayerInteract();

    }

	private void OnEnable()
	{
        mControls.Enable();
	}

	private void OnDisable()
	{
        mControls.Disable();
	}

	private void Start()
    {
        //InteractUI = Instantiate(InteractPrefab, FindObjectOfType<Canvas>().transform);
        InteractUI = Instantiate(InteractPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);

        InteractUIYOffset = (gameObject.GetComponent<SpriteRenderer>().sprite.rect.size.y) +
                                (InteractUI.GetComponentInChildren<Image>().sprite.rect.y);
        InteractUIYOffset += InteractUIYOffset * 0.1f;

        InteractUI.SetActive(false);
    }

    public void HandlePlayerInteract()
	{
        if (playerTouching && ClickToPickup)
        {
            if (PickupDialogue != null && PickupDialogue.DialogueList != null
                && PickupDialogue.DialogueList.Count > 0)
            {
                if (DialogueManager.Instance.DialoguePauseWorld == false)
                {
                    DialogueManager.Instance.StartDialogue(PickupDialogue.GetCurrentDialogue());
                }
            }
            else
            {
                Debug.LogWarning("Item " + ItemName + " does not have a pickup dialoauge set!");
            }

            InventoryItem item = GameManager.Instance.currentGameState.CurrentInventory.Find(itm => itm.Name == ItemName);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                GameManager.Instance.currentGameState.CurrentInventory.Add(new InventoryItem()
                {
                    ID = ItemID,
                    Name = ItemName,
                    Quantity = 1,
                    IsKeyItem = true,
                    SpriteName = SpriteName,
                    UseScript = UseDialogue,
                    EquipScript = EquipDialogue,
                    InfoScript = InfoDialouge,
                });
            }
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        InteractUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        InteractUI.transform.position = InteractUI.transform.position + new Vector3(0.0f, InteractUIYOffset, 0.0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (ClickToPickup)
            {
                playerTouching = false;
                InteractUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.gameObject.CompareTag("Player"))
        {
            if( ClickToPickup)
            {
                playerTouching = true;
                //wait for click.          
                InteractUI.SetActive(true);
            }
            else
            {                
                //should probably check if Item already there, if so delete and add new with increased Quantity.
                GameManager.Instance.currentGameState.CurrentInventory.Add(new InventoryItem()
                {
                    ID = ItemID,
                    Name = ItemName,
                    Quantity = 1,
                    IsKeyItem = false
                });

                if (PickupDialogue != null && PickupDialogue.DialogueList != null
                && PickupDialogue.DialogueList.Count > 0)
                {
                    if (DialogueManager.Instance.DialoguePauseWorld == false)
                    {
                        DialogueManager.Instance.StartDialogue(PickupDialogue.GetCurrentDialogue());
                    }
                }
                else
                {
                    Debug.LogWarning("Item " + ItemName + " does not have a pickup dialoauge set!");
                }

                GameObject.Destroy(this.gameObject);
            }
        }
    }

}
