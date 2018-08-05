using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NW_UI_ELEMENT))]
public class NW_UI_ELEMENT_CustumInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        NW_UI_ELEMENT cp = target as NW_UI_ELEMENT;
        if(GUILayout.Button("Resize Element")) { cp.ResizeElement(); }
    }
}

[CustomEditor(typeof(CharectorViewer_SHT))]
public class CharectorViewer_SHT_CustumInspector : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        CharectorViewer_SHT cp = target as CharectorViewer_SHT;
        if (GUILayout.Button("Refresh UI Size")) { cp.RefreshSize_UI(); }
    }
}