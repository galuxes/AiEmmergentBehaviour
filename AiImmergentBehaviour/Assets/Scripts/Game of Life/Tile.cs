using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool _isAlive = false, _willBeAlive;
    private SpriteRenderer _sr;
    private Mouse _mouse;
    public List<Tile> _neighborhood = new List<Tile>();
    public Vector2Int tileLocation = Vector2Int.zero;
        
    private void Start()
    {
        _mouse = Mouse.current;
        _sr = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        _sr.color = _isAlive ? Color.black : Color.white;
        CalcNextState();
    }

    private void OnMouseDown()
    {
        _isAlive = !_isAlive;
    }

    public void AddToNeighborhood(Tile neighboringTile)
    {
        _neighborhood.Add(neighboringTile);
    }

    private int GetNumNeighbors()
    {
        int numAlive = 0;
        foreach (var tile in _neighborhood)
        {
            if (tile._isAlive)
            {
                numAlive++;
            }
        }
        return numAlive;
    }

    public void CalcNextState()
    {
        _willBeAlive = false;
        switch (GetNumNeighbors())
        {
            case 2:
                if (_isAlive)
                {
                    _willBeAlive = true;
                }
                break;
            case 3:
                _willBeAlive = true;
                break;
            default:
                break;
        }
    }

    public void Flip()
    {
        _isAlive = _willBeAlive;
    }
}
