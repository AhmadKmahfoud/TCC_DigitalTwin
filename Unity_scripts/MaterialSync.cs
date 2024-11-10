using UnityEngine;

//Faz com que o objeto camaleao espelhe o material do objeto independente
public class MaterialSync : MonoBehaviour
{
    public GameObject objetoIndependente;
    public GameObject objetoCamaleao;

    private Material previousMaterial;

    private void Start()
    {
        // Inicializa o material anterior para o material atual do objetoIndependente
        previousMaterial = objetoIndependente.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        // Verifica se o material do objetoIndependente mudou
        Material currentMaterial = objetoIndependente.GetComponent<Renderer>().material;
        if (currentMaterial != previousMaterial)
        {
            // Atualiza o material do objetoCamaleao para corresponder ao novo material do objetoIndependente
            objetoCamaleao.GetComponent<Renderer>().material = currentMaterial;

            // Atualiza o material anterior para o novo material
            previousMaterial = currentMaterial;
        }
    }
}