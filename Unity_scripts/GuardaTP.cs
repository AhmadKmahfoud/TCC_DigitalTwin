using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuardaTP : MonoBehaviour
{
    [SerializeField] private GameObject TP_mitsubishi;
    [SerializeField] private Transform posTPguardado;
    [SerializeField] private GameObject BtnTP_Canvas;

    private Vector3 posInicial;
    private Quaternion rotInicial;

    private void Start()
    {
        if (TP_mitsubishi != null)
        {
            // Salva a posicao inicial do objeto TP_mitsubishi
            posInicial = TP_mitsubishi.transform.position;
            rotInicial = TP_mitsubishi.transform.rotation;
            Guarda_TP();
        }
        else
        {
            Debug.LogError("TP_mitsubishi não foi atribuído no Inspector!");
        }
    }

    public void Guarda_TP()
    {
        if (TP_mitsubishi != null && posTPguardado != null)
        {
            // Guarda o objeto TP_mitsubishi
            TP_mitsubishi.transform.position = posTPguardado.position;
            TP_mitsubishi.transform.rotation = posTPguardado.rotation;
        }

        if (BtnTP_Canvas != null)
        {
            BtnTP_Canvas.SetActive(false);
        }
    }

    public void Poe_TP()
    {
        if (TP_mitsubishi != null)
        {
            // Reposiciona o objeto TP_mitsubishi como na posicao inicial
            TP_mitsubishi.transform.position = posInicial;
            TP_mitsubishi.transform.rotation = rotInicial;
        }

        if (BtnTP_Canvas != null)
        {
            BtnTP_Canvas.SetActive(true);
        }
    }
}