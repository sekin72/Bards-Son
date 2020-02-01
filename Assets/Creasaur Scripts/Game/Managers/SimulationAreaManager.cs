using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimulationAreaManager : MonoBehaviour
{

    public Vector3 EntryPos { get; set; }
    public Vector3 ExitPos { get; private set; }

    private void Awake()
    {
        LoadEntryAndExitPoints();
        /*LoadSpecialPoints();*/
    }

    private void LoadEntryAndExitPoints()
    {
        var _pointList = Resources.LoadAll("Points/Queues");
        var obj = new GameObject();
        var entryPoint = _pointList.FirstOrDefault(x => x.name.StartsWith("Entry"));
        obj = GameObject.Instantiate(entryPoint) as GameObject;
        EntryPos = obj.GetComponent<Transform>().position;
        ExitPos = obj.GetComponent<Transform>().position;
    }
    //private void LoadSpecialPoints()
    //{

    //}
}
