using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : MonoBehaviour
{
    [SerializeField] AudioSource triggerAudio;

    [SerializeField] Light[] controlledLights;
    [SerializeField] GameObject lightSwitch;
    bool lightsOff = true;

    void Start()
    {
        if(triggerAudio == null)
        {
            triggerAudio = gameObject.GetComponent<AudioSource>();
        }
    }

    public void UseLightSwitch()
    {
        if(!triggerAudio.isPlaying)
        {
            triggerAudio.Play();
        }
        //Turn lights on
        if(lightsOff)
        {
            lightsOff = false;
            /*
            //Rotate lightswitch game object to "on"
            lightSwitch.transform.eulerAngles = new Vector3(-60, 0, 0);
            */
            foreach(Light light in controlledLights)
            {
                light.enabled = true;
            }
        }
        //Turn lights off
        else
        {
            lightsOff = true;
            /*
            //Rotate lightswitch game object to "off"
            lightSwitch.transform.eulerAngles = new Vector3(-20, 0, 0);
            */
            foreach(Light light in controlledLights)
            {
                light.enabled = false;
            }
        }
    }
}
