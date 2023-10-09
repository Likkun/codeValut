using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(HippoAnimationController))]
public class HippoAnimationControllerEditor : Editor
{
    HippoAnimationController _controller;
	Animator Animator;
    Dictionary<string, bool> showAnimState;

	//Toggles for anim states to show thier "next" lists
	Dictionary<string, bool> AnimShowNextList = new Dictionary<string, bool>();

	private void OnEnable()
	{
		_controller = (HippoAnimationController)target;

		MonoBehaviour monoBev = (MonoBehaviour)target;
		Animator = monoBev.GetComponent<Animator>();
		//look for folder with HippoAnimationStates...

		showAnimState = new Dictionary<string, bool>();
		foreach (AnimationClip a in Animator.runtimeAnimatorController.animationClips)
		{
			showAnimState.Add(a.name, false);
			AnimShowNextList.Add(a.name, false);
			UnityEditor.AnimationUtility.SetAnimationEvents(a, new AnimationEvent[]
			{
				new AnimationEvent(){
					functionName = "HippoAnimationEndHandle",
					time = a.length - (1/a.frameRate),
				}
			});
			EditorUtility.SetDirty(a);
		}

		
		//load intial states for foldouts here
	}
	public override void OnInspectorGUI()
	{

		serializedObject.Update();

		_controller.StartingAnimation = EditorGUILayout.ObjectField("Starting Animation State",
													_controller.StartingAnimation, 
													typeof(HippoAnimationState), 
													false) as HippoAnimationState;

		EditorGUILayout.LabelField(" Current Animation: " 
									+ (_controller.CurrentAnimationState ? 
									_controller.CurrentAnimationState.AnimationName 
									: "NONE"));

		EditorGUILayout.LabelField("Variable Fields");

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("floatVars"));
		foreach( KeyValuePair<string,float> kvp in _controller.floatVars)
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.TextField("Name", kvp.Key);
			EditorGUILayout.FloatField("Value", kvp.Value);
			GUILayout.EndHorizontal();
		}

		if( Animator == null)
		{
			EditorGUILayout.HelpBox("An Animator and Animator Controller is required for this script to work.", MessageType.Error);
		}

		if (!AssetDatabase.IsValidFolder("Assets/HippoAnimationStates/" + target.name + "_HippoAnimationStates"))
		{


			if (GUILayout.Button("Generate HippoAnimationStates"))
			{

				if (!AssetDatabase.IsValidFolder("Assets/HippoAnimationStates"))
				{
					AssetDatabase.CreateFolder("Assets", "HippoAnimationStates");
				}
				if (!AssetDatabase.IsValidFolder("Assets/HippoAnimationStates/" + target.name + "_HippoAnimationStates"))
				{
					AssetDatabase.CreateFolder("Assets/HippoAnimationStates", target.name + "_HippoAnimationStates");
				}

				//reset ShowAnimState
				showAnimState = new Dictionary<string, bool>();
				AnimShowNextList = new Dictionary<string, bool>();
				foreach (AnimationClip a in Animator.runtimeAnimatorController.animationClips)
				{
					AssetDatabase.CreateAsset(new HippoAnimationState(),
						"Assets/HippoAnimationStates/" + target.name + "_HippoAnimationStates/" + a.name + ".asset");

					showAnimState.Add(a.name, false);
					AnimShowNextList.Add(a.name, false);
				}

			}
		}
		else
		{

			if(GUILayout.Button("Update Animations"))
			{
				Debug.LogWarning("Update Animations not yet implimented.");
			}



			//TODO: Add interface for editing AnimStates



		}
		if (GUI.changed)
		{
			//Undo.RecordObject(animState);
			EditorUtility.SetDirty(_controller);
			//UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
