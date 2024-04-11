using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // Velocidad de desplazamiento de la textura
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula el desplazamiento basado en el tiempo
        float offset = Time.time * scrollSpeed;

        // Crea un Vector2 con el desplazamiento en el eje x e y
        Vector2 offsetVector = new Vector2(offset, 0);

        // Aplica el desplazamiento a la textura del material
        rend.material.SetTextureOffset("_MainTex", offsetVector);
    }
}
