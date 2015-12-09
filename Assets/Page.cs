using UnityEngine;
using System.Collections;

public class Page
{
    public string location {get;set;}
    public int pageNumber {get; private set;}
    public int timesUsed {get;set;}
    public int timeInRam {get;set;}
    public bool isActivated {get;set;}

    public Page()
    {
        location = "N/A";
        timesUsed = 0;
        timeInRam = 0;
    }

    public Page(string location, int pageNumber, int timesUsed, int timeInRam)
    {
        this.location = location;
        this.pageNumber = pageNumber;
        this.timesUsed = timesUsed;
        this.timeInRam = timeInRam;
    }
}
