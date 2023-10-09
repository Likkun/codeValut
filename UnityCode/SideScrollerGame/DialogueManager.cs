using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance; 
    public static DialogueManager Instance { get { return _instance; } }

    public delegate void SetDialogueState( bool actice, bool worldPause );
    public static event SetDialogueState OnSetDialogueState;

    InputControls mInputControls;

    public Text NameText;
    public Text DialogueText;
    public Animator AnimControl;
    public Image Left_Portrait;
    public Image Right_Portrait;

    List<Sprite> Portraits;

    public float TypingSpeed = 0.02f;

    //private Queue<string> sentences;
    private Queue<Dialogue> DialogueFrames;

    private IEnumerator TypingHandle;
    private IEnumerator AutoContinueHandle;

    public Dialogue CurrentSentence;

    private bool readyForNextSentence = false;

    public bool DialogueInProgress = false;
    public bool DialoguePauseWorld = true;

    public bool WaitingAutoContinue = false;

    public PlayerController _Player;
    private void Awake()
    {
        mInputControls = new InputControls();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Portraits = new List<Sprite>();

        Sprite[] temp = Resources.LoadAll<Sprite>("Portraits");

        foreach(Sprite s in temp)
        {
            Portraits.Add(s);            
        }

        
        mInputControls.Player.Interact.performed += ctx => HandlePlayerInteract();

        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    private void HandlePickupActions(Dialogue d)
	{
        if( d != null && d.PickupActions.Count > 0)
		{
            HandleDalogueAction(d.PickupActions);
        }        
    }

	private void HandlePlayerInteract()
	{
        FinishedSentencePostActions();
        WaitingAutoContinue = false;
        if (readyForNextSentence)
        {
            DisplayNextSenetence();
            readyForNextSentence = false;
        }
        else
        {
            if (TypingHandle != null)
            {
                StopCoroutine(TypingHandle);
            }
            if (CurrentSentence != null)
            {
                DialogueText.text = CurrentSentence.Body;
            }
            readyForNextSentence = true;
        }
    }

	public void SendSetDialogue(bool active, bool worldPause)
    {
        OnSetDialogueState(active, worldPause);
    }

    private void OnEnable()
    {
        mInputControls.Enable();
        DialogeUIController.OnDialogueOpenComplete += HandleDialogueOpenComplete;
    }

    private void OnDisable()
    {
        mInputControls.Disable();
        DialogeUIController.OnDialogueOpenComplete -= HandleDialogueOpenComplete;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sentences = new Queue<string>();
        DialogueFrames = new Queue<Dialogue>();
    }

    public void Update()
    {
        
    }

    public void StartDialogue(List<Dialogue> dialogue)
    {
        if( dialogue == null || dialogue.Count <= 0)
		{
            return;
		}

        if (AnimControl != null)
        {
            AnimControl.SetBool("Close", false);
            AnimControl.SetBool("Open", true);
        }
        DialogueInProgress = true;

        Debug.Log("HELP: " + dialogue[0].PauseWorld);

        DialoguePauseWorld = dialogue[0].PauseWorld;

        HandlePickupActions(dialogue[0]);

        SendSetDialogue(true, DialoguePauseWorld);
        //Debug.Log("Starting Conversation With " + dialogue.Name);
        //NameText.text = dialogue.Name;

        DialogueFrames.Clear();

        foreach(Dialogue d in dialogue)
        {            
            DialogueFrames.Enqueue(d);
        }

        //foreach(string sentence in dialogue.sentences)
        //{
        //    sentences.Enqueue(sentence);
        //}

        //DisplayNextSenetence();

    }

    public void HandleDialogueOpenComplete()
    {
        DisplayNextSenetence();
    }

    public void FinishedSentencePostActions()
    {
        if( CurrentSentence == null || CurrentSentence.PostActions == null)
        {
            return;
        }
        if(CurrentSentence.PostActions.Count > 0)
        {
            List<DialoguePostAction> postActions = CurrentSentence.PostActions;
            HandleDalogueAction(postActions);
        }
    }

	private void HandleDalogueAction(List<DialoguePostAction> postActions)
	{
        Debug.Log("HandleDialogueAction");
        foreach (DialoguePostAction pa in postActions)
        {
            switch (pa.Action.ToUpper())
            {
                case "STORYPOINT":
                case "STORY_POINT":
                case "STORY POINT":
                    foreach (string sp in pa.StoryPoints)
                    {
                        try
                        {
                            GameManager.Instance.currentGameState.CompletedStoryPoints.Add(sp);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                    break;
                case "HEALTH":
                    GameManager.Instance.currentGameState.PlayerState.CurrentHealth = Math.Min(GameManager.Instance.currentGameState.PlayerState.CurrentHealth + 5, GameManager.Instance.currentGameState.PlayerState.MaxHealth);
                    break;
                case "BULLETS":
                case "AMMO":
                    
                    if (_Player != null)
					{
                        Debug.Log("Add ammo: " + pa.Quantity);
                        _Player.StoredAmmo += pa.Quantity;
					}
					else
					{
                        Debug.Log("DialogueManager::_Player is null");
					}
                    break;
            }
        }
    }

    public void DisplayNextSenetence()
    {
        if(DialogueFrames.Count == 0)
        {
            EndDialogue();
            return;
        }
        CurrentSentence = DialogueFrames.Dequeue();

        NameText.text = CurrentSentence.SpeakerName;
        NameText.alignment = (CurrentSentence.Portrait != null && 
                              CurrentSentence.Portrait.Side.ToUpper().Trim() == "RIGHT") ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;

        

        if (Left_Portrait && Right_Portrait != null)
        {
            Left_Portrait.gameObject.SetActive(false);
            Right_Portrait.gameObject.SetActive(false);

            Left_Portrait.sprite = null;
            Right_Portrait.sprite = null;
            Left_Portrait.enabled = false;
            Right_Portrait.enabled = false;
            //reset portaits                        
            if (CurrentSentence.Portrait != null &&
                CurrentSentence.Portrait.Filename != null)
            {
                if (CurrentSentence.Portrait.Side.ToUpper().Trim() == "RIGHT")
                {
                    Right_Portrait.gameObject.SetActive(true);
                    Right_Portrait.enabled = true;
                    Right_Portrait.sprite = Portraits.Find(s => s.name == CurrentSentence.Portrait.Filename);
                }
                else if(CurrentSentence.Portrait.Side.ToUpper().Trim() == "LEFT")
                {
                    Left_Portrait.gameObject.SetActive(true);
                    Left_Portrait.enabled = true;
                    Left_Portrait.sprite = Portraits.Find(s => s.name == CurrentSentence.Portrait.Filename);
                }
            }
        }
        else
        {
            Debug.LogWarning("Portrait containers not set in dialogue");
        }
        string sentence = CurrentSentence.Body;
        //Debug.Log(sentence);
        if (TypingHandle != null)
        {
            StopCoroutine(TypingHandle);
        }
        TypingHandle = TypeSentence(sentence);
        StartCoroutine(TypingHandle);
        
        //DialogueText.text = sentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        readyForNextSentence = true;
        if (CurrentSentence.PauseWorld == false) 
        {
            //Auto continue since player might still be playing.
            WaitingAutoContinue = true;
            AutoContinueHandle = AutoContinue(1.5f);
            StartCoroutine(AutoContinueHandle);
        }
    }

    IEnumerator AutoContinue(float time)
	{
        if(!WaitingAutoContinue)
		{
            yield return 0;
		}

        yield return new WaitForSeconds(time);
        if (WaitingAutoContinue)
        {
            HandlePlayerInteract();
        }
    }

    void EndDialogue()
    {
        SendSetDialogue(false, false);

        if (Left_Portrait && Right_Portrait != null)
        {
            Left_Portrait.sprite = null;
            Right_Portrait.sprite = null;
            Left_Portrait.enabled = false;
            Right_Portrait.enabled = false;
            Left_Portrait.gameObject.SetActive(false);
            Right_Portrait.gameObject.SetActive(false);
        }

        AnimControl.SetBool("Close", true);
        AnimControl.SetBool("Open", false);
        //FinishedSentencePostActions();
        CurrentSentence = null;

        DialogueInProgress = false;
        //reset Pause World control
        DialoguePauseWorld = false;
    }

}
