using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mqttDigitalTwin : MonoBehaviour
{
    public string topicPublish;
    public mqttReceiver mqttReceiver;
    public Button BtnForward;

    public TextMeshProUGUI textOp1;
    public TextMeshProUGUI textOp2;
    public TextMeshProUGUI textOp3;

    public JSONManipulation JM;

    //CanvasSendData
    public GameObject objectToShow;
    public float showDuration = 10f;
    public GameObject receivedShow;
    public float receivedDuration = 2f;

    
    private int nextStep = 1;
    private bool waitingForZeroResponse = false;
    private TextMeshProUGUI[] textsToBoldify;

    private void Start()
    {
        mqttReceiver.OnMessageArrived += OnMessageArrived;
        BtnForward.onClick.AddListener(PublishNextStep);
        
        // Obter referências aos textos
        textsToBoldify = new TextMeshProUGUI[]
        {
            textOp1,
            textOp2,
            textOp3
        };

        //CanvasSendData
        objectToShow.SetActive(false);
    }

    private void OnDestroy()
    {
        mqttReceiver.OnMessageArrived -= OnMessageArrived;
    }

    public void PublishNextStep()
    {
        if (!waitingForZeroResponse)
        {
            mqttReceiver.topicPublish = topicPublish;
            mqttReceiver.messagePublish = nextStep.ToString();
            mqttReceiver.Publish();
            
            ApplyBoldStyle();
            OpenCanvasSendData();
            
            waitingForZeroResponse = true;
        }
    }

    private void OnMessageArrived(string newMsg)
    {
        Debug.Log("Received message: " + newMsg);

        if (newMsg == "-1")
        {
            waitingForZeroResponse = false;
            nextStep++;
            if (nextStep > 3)
            {
                nextStep = 1;
            }
        }
    }

    private void ApplyBoldStyle()
    {
        for (int i = 0; i < textsToBoldify.Length; i++)
        {
            TextMeshProUGUI text = textsToBoldify[i];
            if (i == nextStep - 1)
            {
                text.fontStyle = FontStyles.Bold;
            }
            else
            {
                text.fontStyle = FontStyles.Normal;
            }
        }
    }

    private void StartUnityOp(){
        if(nextStep == 1){
            //JM.PlayProgramFromJSON1();
        }
        else if(nextStep == 2){
            //JM.PlayProgramFromJSON2();
        }
        else if(nextStep == 3){
            //JM.PlayProgramFromJSON3();
        }
    }

    //CanvasSendData
    private void OpenCanvasSendData(){
        objectToShow.SetActive(true);
        StartCoroutine(HideObjectAfterDelay());
    }

    private IEnumerator HideObjectAfterDelay(){
        yield return new WaitForSeconds(showDuration);
        objectToShow.SetActive(false);
        OpenCanvasReceivedData();
    }

    private void OpenCanvasReceivedData(){
        receivedShow.SetActive(true);
        StartCoroutine(DataReceivedCanvas());
    }

    private IEnumerator DataReceivedCanvas(){
        yield return new WaitForSeconds(receivedDuration);
        receivedShow.SetActive(false);
        
        StartUnityOp();
    }
}