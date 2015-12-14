using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryProcess
{
    public string name {get;set;}
    public int size {get;set;}
    public int pageNumber{get;set;}
    private int pagerunning;
    public int pageRunning
    {
        get{return pagerunning;}
        set
        {
            if(pagerunning == value)
            {
                if(pagerunning < this.pageNumber)
                {
                    pagerunning = value + 1;
                }
                else
                {
                    pagerunning = value - 1;
                }
            }
        }
    }
    public string[] pages{get;set;}

    public MemoryProcess()
    {
        name = "P";
        size = 24;
        pageNumber = 1;
    }

    public MemoryProcess(string name, int size, int pageNumber)
    {
        this.name = name;
        this.size = size;
        this.pageNumber = pageNumber;
        this.pages = new string[pageNumber];
        pagerunning = 0;
    }


    public bool findName(string name)
    {
        return this.name.Equals(name);
    }
}
