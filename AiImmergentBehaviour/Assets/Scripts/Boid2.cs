using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid2 : MonoBehaviour
{
    private Rigidbody2D _rb;
    [NonSerialized] public BoidManager2 bm;
    [SerializeField] private float _speed, _maxSpeed, _edgeForce;
    [NonSerialized] public Vector2 screenSize;
    [SerializeField] private float _minDistance, _maxDistance;
    private List<GameObject> _withinMin = new List<GameObject>(), _withinMax = new List<GameObject>();

    [SerializeField] private float _cWeight, _sWeight, _aWeight;
    [SerializeField] private bool _cActive, _sActive, _aActive;

    [SerializeField] private Vector2 _newVelocity = Vector2.zero, _edgeVelocity = Vector2.zero;

    private bool _inBounds;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyRules();
        StayInBounds();
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
        _withinMax = bm.FindGameObjectsInRange(_maxDistance, transform.position, gameObject);
        _withinMin = bm.FindGameObjectsInRange(_minDistance, transform.position, gameObject);
    }

    Vector2 Cohesion()
    {
        Vector3 centerMass = Vector3.zero;
        
        if (_withinMax.Count > 0)
        {
            centerMass = _withinMax.Aggregate(centerMass, (current, obj) => current + obj.transform.position);
            Vector2 direction = centerMass - transform.position;
        
            return direction / _maxDistance;
        }

        return Vector2.zero;
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
        _inBounds = true;
        
        if (pos.x > screenSize.x/2)
        {
            _edgeVelocity += Vector2.left * (_edgeForce * Time.deltaTime);
            _inBounds = false;
        }
        
        if (pos.x < screenSize.x/2 * -1)
        {
            _edgeVelocity += Vector2.right * (_edgeForce * Time.deltaTime);
            _inBounds = false;
        }
        
        if (pos.y > screenSize.y/2)
        {
            _edgeVelocity += Vector2.down * (_edgeForce * Time.deltaTime);
            _inBounds = false;
        }
        
        if (pos.y < screenSize.y/2 * -1)
        {
            _edgeVelocity += Vector2.up * (_edgeForce * Time.deltaTime);
            _inBounds = false;
        }
        
    }

    private void FixedUpdate()
    {
        _rb.velocity += _edgeVelocity + _newVelocity * Time.deltaTime;
        Vector2 normVel = _rb.velocity.normalized;
        transform.up = normVel;

        if (_rb.velocity.sqrMagnitude < _speed*_speed)
        {
            //_rb.velocity += (Vector2)transform.up * (_speed * Time.deltaTime);
        }
        
        if (_rb.velocity.sqrMagnitude > _speed*_speed)
        {
            _rb.velocity = normVel * (_maxSpeed * Time.deltaTime);
        }

        if (_inBounds)
        {
            _edgeVelocity *= .95f;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _minDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _edgeVelocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _newVelocity);

    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _edgeVelocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _newVelocity);*/
        
    }
}
