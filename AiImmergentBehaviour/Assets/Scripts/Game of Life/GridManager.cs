using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridBuffer;
    [SerializeField] private float _timeBetweenGenerations;
    [SerializeField] private bool _isPaused = true;
    [SerializeField] private int _generation = 0;
    [SerializeField] private GameObject _tile;
    [SerializeField] private List<Tile> _grid = new List<Tile>();

    private void Start()
    {
        StartCoroutine(Generation());
    }

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
                var newObj = Instantiate(_tile, tilePosition, quaternion.identity, transform);
                var newTile = newObj.GetComponent<Tile>();
                _grid.Add(newTile);
                newTile.tileLocation = Vector2Int.right * x + Vector2Int.up * y;
            }
        }
        PopulateTileNeighborhoods();
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

    private void PopulateTileNeighborhoods()
    {
        int lastIndex = _grid.Count;
        for (int i = 0; i < lastIndex; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1*_gridSize.y; k <= _gridSize.y; k += _gridSize.y)
                {
                    if (i + k + j >= 0 && i + k + j < lastIndex && i + k + j != i)
                    {
                        _grid[i].AddToNeighborhood(_grid[i + k + j]);
                    }
                }
            }
        }
    }

    private IEnumerator Generation()
    {
        while (true)
        {
            while (!_isPaused)
            {
                yield return new WaitForSeconds(_timeBetweenGenerations);
                foreach (var tile in _grid)
                {
                    tile.Flip();
                }
                _generation++;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void TogglePaused()
    {
        _isPaused = !_isPaused;
    }
}
