using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//"Objeto Controlado" que publica os angulos das juntas, se ao menos uma delas mudar de posicao
public class mqttJointSender : MonoBehaviour
{
    public string topicPublish;
    public Transform point;
    public mqttReceiver mqttReceiver;
    public IK_algoritmo IK;

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
                mqttReceiver.messagePublish = string.Join(", ", IK.finalAngles.Select(angle => angle.ToString()));
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
