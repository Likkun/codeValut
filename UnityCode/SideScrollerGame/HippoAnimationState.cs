using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Hippo", menuName = "Hippo/AnimationState")]
public class HippoAnimationState : ScriptableObject
{

	//public string AnimationName { get { return m_AnimationName; } set { m_AnimationName = value; } }
	[field: SerializeField]
	public string AnimationName;
	//private string m_AnimationName = "";
	[field: SerializeField]
	public bool Interruptable;
	[field:SerializeField]
	public List<HippoAnimationNextState> NextStates;
}

[Serializable]
public class HippoAnimationNextState
{
	[field: SerializeField]
	public string NextAnimationName;
	[field: SerializeField]
	public HippoAnimationState NextAnimationState;
	[field: SerializeField]
	public int Priority;
	[field: SerializeField]
	public List<HippoAnimationNextCondition> Conditions;
	[field: SerializeField]
	public float Delay;
	[field: SerializeField]
	public bool InterruptCurrentAnim;
}

[Serializable]
public class HippoAnimationNextCondition
{
	[field: SerializeField]
	public string floatName;
	[field: SerializeField]
	public string intName;
	[field: SerializeField]
	public string boolName;
	[field: SerializeField]
	public float floatValue;
	[field: SerializeField]
	public int intValue;

	[field: SerializeField] 
	public bool boolValue;
	[field: SerializeField] 
	public string stringName;
	[field: SerializeField] 
	public string stringValue;

	[field: SerializeField] 
	public string comparison;
}
