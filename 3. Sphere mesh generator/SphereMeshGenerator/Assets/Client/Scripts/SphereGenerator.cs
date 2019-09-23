using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField, Range(3,20)] private int lonSegments;
    [SerializeField, Range(2,20)] private int latSegments;
    [SerializeField, Range(0,1)] private float _cylinder;
    [SerializeField, Range(2,300)] private int _vertexCount;

    private GameObject _sphere;
    private bool _generating;

    #region MonoBehaviour

    private void OnValidate()
    {
        if (_sphere == null)
        {
            Debug.Log("Hello");
            _sphere = CreateSphere();
            _sphere.GetComponent<MeshRenderer>().material = _material;
        }
        
        var sphereMesh = GenerateSphereMeshData();
        _sphere.GetComponent<MeshFilter>().mesh = sphereMesh;
    }
    
    #endregion
    
    private GameObject CreateSphere()
    {
        GameObject sphere = new GameObject("Sphere",typeof(MeshFilter),typeof(MeshRenderer));
        sphere.transform.parent = transform;

        return sphere;
    }

    public Mesh GenerateSphereMeshData() {
            var mesh = new Mesh();

            var vertices = new Vector3[(lonSegments + 1) * latSegments + 2];

            float pi2 = Mathf.PI * 2f;

            vertices[0] = Vector3.up;
            for (int lat = 0; lat < latSegments; lat++) {
                float a1 = Mathf.PI * (float)(lat + 1) / (latSegments + 1);
                float sin = Mathf.Sin(a1);
                float cos = Mathf.Cos(a1);

                for (int lon = 0; lon <= lonSegments; lon++)
                {
                    float a2 = pi2 * (float)lon / lonSegments;

                    float sin2 = Mathf.Sin(a2);
                    float cos2 = Mathf.Cos(a2);
                    vertices[lon + lat * (lonSegments + 1) + 1] = new Vector3(sin * cos2, cos, sin * sin2);
                }
            }
            vertices[vertices.Length - 1] = -Vector3.up;

            int len = vertices.Length;

            Vector2[] uvs = new Vector2[len];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (int lat = 0; lat < latSegments; lat++) {
                for (int lon = 0; lon <= lonSegments; lon++) {
                    uvs[lon + lat * (lonSegments + 1) + 1] = new Vector2((float)lon / lonSegments, 1f - (float)(lat + 1) / (latSegments + 1));
                }
            }

            int[] triangles = new int[len * 2 * 3];

            
            // top cap
            int acc = 0;
            for (int lon = 0; lon < lonSegments; lon++) {
                triangles[acc++] = lon + 2;
                triangles[acc++] = lon + 1;
                triangles[acc++] = 0;
            }

            // middle
            for (int lat = 0; lat < latSegments - 1; lat++) {
                for (int lon = 0; lon < lonSegments; lon++) {
                    int current = lon + lat * (lonSegments + 1) + 1;
                    int next = current + lonSegments + 1;

                    triangles[acc++] = current;
                    triangles[acc++] = current + 1;
                    triangles[acc++] = next + 1;

                    triangles[acc++] = current;
                    triangles[acc++] = next + 1;
                    triangles[acc++] = next;
                }
            }

            // bottom cap
            for (int lon = 0; lon < lonSegments; lon++) {
                triangles[acc++] = len - 1;
                triangles[acc++] = len - (lon + 2) - 1;
                triangles[acc++] = len - (lon + 1) - 1;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
}
