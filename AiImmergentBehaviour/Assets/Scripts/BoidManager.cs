using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private Vector2 _screenSize;
    [SerializeField] private int _numBoids;
    [SerializeField] private GameObject _boid;

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
            boid.GetComponent<Boid>().screenSize = _screenSize;
            boid.GetComponent<Rigidbody2D>().velocity = boid.transform.up;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _screenSize);
    }
}
