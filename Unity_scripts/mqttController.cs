using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//"Objeto Controlado" generico, usando tag para identificar o Receiver
public class mqttController : MonoBehaviour
{
    public string nameController = "RV-4FRL";
    public string tagOfTheMQTTReceiver="MQTT_ReceiverTag";
    public mqttReceiver _eventSender;

  void Start()
  {
    //_eventSender=GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<mqttReceiver>();
    _eventSender.OnMessageArrived += OnMessageArrivedHandler;
  }

  private void OnMessageArrivedHandler(string newMsg)
  {
    Debug.Log("Event Fired. The message, from Object " +nameController+" is = " + newMsg);
  }
}