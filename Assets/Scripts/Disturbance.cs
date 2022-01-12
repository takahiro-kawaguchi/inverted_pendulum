using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour
{
    ArticulationBody abody;
    public float magnitude = 0;
    public bool random;
    public float probability = 0;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        abody = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        float mag;
        if (random)
        {
            mag = UnityEngine.Random.Range(-Mathf.Abs(magnitude), Mathf.Abs(magnitude));
        }
        else
        {
            mag = magnitude;
        }
        if (Input.GetKeyDown(key) || UnityEngine.Random.Range(0, 100) < probability)
        {
            abody.AddForce(Vector3.forward * mag, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {

    }
}
