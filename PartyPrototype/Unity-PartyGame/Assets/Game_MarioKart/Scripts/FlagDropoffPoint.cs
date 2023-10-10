using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagDropoffPoint : MonoBehaviour
{
    [SerializeField] private GameObject redFlagObj;
    [SerializeField] private GameObject blueFlagObj;

    private enum DropoffType { Blue, Red };
    [SerializeField] private DropoffType dropoffType;

    private void OnTriggerEnter(Collider other)
    {
        if(dropoffType == DropoffType.Red) {
            if(other.tag == "RedFlagCarrier") {
                //Reset carrying flag UI / status
                blueFlagObj.SetActive(true);
                other.tag = "Red";
            }
        }
        else if(dropoffType == DropoffType.Blue)
        {
            if(other.tag == "BlueFlagCarrier")
            {
                //Reset  carrying flag UI / status
                redFlagObj.SetActive(true);
                other.tag = "Blue";
            }
        }
    }
}
