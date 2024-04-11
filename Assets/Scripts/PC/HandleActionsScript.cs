using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class HandleActionsScript : MonoBehaviour
{
    public GameObject pc;
    public List<string> ips = new List<string>();
    public List<Button> pingButtons = new List<Button>();
    public TMP_InputField input;

    private int page = 0;

    public void SetIp()
    {
        if (!pc) return;

        pc.GetComponent<PCConManager>().HandleCommand("set ip " + input.text);
        //pc.GetComponent<PCConManager>().SetIP(input.text);
    }

    //..var txt = btn.gameObject.transform.GetChild(0).gameObject;
    //..pc.GetComponent<PCConManager>().Ping(txt.transform.GetComponent<TextMeshProUGUI>().text);
    public void HandlePing(Button btn)
    {
        // Mensaje de delay
        pc.GetComponent<PCConManager>().HandleCommand("...");

        // Encontrar cables implicados en el ping y agregarles la animacion
        GrabInteractableCable[] cables = FindObjectsOfType<GrabInteractableCable>();

        var pc1 = transform.parent.GetChild(3).GetComponent<ScreenConManager>().PcAsociado;
        GameObject pc2 = null;
        PCConManager[] pcs = FindObjectsOfType<PCConManager>();
        foreach (PCConManager pc in pcs)
        {
            if (pc.IPAddress == btn.gameObject.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text)
            {
                pc2 = pc.gameObject;
            }
        }

        foreach (GrabInteractableCable cable in cables)
        {
            //Material nuevoMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/bola.mat");
            Material nuevoMaterial = Resources.Load<Material>("bola");
            CableComponent cableAfectado = null;
            var direccion = 1; // de cable end a start
            if ((cable.device == pc1 || cable.device == pc2) && cable.extremoCable.GetComponent<GrabInteractableCable>().device.tag == "Router")
            {
                if (cable.GetComponent<CableComponent>() != null)
                {
                    cableAfectado = cable.GetComponent<CableComponent>();
                    if(cable.device == pc1)
                    {
                        direccion = -1;
                    }
                    else
                    {
                        nuevoMaterial = Resources.Load<Material>("bola2");
                    }
                }
                else
                {
                    cableAfectado = cable.extremoCable.GetComponent<CableComponent>();
                    if (cable.device == pc2)
                    {
                        nuevoMaterial = Resources.Load<Material>("bola2");
                        direccion = -1;
                    }
                }
                cableAfectado.cableMaterial = nuevoMaterial;
                cableAfectado.InitLineRenderer();
                if (cableAfectado.GetComponent<TextureScrollScript>() == null)
                {
                    cableAfectado.gameObject.AddComponent<TextureScrollScript>();
                    cableAfectado.GetComponent<TextureScrollScript>().SetDirection(direccion);
                }
            }
        }

        StartCoroutine(DelayedPing(btn));
    }

    private IEnumerator DelayedPing(Button btn)
    {
        // Espera 5 segundos antes de continuar con la ejecuci�n
        yield return new WaitForSeconds(10f);

        var txt = btn.gameObject.transform.GetChild(0).gameObject;

        Debug.Log("ping tardio");

        // Asignar a todos los cables el material del calbe, recargar cable y quitar animacion
        var cables = GameObject.FindGameObjectsWithTag("Cable-Eth").Select(objeto => objeto.GetComponent<CableComponent>()).Where(cable => cable != null).ToList();
        //Material nuevoMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/MaterialsE/CableMaterial.mat");
        Material nuevoMaterial = Resources.Load<Material>("CableMaterial");
        foreach (CableComponent cable in cables)
        {
                cable.cableMaterial = nuevoMaterial;
                cable.InitLineRenderer();
                var scriptScroll = cable.GetComponent<TextureScrollScript>();
                if (scriptScroll != null)
                {
                    Destroy(scriptScroll);
                }
        }

        pc.GetComponent<PCConManager>().HandleCommand("ping " + txt.transform.GetComponent<TextMeshProUGUI>().text);
    }

    public void Start()
    {
        var menu = gameObject.transform.GetChild(0).gameObject;
        var pingMenu = menu.transform.GetChild(4).gameObject;
        var panel = pingMenu.transform.GetChild(0).gameObject;

        pingButtons.Add(panel.transform.GetChild(2).GetComponent<Button>());
        pingButtons.Add(panel.transform.GetChild(3).GetComponent<Button>());
        pingButtons.Add(panel.transform.GetChild(4).GetComponent<Button>());
        //pingButtons.Add(GameObject.Find("ConsoleActions/Menu Comandos/PING-Menu/Panel/Ping Button").GetComponent<Button>());
        //pingButtons.Add(GameObject.Find("ConsoleActions/Menu Comandos/PING-Menu/Panel/Ping Button2").GetComponent<Button>());
        //pingButtons.Add(GameObject.Find("ConsoleActions/Menu Comandos/PING-Menu/Panel/Ping Button3").GetComponent<Button>());

        pingButtons[0].onClick.AddListener(delegate { HandlePing(pingButtons[0]); });
        pingButtons[1].onClick.AddListener(delegate { HandlePing(pingButtons[1]); });
        pingButtons[2].onClick.AddListener(delegate { HandlePing(pingButtons[2]); });
    }

    public void GetIps()
    {
        var pcs = GameObject.FindGameObjectsWithTag("PC");
        

        ips.Clear();
        for (int i = 0; i < pcs.Length; i++)
        {
            ips.Add(pcs[i].GetComponent<PCConManager>().IPAddress);
        }
        ips.Add("192.168.1.100");
        setIpButtons();
    }

    private void setIpButtons()
    {
        foreach(var btn in pingButtons)
        {
            btn.gameObject.SetActive(false);
        }

        for (int i = (page * 3), j = 0; i < ips.Count && i < (page * 3) + 3; i++, j++)
        {
            pingButtons[j].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ips[i]; //
            pingButtons[j].gameObject.SetActive(true);
        }
    }

    public void nextPage()
    {
        if ((page+1)*3 >= ips.Count) return;
        page++;

        setIpButtons();
    }

    public void previousPage()
    {
        if (page - 1 < 0) return;
        page--;

        setIpButtons();
    }
}
