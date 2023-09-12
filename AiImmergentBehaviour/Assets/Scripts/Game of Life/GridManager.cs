using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridBuffer;
    [SerializeField] private GameObject _tile;
    [SerializeField] private List<GameObject> _grid = new List<GameObject>();

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        DeleteGrid();
        
        Vector3 startPosition = transform.position, tilePosition;
        
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                tilePosition = Vector3.right * (x * _gridBuffer.x) + Vector3.up * (y * _gridBuffer.y);
                tilePosition += startPosition;
                _grid.Add(Instantiate(_tile, tilePosition, quaternion.identity, transform));
            }
        }
        
    }

    [ContextMenu("Destroy Grid")]
    public void DeleteGrid()
    {
        if (_grid.Count > 0)
        {
            foreach (var tile in _grid)
            {
                DestroyImmediate(tile);
            }
            _grid.Clear();
        }
    }
    
}
