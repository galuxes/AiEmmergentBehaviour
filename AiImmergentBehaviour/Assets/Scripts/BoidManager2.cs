using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager2 : MonoBehaviour
{
    [SerializeField] private Vector2 _screenSize;
    [SerializeField] private int _numBoids;
    [SerializeField] private GameObject _boid;
    [SerializeField] private List<GameObject> _boids = new List<GameObject>();

    private void Start()
    {
        SpawnBoids(_numBoids);
    }

    void SpawnBoids(int numBoids)
    {
        for (int i = 0; i < numBoids; i++)
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
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _screenSize);
    }
}
