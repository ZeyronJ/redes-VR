using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textTest : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        //textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = Time.deltaTime.ToString();
    }
}
