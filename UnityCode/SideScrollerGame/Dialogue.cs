using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueControl
{
    public List<DialogueContainer> DialogueList;

    //this function should parse throught the list
    //from top to bottom
    //and return the first valid Dialogue object
    public List<Dialogue> GetCurrentDialogue()
    {
        foreach (DialogueContainer container in DialogueList)
        {
            bool isValid = true;
            foreach (DialogueCondition condition in container.Conditionals)
            {
                if (condition.StoryPoints != null && condition.StoryPoints.Length > 0 && 
                    !GameManager.Instance.currentGameState.CompletedStoryPoints.Contains(condition.StoryPoints[0] ))
                {
                    isValid = false;
                }
            }
            if (isValid)
            {
                return container.Dialogue;
            }
        }
        //default to last dialogue
        return DialogueList[DialogueList.Count-1].Dialogue;
    }
}

[Serializable]
public class DialogueContainer
{
    public List<DialogueCondition> Conditionals;
    public List<Dialogue> Dialogue;
}

[Serializable]
public class DialogueCondition
{
    public List<DialogItemRef> Items;
    public string Health;
    public string[] StoryPoints;
}

[Serializable]
public class DialogItemRef
{
    public string Condition;
    public int ID;
    public int Quantity;

}

[Serializable]
public class Dialogue
{
    public string SpeakerName;
    public string Body;
    public bool PauseWorld = true;
    public DialoguePortrait Portrait;
    public List<DialoguePostAction> PickupActions;
    public List<DialoguePostAction> PostActions;
}

[Serializable]
public class DialoguePortrait
{
    public string Filename;
    public string Side;
    public bool Shake;
}

[Serializable]
public class DialoguePostAction
{
    public string Action;
    public int TargetID;
    public int Quantity;
    public string[] StoryPoints;
}

