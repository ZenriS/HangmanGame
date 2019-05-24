using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingScreen_Script : MonoBehaviour
{
    private Transform _loadingIcon;

    
    void Start()
    {
        _loadingIcon = transform.GetChild(1);
    }

    void Update()
    {
        _loadingIcon.transform.Rotate(new Vector3(0,0,45 * Time.deltaTime));
    }
}
