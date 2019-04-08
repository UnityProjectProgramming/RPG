using UnityEngine;
using DuloGames.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DuloGamesEditor.UI
{
	[CanEditMultipleObjects, CustomEditor(typeof(UIButtonExtended), true)]
	public class UIButtonExtendedEditor : ButtonEditor {
	
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			serializedObject.Update();
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onStateChange"), new GUIContent("On State Change"), true);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
