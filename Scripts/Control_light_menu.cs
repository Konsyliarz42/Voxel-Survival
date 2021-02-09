using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_light_menu : MonoBehaviour
{
    const float color_per_hour = 1583.3333f;
    const float angles_per_hour = 30.0f;

    //----------------

    Light directionalLight;
    public GameObject lightObject;
    public GameObject centerObject;

    //----------------

    float color_temperature;
    float color_time;

    float z_angles;

    //================================================================

    void Start()
    {
        directionalLight = lightObject.GetComponent<Light>();
        directionalLight.colorTemperature = 1000;
        
        ChangeTemperature();
        SetPositionLight();
    }

    //================================================================

    void ChangeTemperature()
    {
        System.DateTime realTime = System.DateTime.Now;
        
        color_time += (float)realTime.Hour + ((float)realTime.Minute / 100);

        if(realTime.Hour < 12)  directionalLight.colorTemperature = 20000 - (color_time * color_per_hour);
        else                    directionalLight.colorTemperature = 1000 + ((color_time - 12.0f) * color_per_hour);
    }

    void SetPositionLight()
    {
        System.DateTime realTime = System.DateTime.Now;
        if(realTime.Hour >= 12) z_angles -= 12.0f;

        z_angles += (float)realTime.Hour + ((float)realTime.Minute / 100);
        z_angles *= angles_per_hour;

        gameObject.transform.Rotate(0, 0, z_angles + 90.0f, Space.Self);
        lightObject.transform.LookAt(centerObject.transform.position);
    }
}
