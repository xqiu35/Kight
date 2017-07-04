using UnityEditor;
using UnityEngine;

namespace Merlin.FastHighlight
{
	[CustomEditor(typeof(Highlighter))]
	public class HighlighterEditor : Editor 
	{
		private const float MinHighlightThickness = 0f;
		private const float MaxHighlightThickness = 0.5f;

		public override void OnInspectorGUI()
	    {
	        Highlighter highlighter = (Highlighter)target;

            GUI.changed = false;
            Color newColor = EditorGUILayout.ColorField(new GUIContent("Color", "Control the color of the highlight"), highlighter.HighlightColor);
		    if (newColor != highlighter.HighlightColor)
		    {
                Undo.RecordObject(highlighter, "Change Highlight Color");
		        highlighter.HighlightColor = newColor;
		    }

		    float newThickness = EditorGUILayout.Slider(new GUIContent("Thickness", "Control the thickness of the highlight"), highlighter.HighlightThickness, MinHighlightThickness, MaxHighlightThickness);
		    if (newThickness != highlighter.HighlightThickness)
		    {
                Undo.RecordObject(highlighter, "Change Highlight Thickness");
                highlighter.HighlightThickness = newThickness;
		    }

	        bool newIsFluidGeometry = EditorGUILayout.Toggle(new GUIContent("Is Fluid Geometry", "Toggle this if your model has a lot of fluid curvatures to produce a smoother highlight"), highlighter.IsFluidGeometry);
	        if (newIsFluidGeometry != highlighter.IsFluidGeometry)
	        {
                Undo.RecordObject(highlighter, "Change Highlight Fluid Geometry");
                highlighter.IsFluidGeometry = newIsFluidGeometry;
	        }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(highlighter);
            }
	    }
	}
}