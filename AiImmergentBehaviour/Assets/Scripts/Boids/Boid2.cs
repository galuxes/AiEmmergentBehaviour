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
    [SerializeField] private float _minDistance, _maxDistance, _maxSepForce;
    private List<GameObject> _withinMin = new List<GameObject>(), _withinMax = new List<GameObject>();

    [SerializeField] public float _cWeight, _sWeight, _aWeight;
    [SerializeField] public bool _cActive, _sActive, _aActive;

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
    
    private void ApplyRules()
    {
        PopulateLists();
        Vector2 cVector = _cActive ? Cohesion() : Vector2.zero;
        Vector2 sVector = _sActive ? Separation() : Vector2.zero;
        Vector2 aVector = _aActive ? Alignment() : Vector2.zero;

        CalculateVelocity(cVector, sVector, aVector);
    }
    
    private void PopulateLists()
    {
        Vector3 pos = transform.position;
        _withinMax = bm.FindGameObjectsInRange(_maxDistance, pos, gameObject);
        _withinMin = bm.FindGameObjectsInRange(_minDistance, pos, gameObject);
    }

    private Vector2 Cohesion()
    {
        Vector3 centerMass = Vector3.zero;
        
        if (_withinMax.Count > 0)
        {
            centerMass = _withinMax.Aggregate(centerMass, (current, boid) => current + boid.transform.position);
            centerMass /= _withinMax.Count;
            Vector2 direction = centerMass - transform.position;
        
            return direction / _maxDistance;
        }

        return Vector2.zero;
    }

    private Vector2 Separation()
    {
        Vector3 separationForce = Vector3.zero;
        if (_withinMin.Count > 0)
        {
            separationForce = _withinMin.Select(boid => transform.position - boid.transform.position).Aggregate(separationForce, (current, vector) => current + vector / vector.sqrMagnitude);
        }

        if (separationForce.sqrMagnitude > _maxSepForce*_maxSepForce)
        {
            separationForce = separationForce.normalized * _maxSepForce;
        }
        
        return separationForce;
    }

    private Vector2 Alignment()
    {
        Vector2 average = Vector2.zero;
        if (_withinMin.Count > 0)
        {
            average = _withinMax.Aggregate(average, (current, boid) => current + boid.GetComponent<Rigidbody2D>().velocity);

            average /= _withinMax.Count;
        }

        return average;
    }

    private void CalculateVelocity(Vector3 cohesion, Vector3 separation, Vector3 alignment)
    {
        _newVelocity = cohesion * _cWeight + separation * _sWeight + alignment * _aWeight;
    }
    
    private void StayInBounds()
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
        
        Vector2 currVel = _rb.velocity;
        
        Vector2 normVel = currVel.normalized;
        
        transform.up = normVel;

        if (_rb.velocity.sqrMagnitude < _speed*_speed)
        {
            _rb.velocity = (Vector2)transform.up * (_speed * Time.deltaTime);
        }
        
        if (_rb.velocity.sqrMagnitude > _maxSpeed*_maxSpeed)
        {
            _rb.velocity = normVel * (_maxSpeed * Time.deltaTime);
        }

        if (_inBounds)
        {
            _edgeVelocity *= .95f;
            if (_edgeVelocity.sqrMagnitude is > -0.4f and < 0.4f)
            {
                _edgeVelocity = Vector2.zero;
            }
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
