using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    public List<GameObject> floors;
    public List<GameObject> ghostLights;
    [HideInInspector] public int currentFloor;
    // Start is called before the first frame update
    void Start()
    {
        currentFloor = floors.Count ;
        SetupFloors();
    }


    private void SetupFloors()
    {
        for(int i =0;i<floors.Count;i++)
        {
           floors[i].gameObject.SetActive(i< currentFloor);
            ghostLights[i].gameObject.SetActive(i==currentFloor);   
        }
    }

    public void NextFloor()
    {
        currentFloor = currentFloor + 1;
        if (currentFloor > floors.Count)
            currentFloor = floors.Count;
        SetupFloors();
    }

    public void PrevFloor()
    {
        currentFloor = currentFloor - 1;
        if (currentFloor < 0)
            currentFloor = 0;
        SetupFloors();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
