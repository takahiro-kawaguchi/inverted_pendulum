using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public CarSettings car = new CarSettings();
    public PendulumSettings pendulum = new PendulumSettings();
    public DisturbanceSettings disturbance = new DisturbanceSettings();
    public UDPSettings UDP = new UDPSettings();
    public GameObject root;

    void Start()
    {
        ArticulationBody carJointBody = root.transform.Find("CarJoint").gameObject.GetComponent<ArticulationBody>();
        carJointBody.mass = car.mass;
        carJointBody.jointFriction = car.jointFriction;
        carJointBody.linearDamping = car.linearDamping;
        carJointBody.angularDamping = car.angularDamping;
        GameObject pendulumJoint = root.transform.Find("CarJoint/PendulumJoint").gameObject;
        ArticulationBody pendulumJointBody = pendulumJoint.GetComponent<ArticulationBody>();
        pendulumJointBody.mass = this.pendulum.mass;
        pendulumJointBody.jointFriction = this.pendulum.jointFriction;
        pendulumJointBody.linearDamping = this.pendulum.linearDamping;
        pendulumJointBody.angularDamping = this.pendulum.angularDamping;

        List<float> positions = new List<float>();
        ArticulationBody abody = root.GetComponent<ArticulationBody>();
        abody.GetJointPositions(positions);
        positions[0] = car.initialPosition;
        positions[1] = Mathf.Deg2Rad * this.pendulum.initialAngle;
        abody.SetJointPositions(positions);

        GameObject pendulum = pendulumJointBody.transform.Find("Pendulum").gameObject;
        Vector3 size = pendulum.transform.localScale;
        size.y = this.pendulum.length;
        pendulum.transform.localScale = size;
        pendulum.transform.localPosition = new Vector3(0f, size.y / 2f, 0f);

        UDPApp udp = transform.Find("Controller").gameObject.GetComponent<UDPApp>();
        udp.sendAddress = UDP.sendAddress;
        udp.sendPort = UDP.sendPort;
        udp.recievePort = UDP.recievePort;
        udp.UDPStart();

        pendulumJoint.GetComponent<Disturbance>().magnitude = disturbance.magnitude;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public struct CarSettings
{
    public float mass;
    public float initialPosition;
    public float jointFriction;
    public float linearDamping;
    public float angularDamping;
}


[Serializable]
public struct PendulumSettings
{
    public float length;
    public float mass;
    [Range(-180f, 180f)]
    public float initialAngle;
    public float jointFriction;
    public float linearDamping;
    public float angularDamping;
}


[Serializable]
public struct UDPSettings
{
    public int recievePort;
    public int sendPort;
    public string sendAddress;
}

[Serializable]
public struct DisturbanceSettings
{
    public float magnitude;
}