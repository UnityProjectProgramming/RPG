using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DuloGames.UI;

namespace DuloGamesEditor.UI
{
	[CanEditMultipleObjects, CustomEditor(typeof(UIButtonExtended_Target))]
	public class UIButtonExtended_TargetEditor : Editor {
		
		private SerializedProperty m_TransitionProperty;
		private SerializedProperty m_TargetGraphicProperty;
		private SerializedProperty m_TargetGameObjectProperty;
		private SerializedProperty m_ColorsProperty;
		private SerializedProperty m_SpriteStateProperty;
		private SerializedProperty m_AnimationTriggersProperty;
		
		protected void OnEnable()
		{
			this.m_TransitionProperty = this.serializedObject.FindProperty("m_Transition");
			this.m_TargetGraphicProperty = this.serializedObject.FindProperty("m_TargetGraphic");
			this.m_TargetGameObjectProperty = this.serializedObject.FindProperty("m_TargetGameObject");
			this.m_ColorsProperty = this.serializedObject.FindProperty("m_Colors");
			this.m_SpriteStateProperty = this.serializedObject.FindProperty("m_SpriteState");
			this.m_AnimationTriggersProperty = this.serializedObject.FindProperty("m_AnimationTriggers");
		}
		
		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			
			Selectable.Transition transition = (Selectable.Transition)this.m_TransitionProperty.enumValueIndex;
			Graphic graphic = this.m_TargetGraphicProperty.objectReferenceValue as Graphic;
			GameObject targetGameObject = this.m_TargetGameObjectProperty.objectReferenceValue as GameObject;
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.m_TransitionProperty, new GUIContent("Transition"));
			EditorGUI.indentLevel++;
			
			// Check if the transition requires a graphic
			if (transition == Selectable.Transition.ColorTint || transition == Selectable.Transition.SpriteSwap)
			{
				EditorGUILayout.PropertyField(this.m_TargetGraphicProperty, new GUIContent("Target Graphic"));
				
				if (transition == Selectable.Transition.ColorTint)
				{
					if (graphic == null)
					{
						EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Info);
					}
					else
					{
						EditorGUILayout.PropertyField(this.m_ColorsProperty, true);
					}
				}
				else if (transition == Selectable.Transition.SpriteSwap)
				{
					if (graphic as Image == null)
					{
						EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", MessageType.Info);
					}
					else
					{
						EditorGUILayout.PropertyField(this.m_SpriteStateProperty, true);
					}
				}
			}
			else if (transition == Selectable.Transition.Animation)
			{
				EditorGUILayout.PropertyField(this.m_TargetGameObjectProperty, new GUIContent("Target GameObject"));
				
				if (targetGameObject == null)
				{
					EditorGUILayout.HelpBox("You must have a Game Object target in order to use a animation transition.", MessageType.Info);
				}
				else
				{
					EditorGUILayout.PropertyField(this.m_AnimationTriggersProperty, true);
					
					Animator animator = (target as UIButtonExtended_Target).animator;
					
					if (animator == null || animator.runtimeAnimatorController == null)
					{
						Rect controlRect = EditorGUILayout.GetControlRect();
						controlRect.xMin = (controlRect.xMin + EditorGUIUtility.labelWidth);
						
						if (GUI.Button(controlRect, "Auto Generate Animation", EditorStyles.miniButton))
						{
							// Generate the animator controller
							UnityEditor.Animations.AnimatorController animatorController = UIAnimatorControllerGenerator.GenerateAnimatorContoller(this.m_AnimationTriggersProperty, this.m_TargetGameObjectProperty.objectReferenceValue.name);
							
							if (animatorController != null)
							{
								if (animator == null)
								{
									animator = (target as UIButtonExtended_Target).targetGameObject.AddComponent<Animator>();
								}
								UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, animatorController);
							}
						}
					}
				}
			}
			
			this.serializedObject.ApplyModifiedProperties();
		}
	}
}
