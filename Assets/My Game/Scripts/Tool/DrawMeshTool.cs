using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DrawMeshTool : EditorWindow
{
    private bool paintMode = false;
    public GameObject spawnMesh;
    public float radius = 3f;
    public int amount = 10;
    public LayerMask terrainLayer;
    [Range(0f, 10f)]
    public float sizeSpawn = 1f;

    [MenuItem("Tools/Mesh Spawner")]
    public static void ShowWindow()
    {
        GetWindow<DrawMeshTool>("Mesh Spawner");
    }
    public void OnGUI()
    {
        GUILayout.Label("Mesh Spawner Tool", EditorStyles.boldLabel);

        spawnMesh = (GameObject)EditorGUILayout.ObjectField("SpawnMesh", spawnMesh, typeof(GameObject), false);
        radius = EditorGUILayout.FloatField("Brush Radius", radius);
        amount = EditorGUILayout.IntField("Amount", amount);
        sizeSpawn = EditorGUILayout.FloatField("Size Spawn", sizeSpawn);
        terrainLayer = EditorGUILayout.LayerField("Terrain Layer", terrainLayer);

        if (GUILayout.Button(paintMode ? "Stop Spawner" : "Start Spawning"))
        {
            paintMode = !paintMode;
            SceneView.duringSceneGui -= OnSceneGUI;
            if (paintMode)
            {
                SceneView.duringSceneGui += OnSceneGUI;
            }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!paintMode) return;
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << terrainLayer))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    PaintObject(hit.point);
                    e.Use();
                }
            }
        }
        RaycastHit drawHit;
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out drawHit, Mathf.Infinity, 1 << terrainLayer))
        {
            Handles.color = UnityEngine.Color.green;
            Handles.DrawWireDisc(drawHit.point, Vector3.up, radius);
        }
        sceneView.Repaint();
    }

    private void PaintObject(Vector3 center)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPos = center + new Vector3(UnityEngine.Random.Range(-radius, radius), 0f, UnityEngine.Random.Range(-radius, radius));


            RaycastHit hit;
            if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, 1 << terrainLayer))
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(spawnMesh);
                instance.transform.position = hit.point;
                instance.transform.SetParent(hit.collider.transform);

                float randomYRotation = UnityEngine.Random.Range(0f, 360f);
                instance.transform.Rotate(0, randomYRotation, 0);
                instance.transform.localScale *= sizeSpawn;
            }
        }
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
