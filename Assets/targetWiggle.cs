using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetWiggle : MonoBehaviour
{

    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sine(sineWaveSpeed, amplitude);
    }

    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }
}
