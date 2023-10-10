using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] carMeshes;
    [SerializeField] private string[] carNames;
    [SerializeField] private GameObject carSpawnPoint;
    [SerializeField] private GameObject currentCar;
    [SerializeField] private Text carNameText;
    private int carIndex = 0;
    private bool canChangeCar = true;
    private GameObject nextCar;
    void Start()
    {
        currentCar = carMeshes[carIndex + 1];
    }

    public void ChangeDisplayedCar(string arrow)
    {
        currentCar.GetComponent<FlingCar>().LaunchCar();
        if(canChangeCar)
        {
            canChangeCar = false;
            if(arrow == "right") {
                if(carIndex == carMeshes.Length - 1)
                {
                    carIndex = 0;
                } else {
                    carIndex ++;
                }

            } else if (arrow == "left") {
                if(carIndex == 0)
                {
                    carIndex = carMeshes.Length - 1;
                } else {
                    carIndex --;
                }

            }

            nextCar = carMeshes[carIndex];
            currentCar = nextCar;

            Debug.Log(currentCar);

            Instantiate(currentCar, carSpawnPoint.transform.position, carSpawnPoint.transform.rotation * Quaternion.Euler(0, 270, 0));
            canChangeCar = true;
        }
    }
}
