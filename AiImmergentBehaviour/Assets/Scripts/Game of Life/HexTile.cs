using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexTile : MonoBehaviour
{
    [SerializeField] private bool _isAlive = false, _willBeAlive;
    private SpriteRenderer _sr;
    private Mouse _mouse;
    public List<HexTile> _neighborhood = new List<HexTile>();
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

    public void AddToNeighborhood(HexTile neighboringTile)
    {
        _neighborhood.Add(neighboringTile);
    }

    private int GetNumNeighbors()
    {
        return _neighborhood.Count(tile => tile._isAlive);
    }

    private void CalcNextState()
    {
        _willBeAlive = GetNumNeighbors() == 2;
    }

    public void Flip()
    {
        _isAlive = _willBeAlive;
    }
}
