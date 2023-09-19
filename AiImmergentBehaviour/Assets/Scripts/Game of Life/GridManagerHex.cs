using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class GridManagerHex : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridBuffer;
    [SerializeField] private float _timeBetweenGenerations;
    [SerializeField] private bool _isPaused = true;
    [SerializeField] private int _generation = 0;
    [SerializeField] private GameObject _tile;
    [SerializeField] private List<HexTile> _grid = new List<HexTile>();

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
                
                if (x % 2 == 0)
                {
                    tilePosition = Vector3.right * (x * _gridBuffer.x) + Vector3.up * (y * _gridBuffer.y);
                }
                else
                {
                    tilePosition = Vector3.right * (x * _gridBuffer.x) + Vector3.up * (y * _gridBuffer.y) - Vector3.up * (_gridBuffer.y/2);
                }

                tilePosition += startPosition;
                var newObj = Instantiate(_tile, tilePosition, quaternion.identity, transform);
                var newTile = newObj.GetComponent<HexTile>();
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
                DestroyImmediate(tile.gameObject);
            }
            _grid.Clear();
        }
    }

    private void PopulateTileNeighborhoods()
    {
        int lastIndex = _grid.Count;
        for (int i = 0; i < lastIndex; i++)
        {
            var indexToAdd = i - (2 * _gridSize.y);
            if (IndexValid(indexToAdd))
            {
                _grid[i].AddToNeighborhood(_grid[indexToAdd]);
            }
            
            if ((int)(i / _gridSize.y) % 2 == 0)
            {
                //up
                indexToAdd += _gridSize.y;
                if (IndexValid(indexToAdd))
                {
                    _grid[i].AddToNeighborhood(_grid[indexToAdd]);
                }
            }
            else
            {
                //down
                indexToAdd += _gridSize.y - 1;
                if (IndexValid(indexToAdd))
                {
                    _grid[i].AddToNeighborhood(_grid[indexToAdd]);
                }
            }
            
            indexToAdd++;
            if (IndexValid(indexToAdd))
            {
                _grid[i].AddToNeighborhood(_grid[indexToAdd]);
            }

            indexToAdd += (_gridSize.y * 2) - 1;
            if (IndexValid(indexToAdd))
            {
                _grid[i].AddToNeighborhood(_grid[indexToAdd]);
            }
            
            indexToAdd++;
            if (IndexValid(indexToAdd))
            {
                _grid[i].AddToNeighborhood(_grid[indexToAdd]);
            }

            indexToAdd = i + 2 * _gridSize.y;
            if (IndexValid(indexToAdd))
            {
                _grid[i].AddToNeighborhood(_grid[indexToAdd]);
            }
        }
    }

    private bool IndexValid(int indexToCheck)
    {
        return (indexToCheck < _grid.Count) && (indexToCheck >= 0);
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
