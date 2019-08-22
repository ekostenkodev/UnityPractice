using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private int _cubeCount;
    [SerializeField] private Cube _cubePrefab;
    private Transform _selfTransform;
    
    public event UnityAction<Cube[]> CubesGenerated = delegate {};
    
    void Start()
    {
        _selfTransform = GetComponent<Transform>();
        
        GenerateCubes();
    }

    public void GenerateCubes()
    {
        Cube[] cubes = new Cube[_cubeCount];
        
        foreach (Transform child in _selfTransform)
        {
            Destroy(child.gameObject);
        }
        
        List<int> cubeValues = Enumerable.Range(0,_cubeCount).ToList();

        for (int i = 0; i < _cubeCount; i++)
        {
            cubes[i] = Instantiate(_cubePrefab, new Vector3(i, 0, 0), Quaternion.identity, _selfTransform);
            cubes[i].Value = cubeValues.GetRandomAndRemove();
        }
        
        CubesGenerated.Invoke(cubes);
    }
}
