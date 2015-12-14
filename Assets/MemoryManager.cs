using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MemoryManager : MonoBehaviour
{
    private List<MemoryProcess> process = new List<MemoryProcess>();
    private int RAM = 32;
    private int pageSize = 4;
    private GameObject[,] memoryMap;
    private PageMatrix[,] memoryMapAttributes;
    private GameObject greenOne;
    private PageMatrix greenAttribute;
    private bool mode = false;

    //TAP stuff here
    //private Text tapTxt;
    //private GameObject tap;

	// Use this for initialization
	void Start ()
    {
        //tap = GameObject.Find("Pages");
        //tapTxt = GameObject.Find("PagesText").GetComponent<Text>();
        //tapTxt.text = "";
        memoryMapAttributes = new PageMatrix[8,8];
        initializeMap();
        memoryMap = new GameObject[8,8];
        memoryMap[0,0] = GameObject.Find("A1");
        memoryMap[0,1] = GameObject.Find("A2");
        memoryMap[0,2] = GameObject.Find("A3");
        memoryMap[0,3] = GameObject.Find("A4");
        memoryMap[0,4] = GameObject.Find("A5");
        memoryMap[0,5] = GameObject.Find("A6");
        memoryMap[0,6] = GameObject.Find("A7");
        memoryMap[0,7] = GameObject.Find("A8");
        memoryMap[1,0] = GameObject.Find("B1");
        memoryMap[1,1] = GameObject.Find("B2");
        memoryMap[1,2] = GameObject.Find("B3");
        memoryMap[1,3] = GameObject.Find("B4");
        memoryMap[1,4] = GameObject.Find("B5");
        memoryMap[1,5] = GameObject.Find("B6");
        memoryMap[1,6] = GameObject.Find("B7");
        memoryMap[1,7] = GameObject.Find("B8");
        memoryMap[2,0] = GameObject.Find("C1");
        memoryMap[2,1] = GameObject.Find("C2");
        memoryMap[2,2] = GameObject.Find("C3");
        memoryMap[2,3] = GameObject.Find("C4");
        memoryMap[2,4] = GameObject.Find("C5");
        memoryMap[2,5] = GameObject.Find("C6");
        memoryMap[2,6] = GameObject.Find("C7");
        memoryMap[2,7] = GameObject.Find("C8");
        memoryMap[3,0] = GameObject.Find("D1");
        memoryMap[3,1] = GameObject.Find("D2");
        memoryMap[3,2] = GameObject.Find("D3");
        memoryMap[3,3] = GameObject.Find("D4");
        memoryMap[3,4] = GameObject.Find("D5");
        memoryMap[3,5] = GameObject.Find("D6");
        memoryMap[3,6] = GameObject.Find("D7");
        memoryMap[3,7] = GameObject.Find("D8");
        memoryMap[4,0] = GameObject.Find("E1");
        memoryMap[4,1] = GameObject.Find("E2");
        memoryMap[4,2] = GameObject.Find("E3");
        memoryMap[4,3] = GameObject.Find("E4");
        memoryMap[4,4] = GameObject.Find("E5");
        memoryMap[4,5] = GameObject.Find("E6");
        memoryMap[4,6] = GameObject.Find("E7");
        memoryMap[4,7] = GameObject.Find("E8");
        memoryMap[5,0] = GameObject.Find("F1");
        memoryMap[5,1] = GameObject.Find("F2");
        memoryMap[5,2] = GameObject.Find("F3");
        memoryMap[5,3] = GameObject.Find("F4");
        memoryMap[5,4] = GameObject.Find("F5");
        memoryMap[5,5] = GameObject.Find("F6");
        memoryMap[5,6] = GameObject.Find("F7");
        memoryMap[5,7] = GameObject.Find("F8");
        memoryMap[6,0] = GameObject.Find("G1");
        memoryMap[6,1] = GameObject.Find("G2");
        memoryMap[6,2] = GameObject.Find("G3");
        memoryMap[6,3] = GameObject.Find("G4");
        memoryMap[6,4] = GameObject.Find("G5");
        memoryMap[6,5] = GameObject.Find("G6");
        memoryMap[6,6] = GameObject.Find("G7");
        memoryMap[6,7] = GameObject.Find("G8");
        memoryMap[7,0] = GameObject.Find("H1");
        memoryMap[7,1] = GameObject.Find("H2");
        memoryMap[7,2] = GameObject.Find("H3");
        memoryMap[7,3] = GameObject.Find("H4");
        memoryMap[7,4] = GameObject.Find("H5");
        memoryMap[7,5] = GameObject.Find("H6");
        memoryMap[7,6] = GameObject.Find("H7");
        memoryMap[7,7] = GameObject.Find("H8");
        distributeRAM();
	}

	// Update is called once per frame
	void Update ()
    {

	}

    private void initializeMap()
    {
        for(int i = 0; i < 8; i++)
            for(int j = 0; j < 8; j++)
                memoryMapAttributes[i,j] = new PageMatrix ();
    }

    public void createNewProcess(int totalProcess, int memorySize)
    {
        MemoryProcess temp = new MemoryProcess("P" + totalProcess, memorySize, (memorySize / pageSize));
        for(int i = 0; i < temp.pageNumber; i++)
        {
            temp.pages[i] = "dsk";
        }
        process.Add(temp);
    }

    public void tick()
    {
        //print("this goes next");
        //this should add timeinRam to the activated page
        foreach (PageMatrix page in memoryMapAttributes)
        {
            page.timeInSystem++;
        }
        if(greenAttribute != null)
        {
            greenAttribute.timeRunning++;
        }
    //    resetTAP();
    //    printTAP();
    }

    /*private void resetTAP()
    {
        tapTxt.text = "";
    }

    private void printTAP()
    {
        foreach(MemoryProcess iterator in process)
        {
            tapTxt.text = tapTxt.text + "name: " + iterator.name + "   size: " + iterator.size;
            for(int i = 0; i < iterator.pageNumber; i++)
            {
                tapTxt.text = tapTxt.text + "   pg" + i + ": " + iterator.pages[i];
            }
            tapTxt.text = tapTxt + "\n";
        }
    }*/

    private MemoryProcess search(string name)
    {
        foreach(MemoryProcess iterator in process)
        {
            if(iterator.findName(name))
                return iterator;
        }
        return null;
    }

    public bool Activate(string name)
    {
        bool done,failed;
        done = true;
        failed = true;
        MemoryProcess temp = search(name);
        if(temp != null)
        {
            print(name);
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(!(memoryMapAttributes[i,j].osOccupied) && !(memoryMapAttributes[i,j].isOccupied) && (memoryMapAttributes[i,j].available))
                    {
                        memoryMapAttributes[i,j].processName = name;
                        memoryMapAttributes[i,j].isOccupied = true;
                        memoryMapAttributes[i,j].hasInfo = true;
                        memoryMap[i,j].transform.GetChild(0).GetComponent<Text>().text = name;
                        memoryMap[i,j].GetComponent<Button>().GetComponent<Image>().color = Color.yellow;
                        memoryMapAttributes[i,j].timeInSystem = 0;
                        memoryMapAttributes[i,j].timeRunning = 0;
                        memoryMapAttributes[i,j].process = temp;
                        memoryMapAttributes[i,j].process.pages[memoryMapAttributes[i,j].process.pageRunning] = "RAM";
                        done = false;
                        failed = false;
                        break;
                    }
                }
                if(!done)
                {
                    break;
                }
            }
            if(failed)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void setPage(int size)
    {
        pageSize = size;
        distributeRAM();
    }

    public void setSize(int size)
    {
        RAM = size;
        distributeRAM();
    }

    private void distributeRAM()
    {
        int totalPages = RAM/pageSize;
        totalPages = totalPages + 1;
        int osMemory = totalPages / 4;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(osMemory != 0)
                {
                    memoryMap[i,j].GetComponent<Button>().GetComponent<Image>().color = Color.blue;
                    osMemory--;
                    totalPages--;
                    memoryMapAttributes[i,j].osOccupied = true;
                    memoryMapAttributes[i,j].isOccupied = true;
                    memoryMapAttributes[i,j].hasInfo = true;
                    memoryMapAttributes[i,j].available = true;
                    memoryMapAttributes[i,j].processName = "OS";
                    memoryMap[i,j].transform.GetChild(0).GetComponent<Text>().text = "OS";
                }
                else
                {
                    if(totalPages != 0)
                    {
                        memoryMap[i,j].GetComponent<Button>().GetComponent<Image>().color = Color.white;
                        totalPages--;
                        memoryMapAttributes[i,j].osOccupied = false;
                        memoryMapAttributes[i,j].isOccupied = false;
                        memoryMapAttributes[i,j].available = true;
                        memoryMapAttributes[i,j].hasInfo = false;
                        memoryMapAttributes[i,j].processName = " ";
                        memoryMap[i,j].transform.GetChild(0).GetComponent<Text>().text = " ";
                    }
                    else
                    {
                        memoryMap[i,j].GetComponent<Button>().GetComponent<Image>().color = Color.gray;
                        memoryMapAttributes[i,j].osOccupied = false;
                        memoryMapAttributes[i,j].isOccupied = false;
                        memoryMapAttributes[i,j].available = false;
                        memoryMapAttributes[i,j].hasInfo = false;
                        memoryMapAttributes[i,j].processName = "N/A";
                        memoryMap[i,j].transform.GetChild(0).GetComponent<Text>().text = "N/A";
                    }
                }
            }
        }
        memoryMap[0,0].GetComponent<Button>().GetComponent<Image>().color = Color.blue;
        memoryMapAttributes[0,0].osOccupied = true;
        memoryMapAttributes[0,0].isOccupied = true;
        memoryMapAttributes[0,0].hasInfo = true;
        memoryMapAttributes[0,0].available = true;
    }

    public void swap(string name)
    {
        if(Activate(name))
        {
            return;
        }
        int a = -1, b = -1;
        int time = 0;
        int used = 9999;
        if(mode)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if((!memoryMapAttributes[i,j].osOccupied) && (memoryMapAttributes[i,j].available) && memoryMapAttributes[i,j].isOccupied && (!memoryMapAttributes[i,j].isRunning))
                    {
                        if(memoryMapAttributes[i,j].timeRunning < used)
                        {
                            a = i;
                            b = j;
                            used = memoryMapAttributes[i,j].timeRunning;
                        }
                    }
                }
            }
        }
        else
        {
            //the oldest time in System
            //if i want to use the least used, i would do the same except use time as the minimum and instead of using time in system id use time running
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if((!memoryMapAttributes[i,j].osOccupied) && (memoryMapAttributes[i,j].available) && memoryMapAttributes[i,j].isOccupied && (!memoryMapAttributes[i,j].isRunning))
                    {
                        if(memoryMapAttributes[i,j].timeInSystem > time)
                        {
                            a = i;
                            b = j;
                            time = memoryMapAttributes[i,j].timeInSystem;
                        }
                    }
                }
            }
        }
        if(a != -1 && b != -1)
        {
            memoryMapAttributes[a,b].timeInSystem = 0;
            memoryMapAttributes[a,b].processName = name;
            memoryMapAttributes[a,b].timeRunning = 0;
            memoryMapAttributes[a,b].process.pages[memoryMapAttributes[a,b].process.pageRunning] = "dsk";
            memoryMap[a,b].transform.GetChild(0).GetComponent<Text>().text = name;
            memoryMapAttributes[a,b].process = search(name);
            memoryMapAttributes[a,b].process.pages[memoryMapAttributes[a,b].process.pageRunning] = "RAM";
        }
    }

    public bool isActive(string name)
    {
        if(search(name) != null)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(memoryMapAttributes[i,j].processName.Equals(name))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        return false;
    }

    public void run(string name)
    {
        if(search(name) != null)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(memoryMapAttributes[i,j].processName.Equals(name))
                    {
                        if(greenOne != null)
                        {
                            greenOne.GetComponent<Button>().GetComponent<Image>().color = Color.yellow;
                            greenAttribute.isRunning = false;
                        }
                        greenOne = memoryMap[i,j];
                        greenAttribute = memoryMapAttributes[i,j];
                        greenAttribute.isRunning = true;
                        memoryMap[i,j].GetComponent<Button>().GetComponent<Image>().color = Color.green;
                        memoryMapAttributes[i,j].osOccupied = false;
                        memoryMapAttributes[i,j].isOccupied = true;
                        memoryMapAttributes[i,j].available = true;
                        memoryMapAttributes[i,j].hasInfo = true;
                    }
                }
            }
        }
    }

    public void changeMode(bool val)
    {
        mode = val;
    }

    public void finished(string name)
    {
        MemoryProcess temp = search(name);
        if(temp != null)
        {
            temp.name = temp.name + "done";
            greenOne.GetComponent<Button>().GetComponent<Image>().color = Color.cyan;
            greenAttribute.isOccupied = false;
            greenAttribute.available = true;
            greenOne = null;
            greenAttribute = null;
        }

    }

    public void preSwap(string name)
    {
        int randomNumber;
        MemoryProcess temp = search(name);
        if(temp != null)
        {
            greenOne.GetComponent<Button>().GetComponent<Image>().color = Color.cyan;
            greenAttribute.isOccupied = false;
            greenAttribute.available = true;
            randomNumber = ((Func<int>)(() => {
                    System.Random rnd = new System.Random();
                    int dice = rnd.Next(0,greenAttribute.process.pageNumber - 1);
                    return dice;
                }))();
            greenAttribute.process.pageRunning = randomNumber;
            greenOne = null;
            greenAttribute = null;
        }
    }

    public void thetaProtocol()
    {
        distributeRAM();
    }

}
