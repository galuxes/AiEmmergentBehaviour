using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CohesionWeightUI : MonoBehaviour
{
    [SerializeField] private BoidManager2 _bm; 
    private Slider _slider; 
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void ValueChanged()
    {
        _bm.SetCWeight(_slider.value);
    }
}
