using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NW_CustumContenctMenu : EditorWindow {
    [MenuItem("GameObject/3D Object/HexaTile")]
    public static void Create_HexaTile() {
        GameObject go = new GameObject();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[] {
            new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 0f),
            new Vector3(1f, -1f, 0f), new Vector3(-1f, -1f, 0f)
        };
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        go.GetComponent<MeshFilter>().mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        go.GetComponent<MeshRenderer>().material = material;
    }

}
