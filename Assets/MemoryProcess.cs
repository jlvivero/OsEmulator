using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryProcess
{
    private string name {get;set;}
    private int size {get;set;}
    private int pageNumber {get; set;}

    public MemoryProcess()
    {
        name = "P";
        size = 24;
        pageNumber = 1;
    }

    public MemoryProcess(string name, int size)
    {
        this.name = name;
        this.size = size;
        pageNumber = 1;
    }


    public bool findName(string name)
    {
        return this.name.Equals(name);
    }
}
