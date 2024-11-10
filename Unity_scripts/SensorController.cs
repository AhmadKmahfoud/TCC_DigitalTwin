using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    public List<Sensor> sensors;

    private void Start()
    {
        sensors = new List<Sensor>(GameObject.FindObjectsOfType<Sensor>());
    }

    //CASO QUEIRA DEBUGAR UM SENSOR
    //private void Update()
    //{
    //    foreach (Sensor sensor in sensors)
    //    {
    //        if (sensor.objectDetected)
    //        {
    //            Debug.Log("Object passing through the sensor: " + sensor.nome);
    //        }
    //    }
    //}

}