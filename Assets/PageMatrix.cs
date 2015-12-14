using UnityEngine;
using System.Collections;

public class PageMatrix
{
    public bool osOccupied {get; set;}
    public bool isOccupied {get; set;}
    public bool hasInfo {get; set;}
    public bool available {get; set;}
    public string processName {get;set;}
    public int timeInSystem {get;set;}
    public int timeRunning {get;set;}
    public bool isRunning {get;set;}
    public MemoryProcess process {get;set;}

    public PageMatrix(){
        processName = "";
    }

    public PageMatrix(bool osOccupied, bool isOccupied, bool hasInfo, bool available)
    {
        this.osOccupied = osOccupied;
        this.isOccupied = isOccupied;
        this.hasInfo = hasInfo;
        this.available = available;
        processName = "";
    }
}
