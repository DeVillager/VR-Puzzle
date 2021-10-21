using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShadowChecker))]
public class ShadowCheckerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShadowChecker func = (ShadowChecker)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Clicked"))
        {
            func.InitRaycastPositions();
        }
    }
}
