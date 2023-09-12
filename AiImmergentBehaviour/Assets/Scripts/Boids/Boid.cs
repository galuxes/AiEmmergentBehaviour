using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TrailRenderer _tr;
    [SerializeField] private float _speed;
    [NonSerialized] public Vector2 screenSize;
    [SerializeField] private float _minDistance, _maxDistance;
    private List<GameObject> _withinMin = new List<GameObject>(), _withinMax = new List<GameObject>();

    [SerializeField] private float _cWeight, _sWeight, _aWeight;
    [SerializeField] private bool _cActive, _sActive, _aActive;

    private Vector2 _newVelocity = Vector2.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        _tr.emitting = true;
        StayInBounds();
        ApplyRules();
    }

    void ApplyRules()
    {
        PopulateLists();
        Vector2 cVector = _cActive ? Cohesion() : Vector2.zero;
        Vector2 sVector = _sActive ? Separation() : Vector2.zero;

        CalculateVelocity(cVector, sVector);
    }

    void PopulateLists()
    {
        _withinMax.Clear();
        var maxCollider2Ds= Physics2D.OverlapCircleAll(transform.position, _maxDistance);
        foreach (var collider in maxCollider2Ds)
        {
            _withinMax.Add(collider.gameObject);
        }
        
        _withinMin.Clear();
        var minCollider2Ds= Physics2D.OverlapCircleAll(transform.position, _maxDistance);
        foreach (var collider in minCollider2Ds)
        {
            _withinMin.Add(collider.gameObject);
        }
    }

    Vector2 Cohesion()
    {
        Vector3 centerMass = Vector3.zero;
        foreach (var obj in _withinMax)
        {
            centerMass += obj.transform.position;
        }

        centerMass /= _withinMax.Count;

        Vector2 direction = centerMass - transform.position;

        if ( direction.magnitude <= _maxDistance)
        {
            return direction / _maxDistance;
        }
        else
        {
            return Vector2.zero;
        }
    }

    Vector2 Separation()
    {
        return Vector2.zero;
    }

    void CalculateVelocity(Vector3 cohesion, Vector3 seperation)
    {
        _newVelocity = cohesion * _cWeight + seperation * _sWeight;
    }
    
    void StayInBounds()
    {
        var pos = transform.position;
        if (pos.x > screenSize.x/2)
        {
            _tr.emitting = false;
            transform.position = new Vector2(screenSize.x / 2 * -1, pos.y);
        }
        else if (pos.x < screenSize.x/2 * -1)
        {
            _tr.emitting = false;
            transform.position = new Vector2(screenSize.x / 2, pos.y);
        }
        
        if (pos.y > screenSize.y/2)
        {
            _tr.emitting = false;
            transform.position = new Vector2(pos.x, screenSize.y / 2 * -1);
        }
        else if (pos.y < screenSize.y/2 * -1)
        {
            _tr.emitting = false;
            transform.position = new Vector2(pos.x, screenSize.y / 2 );
        }
        
    }

    private void FixedUpdate()
    {
        _rb.velocity += _newVelocity * Time.deltaTime;
        //_rb.velocity += (Vector2)transform.up * _speed;
        transform.up = _rb.velocity.normalized;

        if (_rb.velocity.sqrMagnitude < _speed)
        {
            _rb.velocity += _speed * (Vector2)transform.up * Time.deltaTime;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _minDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
    }
}
