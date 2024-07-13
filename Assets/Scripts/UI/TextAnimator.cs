using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TMP_Text))]
public class TextAnimator : MonoBehaviour
{
    [SerializeField] public float RotateSpeed = 1f;
    [SerializeField] public float RotateAngle = 20f;
    [SerializeField] public float ScaleSpeed = 1f;
    [SerializeField] public float ScaleMin = 20f;
    [SerializeField] public float ScaleMax = 20f;
    [SerializeField] public float Delay = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RotateSpeed != 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotateAngle * Mathf.Sin(RotateSpeed * Time.time + Delay)));
        // min = 0, max = 2
        // Sin(Time.time) + 1;
        // min = 0, max = 4
        // (Sin(Time.time) + 1) * 2;
        // min = 0, max = 3
        // (Sin(Time.time) + 1) / 2 * max;
        // min = 1, max = 4
        // (Sin(Time.time) + 2)
        if (ScaleSpeed != 0f)
        {
            float scaleFactor = (Mathf.Cos(ScaleSpeed * Time.time + Delay) + 1) / 2 * (ScaleMax - ScaleMin) + 1;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
    }

}
