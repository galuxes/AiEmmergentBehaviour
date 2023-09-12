using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool _isAlive = false;
    private SpriteRenderer _sr;
    private Mouse _mouse;

    private List<Tile> _neighboorhood;
        
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
}
