using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Sensor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string Name;
    private bool objectDetected = false;

    public Sensor(string name)
    {
        Name = name;
    }

    public bool ObjectDetected => objectDetected; // Public getter for controlled access

    //Se for sensor de presenca (usa triggerEnter/Exit)
    private void OnTriggerEnter(Collider other)
    {
        objectDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        objectDetected = false;
    }

    //Se o sensor for um botao (usa OnPointerDown/Up, igual um'OnClick')
    public void OnPointerDown(PointerEventData eventData)
    {
        objectDetected = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        objectDetected = false;
    }
}
