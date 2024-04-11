using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScrollScript : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // Velocidad de desplazamiento de la textura
    public int direccion = 1;
    CableComponent cableScript;
    // Start is called before the first frame update
    void Start()
    {
        cableScript = gameObject.GetComponent<CableComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula el desplazamiento basado en el tiempo
        float offset = Time.time * scrollSpeed;

        // Crea un Vector2 con el desplazamiento en el eje x e y
        Vector2 offsetVector = new Vector2(offset * direccion, 0);

        // Aplica el desplazamiento a la textura del material
        cableScript.cableMaterial.SetTextureOffset("_MainTex", offsetVector);
    }
    public void SetDirection(int newDirection)
    {
        direccion = newDirection;
    }
}
