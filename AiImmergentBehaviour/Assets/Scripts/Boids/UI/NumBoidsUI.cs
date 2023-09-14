using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumBoidsUI : MonoBehaviour
{
    [SerializeField] private BoidManager2 _bm;
    [SerializeField] private List<int> options = new List<int>();
    private TMP_Dropdown _dropdown;
    
    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    public void ValueChanged()
    {
        _bm.SetNumBoids(options[_dropdown.value]);
    }
}
