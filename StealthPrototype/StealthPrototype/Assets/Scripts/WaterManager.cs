using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object must have these components attached
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;

    private void Awake() {
        meshFilter = gameObject.GetComponent<MeshFilter>();
    }

    private void Update() {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for(int i = 0; i < vertices.Length; i ++){
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals(); //Make sure normals are correct
    }
}
