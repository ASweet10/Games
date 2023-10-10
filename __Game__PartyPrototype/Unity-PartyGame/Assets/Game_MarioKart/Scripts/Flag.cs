using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private GameObject flagObj;
    [SerializeField] private GameObject redDropoffPoint;
    [SerializeField] private GameObject blueDropoffPoint;
    private enum FlagType { Blue, Red };
    [SerializeField] private FlagType flagType;
    private void OnTriggerEnter(Collider other)
    {
        if(flagType == FlagType.Red)
        {
            if(other.tag == "Blue")
            {
                Debug.Log("Blue team has flag!");
                blueDropoffPoint.SetActive(true);
                other.tag = "BlueFlagCarrier";
                
                flagObj.SetActive(false);
                //Blue team has the flag announcement
                // Visual effect above car with flag / highlighted? can see him thru walls?
                // Highlight drop area for player with flag
            }
        }
        else if(flagType == FlagType.Blue)
        {
            if(other.tag == "Red")
            {
                Debug.Log("Red team has flag!");
                redDropoffPoint.SetActive(true);
                other.tag = "RedFlagCarrier";

                flagObj.SetActive(false);
                //Blue team has the flag announcement
                // Visual effect above car with flag / highlighted? can see him thru walls?
                // Highlight drop area for player with flag
            }
        }
    }
}
