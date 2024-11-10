using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class VisualizacaoController : MonoBehaviour
{
    [SerializeField] private Button btnInicioVisualizacao;
    [SerializeField] private JSONManipulation JM;
    [SerializeField] private GameObject TP_mitsubishi;
    [SerializeField] private Transform posTPguardado;
    [SerializeField] private GameObject CurvedCanvas;
    [SerializeField] private GameObject CanvasPanel;
    [SerializeField] private Button BtnVoltar;

    private void Start()
    {
        IniciarVisualizacao();
    }

    private void IniciarVisualizacao()
    {
        // Reposiciona o objeto TP_mitsubishi
        TP_mitsubishi.transform.position = posTPguardado.position;
        TP_mitsubishi.transform.rotation = posTPguardado.rotation;
        CurvedCanvas.SetActive(false);
        CanvasPanel.SetActive(true);
        
        // Adiciona o listener ao botão de saída
        BtnVoltar.onClick.AddListener(ReiniciarCena);
        // Chama a função PlayProgramFromJSON do script JSONManipulation
        StartCoroutine(WaitAndPlayProgram());
    }

    private IEnumerator WaitAndPlayProgram()
    {
        yield return new WaitForSeconds(3f);
        JM.PlayProgramFromJSON();
    }

    private void ReiniciarCena()
    {
        // Reinicia a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
