using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlignmentWeightUI : MonoBehaviour
{
    [SerializeField] private BoidManager2 _bm; 
    private Slider _slider; 
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void ValueChanged()
    {
        _bm.SetAWeight(_slider.value);
    }
}
