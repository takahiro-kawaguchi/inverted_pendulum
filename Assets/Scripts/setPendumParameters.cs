using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPendumParameters : MonoBehaviour
{
    public Vector3 size;
    //public Vector3 inertia;
    //public Vector3 centerOfMass;
    public float initialDegree;
    private ArticulationBody abody;
    private GameObject child;
    public Collider col1;
    public Collider col2;
    // Start is called before the first frame update
    void Start()
    {
        child = child = transform.GetChild(0).gameObject;
        child.transform.localScale = size;
        child.transform.localPosition = new Vector3(0f, size.y / 2f, 0f);
        abody = GetComponent<ArticulationBody>();
        //abody.centerOfMass = new Vector3(0f, size.y / 2f, 0f);
        List<float> positions = new List<float>();
        abody.GetJointPositions(positions);
        positions[1] = Mathf.Deg2Rad*initialDegree;
        abody.SetJointPositions(positions);
        Physics.IgnoreCollision(col1, col2);
        //float m = abody.mass;
        //inertia = (size.sqrMagnitude*Vector3.one - Vector3.Scale(size, size))/12f*m;
        //abody.inertiaTensor = inertia;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
