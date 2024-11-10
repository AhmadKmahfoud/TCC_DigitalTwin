using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//"Objeto Controlado" que publica a posicao do EndEffector a cada 2 segundos, se esse mudar de posicao
public class mqttPositionSender : MonoBehaviour
{
    public string topicPublish;
    public Transform point;
    public mqttReceiver mqttReceiver;

    private Vector3 lastPosition;
    private bool shouldPublish = false;

    private void Start()
    {
        mqttReceiver.OnMessageArrived += OnMessageArrived;
        lastPosition = point.position;
        StartCoroutine(PublishPosition());
    }

    private void OnDestroy()
    {
        mqttReceiver.OnMessageArrived -= OnMessageArrived;
    }

    private void Update()
    {
        // Check if the target has moved
        if (point.position!= lastPosition)
        {
            shouldPublish = true;
            lastPosition = point.position;
        }
    }

    IEnumerator PublishPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(2); // Wait for 2 seconds

            if (shouldPublish)
            {
                mqttReceiver.topicPublish = topicPublish;
                // Assuming you have a way to set the messagePublish property
                mqttReceiver.messagePublish = point.position.ToString();
                mqttReceiver.Publish();
                shouldPublish = false; // Reset flag after publishing
            }
        }
    }

    private void OnMessageArrived(string newMsg)
    {
        Debug.Log("Received message: " + newMsg);
    }
}
