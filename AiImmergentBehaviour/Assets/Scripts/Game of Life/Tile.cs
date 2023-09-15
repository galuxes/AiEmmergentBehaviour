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

    private int _numNeighbors;
        
    private void Start()
    {
        _mouse = Mouse.current;
        _sr = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        _sr.color = _isAlive ? Color.black : Color.white;
    }

    private void OnMouseDown()
    {
        _isAlive = !_isAlive;
    }

    private void GetNumNeighbors()
    {
        
    }

    private void CalcNextState()
    {
        GetNumNeighbors();
        _willBeAlive = false;
        switch (_numNeighbors)
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
        CalcNextState();
    }
}
