using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRotation : MonoBehaviour
{
    public GameObject objectToRotate; // Objeto a ser girado (definido no Inspector)
    public bool ManualToggle; // Variável booleana para ativar/desativar

    private void Start(){
        ManualToggle = false; // Inicializa a variável como false
    }

    public void ManualToggleFunc(){
        if(ManualToggle){
            objectToRotate.transform.Rotate(Vector3.up * 90f);
            ManualToggle = !ManualToggle; // Inverter o valor da variável
        }else{
            objectToRotate.transform.Rotate(Vector3.down * 90f);
            ManualToggle = !ManualToggle; // Inverter o valor da variável
        }
    }
}
