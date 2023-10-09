using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HippoAnimationState))]
public class HippoAnimEditor : Editor
{
	SerializedProperty AnimationName;
	SerializedProperty Interruptable;
	HippoAnimationState animState;
	static bool ShowNextStateList = true;

	private void OnEnable()
	{
		animState = (HippoAnimationState)target;
		//AnimationName = serializedObject.FindProperty("AnimationName");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		animState = (HippoAnimationState)target;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("AnimationName"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Interruptable"));

		//serializedObject.FindProperty("m_AnimationName").stringValue = EditorGUILayout.TextField("AnimName", serializedObject.FindProperty("m_AnimationName").stringValue);
		//serializedObject.FindProperty("Interruptable").boolValue = EditorGUILayout.Toggle("Interruptable", serializedObject.FindProperty("Interruptable").boolValue);
		//animState.AnimationName = EditorGUILayout.TextField("AnimName", animState.AnimationName);
		//animState.Interruptable = EditorGUILayout.Toggle("Interruptable", animState.Interruptable);

		ShowNextStateList = EditorGUILayout.Foldout(ShowNextStateList, "Next States"); 

		if (ShowNextStateList)
		{
			if (animState.NextStates != null)
			{
				EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.1f, 1.0f, 0.1f));
				EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.1f, 1.0f, 0.1f));
				foreach (HippoAnimationNextState state in animState.NextStates)
				{
					EditorGUILayout.LabelField("Possible Next State: " + state.NextAnimationName );
					state.NextAnimationName = EditorGUILayout.TextField("Animation Name", state.NextAnimationName);
					state.NextAnimationState = EditorGUILayout.ObjectField("Next Anim State", state.NextAnimationState, typeof(HippoAnimationState), false) as HippoAnimationState;
					state.InterruptCurrentAnim = EditorGUILayout.Toggle("Interrupt Current State", state.InterruptCurrentAnim);
					state.Priority = EditorGUILayout.IntField("Priority", state.Priority);
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Add Condition"))
					{
						if (state.Conditions == null)
						{
							state.Conditions = new List<HippoAnimationNextCondition>();
						}
						state.Conditions.Add(new HippoAnimationNextCondition());
					}
					
					GUILayout.EndHorizontal();
					
					if (state.Conditions != null)
					{
						EditorGUILayout.LabelField("Conditions");
						int ConditionNum = 1;
						List<HippoAnimationNextCondition> forDelete = new List<HippoAnimationNextCondition>();
						foreach (HippoAnimationNextCondition cond in state.Conditions)
						{
							
							EditorGUILayout.BeginVertical(EditorStyles.helpBox);
							EditorGUILayout.LabelField("Condition " + ConditionNum);

							string usedCondition = "none";
							if( cond.floatName != null && cond.floatName.Length > 0)
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

							if (usedCondition == "none" || usedCondition == "float")
							{
								EditorGUILayout.LabelField("Float ");
								GUILayout.BeginHorizontal();
								cond.floatName = EditorGUILayout.TextField("Name", cond.floatName);
								cond.floatValue = EditorGUILayout.FloatField("Value", cond.floatValue);
								GUILayout.EndHorizontal();
								EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.5f, 0.5f, 0.5f));
							}

							if (usedCondition == "none" || usedCondition == "int")
							{
								EditorGUILayout.LabelField("Int ");
								GUILayout.BeginHorizontal();
								cond.intName = EditorGUILayout.TextField("Name", cond.intName);
								cond.intValue = EditorGUILayout.IntField("Value", cond.intValue);
								GUILayout.EndHorizontal();
								EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.5f, 0.5f, 0.5f));
							}

							if (usedCondition == "none" || usedCondition == "bool")
							{
								EditorGUILayout.LabelField("Bool ");
								GUILayout.BeginHorizontal();
								cond.boolName = EditorGUILayout.TextField("Name", cond.boolName);
								cond.boolValue = EditorGUILayout.Toggle("Value", cond.boolValue);
								GUILayout.EndHorizontal();
								EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.5f, 0.5f, 0.5f));
							}

							if (usedCondition == "none" || usedCondition == "string")
							{
								EditorGUILayout.LabelField("String ");
								GUILayout.BeginHorizontal();
								cond.stringName = EditorGUILayout.TextField("Name", cond.stringValue);
								cond.stringValue = EditorGUILayout.TextField("Value", cond.stringValue);
								GUILayout.EndHorizontal();
								EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.5f, 0.5f, 0.5f));
							}

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

							if (usedCondition != "none" )
							{
								EditorGUILayout.HelpBox("Using " + usedCondition + " type condition", MessageType.Info);
								EditorGUILayout.HelpBox("To change the condition type, clear the current condition.", MessageType.Warning);
							}

							if (usedCondition != "string" && usedCondition != "bool")
							{
								cond.comparison = EditorGUILayout.TextField("Comaparison", cond.comparison);
							}
							
							GUILayout.Space(8.0f);
							ConditionNum++;
							if (GUILayout.Button("Delete Condition"))
							{
								forDelete.Add(cond);
							}
							EditorGUILayout.EndVertical();
						}
						if (state.Conditions != null && state.Conditions.Count > 0 && forDelete.Count >0)
						{
							state.Conditions.Remove(forDelete[0]);
						}
					}
					EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.1f, 1.0f, 0.1f));
					EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.1f, 1.0f, 0.1f));
					GUILayout.Space(8.0f);
				}
			}
		}
		
		if(GUILayout.Button("Add Next State"))
		{
			if( animState.NextStates == null)
			{
				animState.NextStates = new List<HippoAnimationNextState>();
			}
			animState.NextStates.Add(new HippoAnimationNextState());
		}
		if (GUI.changed)
		{
			//Undo.RecordObject(animState);
			EditorUtility.SetDirty(animState);
			//UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
