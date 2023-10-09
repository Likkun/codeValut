using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{    
    public GameObject InteractPrefab;
    public GameObject InteractUI;

    public InputControls mControls;

    public float InteractUIYOffset = 0.0f;

    public TextAsset DialogueJSON;
    public DialogueControl DialogueControl;

    public bool ClickToInteract = true;

    public bool dialogueReady = false;

    private void Awake()
    {
        mControls = new InputControls();
        mControls.Player.Interact.performed += ctx => HandlePlayerInteract();

        if ( DialogueJSON != null)
        {
            DialogueControl = JsonUtility.FromJson<DialogueControl>(DialogueJSON.text);
        }
        
    }


    private void OnEnable()
    {
        DialogueManager.OnSetDialogueState += HandleDialogueState;
        mControls.Enable();
    }

    private void OnDisable()
    {
        DialogueManager.OnSetDialogueState -= HandleDialogueState;
        mControls.Disable();
    }

    private void HandleDialogueState( bool active, bool worldPause )
	{
        if( active == true)
		{
            mControls.Disable();
		}
		else
		{
            mControls.Enable();
		}
	}

    private void Start()
    {
        if (ClickToInteract)
        {
            //InteractUI = Instantiate(InteractPrefab, FindObjectOfType<Canvas>().transform);
            InteractUI = Instantiate(InteractPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);

            if (InteractUIYOffset == 0)
            {
                InteractUIYOffset = (gameObject.GetComponent<SpriteRenderer>().sprite.rect.size.y) +
                                        (InteractUI.GetComponentInChildren<Image>().sprite.rect.y);
                InteractUIYOffset += InteractUIYOffset * 0.1f;
            }

            InteractUI.SetActive(false);
        }

    }

	public void HandlePlayerInteract()
	{
        if (dialogueReady)
		{            
			if (DialogueManager.Instance.DialoguePauseWorld == false)
			{
				DialogueManager.Instance.StartDialogue(DialogueControl.GetCurrentDialogue());
			}
		}
	}

    private void Update()
    {
        //get sprite height for
        if (ClickToInteract)
        {
            InteractUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            InteractUI.transform.position = InteractUI.transform.position + new Vector3(0.0f, InteractUIYOffset, 0.0f);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Player"))
		{
            if (ClickToInteract)
            {
                dialogueReady = true;
                InteractUI.SetActive(true);
            }
			else
			{
                //Trigger dialogue
                dialogueReady = true;
                if (dialogueReady)
                {
                    if (DialogueManager.Instance.DialoguePauseWorld == false)
                    {
                        DialogueManager.Instance.StartDialogue(DialogueControl.GetCurrentDialogue());
                    }
                }
            }
		}
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ClickToInteract)
            {
                dialogueReady = false;
                InteractUI.SetActive(false);
            }
			else
			{
                dialogueReady = false;
			}
        }
    }
}
