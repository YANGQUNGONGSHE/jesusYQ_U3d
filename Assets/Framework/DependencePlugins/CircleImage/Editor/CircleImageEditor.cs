using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(CircleImage), true)]
public class CircleImageEditor : Editor
{
    CircleImage _target;
    private SerializedProperty sprite;
    private SerializedProperty color;
    private SerializedProperty material;
    private SerializedProperty raycastTarget;
    private SerializedProperty fillPercent;
    private SerializedProperty fill;
    private SerializedProperty thickness;
    private SerializedProperty segements;
    void OnEnable()
    {
        _target = (CircleImage)target;
        sprite = serializedObject.FindProperty("m_Sprite");
        color = serializedObject.FindProperty("m_Color");
        material = serializedObject.FindProperty("m_Material");
        raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        fillPercent = serializedObject.FindProperty("fillPercent");
        fill = serializedObject.FindProperty("fill");
        thickness = serializedObject.FindProperty("thickness");
        segements = serializedObject.FindProperty("segements");
    }
    [MenuItem("GameObject/UI/CricleImage")]
    static void CricleImage()
    {
        GameObject parent = Selection.activeGameObject;
        RectTransform parentCanvasRenderer = (parent != null) ? parent.GetComponent<RectTransform>() : null;
        if (parentCanvasRenderer)
        {
            GameObject go = new GameObject("CricleImage");
            go.transform.SetParent(parent.transform, false);
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<CircleImage>();
            Selection.activeGameObject = go;
        }
        else
        {
            EditorUtility.DisplayDialog("CricleImage", "You must make the CricleImage object as a child of a Canvas.", "Ok");
        }
    }
    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.BeginHorizontal();
        //_target = (CircleImage)EditorGUILayout.ObjectField("Script", _target, typeof(CircleImage), true);
        //EditorGUILayout.EndHorizontal();
        serializedObject.Update();
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.PropertyField(material);
        EditorGUILayout.PropertyField(raycastTarget);
        EditorGUILayout.PropertyField(fillPercent);
        EditorGUILayout.PropertyField(fill);
        EditorGUILayout.PropertyField(thickness);
        EditorGUILayout.PropertyField(segements);
        serializedObject.ApplyModifiedProperties();
    }
}