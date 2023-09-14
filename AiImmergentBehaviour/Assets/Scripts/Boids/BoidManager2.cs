using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class BoidManager2 : MonoBehaviour
{
    [SerializeField] private Vector2 _screenSize;
    public int _numBoids;
    [SerializeField] private float sWeight, cWeight, aWeight;
    [SerializeField] private bool sEnabled, cEnabled, aEnabled;
    [SerializeField] private GameObject _boid;
    [SerializeField] private List<GameObject> _boids = new List<GameObject>();
    private List<Boid2> _boidComponents = new List<Boid2>();

    private void Start()
    {
        SpawnBoids();
    }
    
    [ContextMenu("Gen Boids")]
    private void SpawnBoids()
    {
        for (int i = 0; i < _numBoids; i++)
        {
            SpawnBoid();
        }
    }
    
    void SpawnBoid()
    {
        Vector3 randLocation = new Vector3(Random.Range(-1 * _screenSize.x/2, _screenSize.x/2), Random.Range(-1 * _screenSize.y/2, _screenSize.y/2), 0);
        Quaternion randRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 359));
        GameObject boid = Instantiate(_boid, randLocation, randRotation);
        boid.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
        boid.GetComponent<Boid2>().screenSize = _screenSize;
        boid.GetComponent<Boid2>().bm = this;
        boid.GetComponent<Rigidbody2D>().velocity = boid.transform.up;
        boid.GetComponent<TrailRenderer>().startColor = Random.ColorHSV();
        _boids.Add(boid);
        _boidComponents.Add(boid.GetComponent<Boid2>());
    }

    public List<GameObject> FindGameObjectsInRange(float radius, Vector3 boidLocation, GameObject requestingBoid)
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach (var boid in _boids)
        {
            if ((boid != requestingBoid ) && ((boidLocation - boid.transform.position).sqrMagnitude <= radius*radius))
            {
                tempList.Add(boid);
            }
        }

        return tempList;
    }
    
    [ContextMenu("Update Boids")]
    private void ValueChanged()
    {
        if (_numBoids != _boids.Count)
        {
            if (_numBoids > _boids.Count)
            {
                int numRun = (_numBoids - _boids.Count);
                for (int i = 0; i < numRun; i++)
                {
                    SpawnBoid();
                }
            }
            else
            {
                int numRun = (_boids.Count - _numBoids);
                for (int i = 0; i < numRun; i++)
                {
                    _boidComponents.Remove(_boidComponents[^1]);
                    var lastBoid = _boids[^1];
                    _boids.Remove(lastBoid);
                    DestroyImmediate(lastBoid);
                }
            }
        }
        foreach (var boid in _boidComponents)
        {
            boid._sActive = sEnabled;
            boid._cActive = cEnabled;
            boid._aActive = aEnabled;
            boid._sWeight = sWeight;
            boid._cWeight = cWeight;
            boid._aWeight = aWeight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _screenSize);
    }

    public void SetSWeight(float newSWeight)
    {
        sWeight = newSWeight;
        ValueChanged();
    }
    
    public void SetCWeight(float newCWeight)
    {
        cWeight = newCWeight;
        ValueChanged();
    }
    
    public void SetAWeight(float newAWeight)
    {
        aWeight = newAWeight;
        ValueChanged();
    }

    public void SetNumBoids(int newNumBoids)
    {
        _numBoids = newNumBoids;
        ValueChanged();
    }

    public void ToggleSActive()
    {
        sEnabled = !sEnabled;
        ValueChanged();
    }
    
    public void ToggleCActive()
    {
        cEnabled = !cEnabled;
        ValueChanged();
    }
    
    public void ToggleAActive()
    {
        aEnabled = !aEnabled;
        ValueChanged();
    }

}
