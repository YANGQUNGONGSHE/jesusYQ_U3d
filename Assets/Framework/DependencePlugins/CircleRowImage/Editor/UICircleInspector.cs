using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CircleRawImage), true)]
[CanEditMultipleObjects]
public class UICircleInspector : RawImageEditor
{

public override void OnInspectorGUI()
{
base.OnInspectorGUI();
    CircleRawImage circleRawImage = target as CircleRawImage;
circleRawImage.segments = Mathf.Clamp(EditorGUILayout.IntField("UICircle多边形", circleRawImage.segments), 4, 360);

}

}