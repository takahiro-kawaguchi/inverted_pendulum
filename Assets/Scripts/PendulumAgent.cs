using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PendulumAgent : Agent
{
    Settings settings;
    Controller controller;
    float width;
    // Start is called before the first frame update
    void Start()
    {
        settings = transform.parent.GetComponent<Settings>();
        controller = GetComponent<Controller>();
        ArticulationDrive drive = controller.root.transform.Find("CarJoint").GetComponent<ArticulationBody>().zDrive;
        width = (drive.upperLimit - drive.lowerLimit)/2f;
    }

    public override void OnEpisodeBegin()
    {

        settings.Initialize();
        controller.u_agent = 0;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(target.transform.position.y);
        //        sensor.AddObservation(hand.transform.position.x);
        //        sensor.AddObservation(hand.transform.position.y);
        //        sensor.AddObservation(hand.transform.position.z);
        float theta = controller.positions[1];
        theta = Mathf.Atan2(Mathf.Sin(theta), Mathf.Cos(theta));
        controller.positions[1] = theta;
        sensor.AddObservation(controller.positions);
        sensor.AddObservation(controller.velocities);
        //Debug.Log(target_tire.transform.position.x - target.transform.position.x);
        //Debug.Log(target_tire.transform.position.y - target.transform.position.y);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        controller.u_agent = 100 * actionBuffers.ContinuousActions[0];
        float theta = controller.positions[1];
        theta = Mathf.Atan2(Mathf.Sin(theta), Mathf.Cos(theta));
        SetReward((-Mathf.Abs(theta) / Mathf.PI - Mathf.Abs(controller.positions[0])/width) + 1);
        //Debug.Log((-Mathf.Abs(theta) / Mathf.PI - Mathf.Abs(controller.positions[0])/width) + 1);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
