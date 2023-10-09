using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HippoAnimationController : MonoBehaviour
{
    Animator mAnimator;

    public HippoAnimationState StartingAnimation;
    public List<HippoAnimationState> AnimationStates;
    public HippoAnimationState CurrentAnimationState;
    public HippoAnimationState NextAnimationState;

    [SerializeField]
    private string Current_Animation = "Idle";

    //ANIMATION STATES
    const string IDLE_ANIMATION = "Idle";
    const string WALK_ANIMATION = "Walk";
    const string RUN_ANIMATION = "Run";
    const string AIM_ANIMATION = "Aim";
    const string AIM_UP_ANIMATION = "Aim_Up";
    const string AIM_DOWN_ANIMATION = "Aim_Down";
    const string AIM_WALK_ANIMATION = "Walk_Aim";
    const string JUMP_ANIMATION = "Jump";
    const string JUMP_CLIMAX_ANIMATION = "Jump_Climax";
    const string FALL_ANIMATION = "Walk";
    const string ONGROUND_ANIMATION = "Walk";
    const string GET_UP_ANIMATION = "Get_Up";
    const string CROUCH_ANIMATION = "Crouch";
    const string CROUCH_AIM_ANIMATION = "Crouch_Aim";
    const string CROUCH_AIM_UP_ANIMATION = "Crouch_Aim_Up";
    const string CROUCH_AIM_DOWN_ANIMATION = "Crouch_Aim_Down";
    const string CROUCH_WALK = "Crouch_Walk";

    //TO DO ANIMATIONS
    const string LAND_ANIMATION = "TODO";
    const string SUMMON_ANIMATION = "TODO";
    const string RETURN_ANIMATION = "TODO";
    const string HURT_ANIMATION = "TODO";
    const string DIE_ANIMATION = "TODO";
    const string RELOAD_ANIMATION = "TODO";
    const string RELOAD_WALK_ANIMATION = "TODO";
    const string JUMP_SHOOT = "TODO";

    private float Current_Animation_Length = 0f;


    [field: SerializeField]
    public Dictionary<string, float> floatVars = new Dictionary<string, float>();

    [field: SerializeField]
    public Dictionary<string, int> intVars = new Dictionary<string, int>();

    [field: SerializeField]
    public Dictionary<string, string> stringVars = new Dictionary<string, string>();

    [field: SerializeField]
    public Dictionary<string, bool> booleanVars = new Dictionary<string, bool>();

    

    private void Awake()
	{        
        mAnimator = GetComponent<Animator>();
    }

	private void OnEnable()
	{
        
	}

	private void OnDisable()
	{
        
	}

    public void HippoAnimationEndHandle()
    {
        //Here is where we need to decide next animation if required....

        //should check if !looping....
        if (!mAnimator.GetCurrentAnimatorStateInfo(0).loop)
        {
            CheckForNextAnimation();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();

        if(StartingAnimation != null)
		{
            SetAnimationState(StartingAnimation);
            Debug.Log("HippoAnimationController Starting on animation: " + StartingAnimation.AnimationName);
            //mAnimator.Play(StartingAnimation.name); ;
        }
		else
		{
            ///??????????????
            mAnimator.Play("Idle");
        }

        //TEST
        //floatVars.Add("XVel", 0f);
    
        //Invoke("GetUp", 2.5f);
    }

    public void HandleNextAnimState()
	{
        //get current animation time
        //How to cancel this?
        float time = mAnimator.GetCurrentAnimatorStateInfo(0).length;
        //Invoke("SetIdle", time);

        //loop through currentAnimation

    }

    // Update is called once per frame
    void Update()
    {
        //CheckForNextAnimation();
    }

    public void CheckForNextAnimation()
    {
        if (CurrentAnimationState != null)
		{
            if( CurrentAnimationState.NextStates != null)
			{
                List<HippoAnimationNextState> validStates = new List<HippoAnimationNextState>();
                foreach (HippoAnimationNextState nextState in CurrentAnimationState.NextStates)
				{
                    if( nextState.Conditions == null || nextState.Conditions.Count == 0)
					{
                        validStates.Add(nextState);
					}
                    //create a list of valid Next States
                    List<bool> validConditions = new List<bool>();
                    foreach( HippoAnimationNextCondition cond in nextState.Conditions)
					{
                        //Debug.Log("--------------");
                        //Debug.Log("Checking " + nextState.Conditions.Count + " for: " + nextState.NextAnimationName);
                        string usedCondition = "none";
                        if (cond.floatName != null && cond.floatName.Length > 0)
                        {
                            usedCondition = "float";
                        }
                        if (cond.intName != null && cond.intName.Length > 0)
                        {
                            usedCondition = "int";
                        }
                        if (cond.boolName != null && cond.boolName.Length > 0)
                        {
                            usedCondition = "bool";
                        }
                        if (cond.stringName != null && cond.stringName.Length > 0)
                        {
                            usedCondition = "string";
                        }

						switch (usedCondition)
						{
                            case "float":
                                if (floatVars.ContainsKey(cond.floatName))
                                {
                                    if (cond.comparison == "==" || cond.comparison == "=")
                                    {
                                        if (floatVars[cond.floatName] == cond.floatValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
										else
										{
                                            validConditions.Add(false);
										}
                                    }
                                    else if (cond.comparison == ">=")
                                    {
                                        if (floatVars[cond.floatName] >= cond.floatValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == "<=")
                                    {
                                        if (floatVars[cond.floatName] <= cond.floatValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == ">")
                                    {
                                        if (floatVars[cond.floatName] > cond.floatValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                             validConditions.Add(false);
                                        }   
                                    }
                                    else if (cond.comparison == "<")
                                    {
                                        if (floatVars[cond.floatName] < cond.floatValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Float Comparision operator: [ " + cond.comparison + " ] was not valid for condition on: " + nextState.NextAnimationName);
                                    }
                                }
								else {
                                    Debug.LogError("[HippoAnimationController::CheckForNextAnimation()] - floatVar: "+ cond.floatName +" was not defined");
                                }
                                break;
                            case "int":
                                if (intVars.ContainsKey(cond.intName))
                                {
                                    if (cond.comparison == "==" || cond.comparison == "=")
                                    {
                                        if (intVars[cond.intName] == cond.intValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == ">=")
                                    {
                                        if (intVars[cond.intName] >= cond.intValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == "<=")
                                    {
                                        if (intVars[cond.intName] <= cond.intValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == ">")
                                    {
                                        if (intVars[cond.intName] > cond.intValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else if (cond.comparison == "<")
                                    {
                                        if (intVars[cond.intName] < cond.intValue)
                                        {
                                            //validStates.Add(nextState);
                                            validConditions.Add(true);
                                        }
                                        else
                                        {
                                            validConditions.Add(false);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Int Comparision operator: [ " + cond.comparison + " ] was not valid for condition on: " + nextState.NextAnimationName);
                                    }
                                }
                                else
                                {
                                    Debug.LogError("[HippoAnimationController::CheckForNextAnimation()] - intVar: " + cond.intName + " was not defined");
                                }
                                break;
                            case "bool":
                                if( booleanVars.ContainsKey(cond.boolName) )
								{
                                    if (booleanVars[cond.boolName] == cond.boolValue)
									{
                                        //Debug.Log(cond.boolName + " condition was met");
                                        //validStates.Add(nextState);
                                        validConditions.Add(true);
                                    }
									else 
                                    {
                                        //Debug.Log(cond.boolName + " condition was not met");
                                        validConditions.Add(false);
                                    }
								}
								else
								{
                                    Debug.LogError("HippoAnimator boolean condition does not exist: " + cond.boolName);
								}
                                break;
                            case "string":
                                if (stringVars.ContainsKey(cond.stringName))
                                {
                                    if (stringVars[cond.stringName] == cond.stringValue) 
                                    {
                                        //validStates.Add(nextState);
                                        validConditions.Add(true);
                                    }
                                    else
									{
                                        validConditions.Add(false);
									}
                                }
                                else
                                {
                                    Debug.LogError("HippoAnimator string condition does not exist: " + cond.boolName);
                                }
                                break;
                            default:
                                validStates.Add(nextState);
                            break;
						}
                    }
					//loop through condition results
					if (!validConditions.Contains(false))
					{
                        //Debug.Log("----------------------");
                        //Debug.Log(validConditions.Count + " were found as true for state " + nextState.NextAnimationName);
                        //Debug.Log("----------------------");
                        //if there were no falses
                        //add this state.
                        validStates.Add(nextState);
					}
					else
					{
                        //Debug.Log(nextState.NextAnimationName + " was not added as valid");
					}
                   
				}
                //order list  by priority 
                validStates = (from nx in validStates
                               orderby nx.Priority descending
                               select nx).ToList<HippoAnimationNextState>();
                if (validStates.Count > 0)
                {
                    SetAnimationState(validStates[0].NextAnimationState);
                }
                else
                {
                    //Debug.Log("No Valid transitions found");
                }
            }
		}
		else
		{
            Debug.LogError("[HippoAnimationController::CheckForNextAnimation()] - CurrentAnimationState was null!");
		}
	}
    
    public void SetAnimationState( HippoAnimationState newState)
	{
        if(CurrentAnimationState != null &&
            CurrentAnimationState.AnimationName == newState.AnimationName)
		{
            //trying to set the same state            
            return;
		}
        //Debug.Log("Set animation: " + newState.AnimationName);
        mAnimator.Play(newState.AnimationName);
        CurrentAnimationState = newState;
        Current_Animation = newState.name;

        //Cancel Anything queued to handle the next anim state
        //since we'll be changing it.
        //CancelInvoke("HandleNextAnimState");

        //if (!mAnimator.GetCurrentAnimatorStateInfo(0).loop)
        //{
        //    //if the current playing animtion (not the one just set, that will take effect 
        //    //next frame) does not loop
        //    Current_Animation_Length = mAnimator.GetCurrentAnimatorStateInfo(0).length;
        //    Invoke("HandleNextAnimState", Current_Animation_Length);
        //}

    }

    public void setBool(string name, bool value)
	{
        if (booleanVars.ContainsKey(name))
        {
            booleanVars[name] = value;
            CheckForNextAnimation();
        }
        else
        {
            Debug.LogWarning("HippoAnimationController: Bool Var: " + name + " was not found in the list!");
        }
    }
    public void setFloat(string name, float value) { 
        if(floatVars.ContainsKey(name))
		{
            floatVars[name] = value;
            CheckForNextAnimation();
        }
		else
		{
            Debug.LogWarning("HippoAnimationController: Float Var: " + name + " was not found in the list!");
		}
    }

    public void SetAnimationState(string newState)
    {
        if (Current_Animation == newState)
        {
            //Trying to set same animation state
            return;
        }
        
        mAnimator.Play(newState);
        Current_Animation = newState;

        CancelInvoke("HandleNextAnimState");

        //may not need this?
        if (!mAnimator.GetCurrentAnimatorStateInfo(0).loop)
        {
            Current_Animation_Length = mAnimator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("HandleNextAnimState", Current_Animation_Length);
        }
    }

}
