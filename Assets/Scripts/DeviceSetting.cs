using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeviceSetting : MonoBehaviour
{

    public Text DebugText;
    // Start is called before the first frame update
    void Start()
    {
        string device = SystemInfo.deviceModel;
        DebugText.text = " Debug Mode :" + device;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
