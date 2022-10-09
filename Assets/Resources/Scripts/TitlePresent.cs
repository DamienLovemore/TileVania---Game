using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePresent : MonoBehaviour
{
    [SerializeField] private GameObject levelTitle;
    [SerializeField] private float textShowTime = 2f;

    void Start()
    {
        Destroy(levelTitle, textShowTime);
    }
}
