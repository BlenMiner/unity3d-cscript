using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(
            (Mathf.PerlinNoise(0, Time.time) - 0.5f) * Time.deltaTime * 1000, 
            (Mathf.PerlinNoise(Time.time, 0) - 0.5f)* Time.deltaTime * 1000, 
            (Mathf.PerlinNoise(Time.time, Time.time) - 0.5f) * Time.deltaTime * 1000
        );
    }
}
