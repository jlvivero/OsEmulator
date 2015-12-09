using UnityEngine;
using System.Collections;
using System;

public class TextBox : MonoBehaviour
{
	private string textBox = "0";
	private int seconds = 0;
	private int counter = 0;
	private int max = 100; // time it takes for the counter to start ticking
	private bool pause = true;
	private int percentage = 100;
	private int totalProcess = 1; //number of processes created
	private int quantum;
	private bool roundRobin = false;
	private int rrCounter = 0; // round robin counter for quantums
	private bool quantumReset = false;
	private int duration; // duration of each process , (ticks it needs to be running)
	private int ioTimer; //calculated time it uses the io for every new process


	public int positionx;
	public int positiony;
	public int length;
	public int width;

    //memory part
    private int memorySize = 32;
    private int pageSize = 4;
    private bool stopped = true;
    private GameObject manager;


	// Use this for initialization
	void Start ()
	{
        manager = GameObject.Find("manager");
		duration = 10;
		quantum = 10;
		ioTimer = 5;
		Screen.SetResolution(1280,768, false);
	}

/*************************************************************************************************************
    this does not have anything to do with this project this is just how you call anonymus functions that are
    void on C#
    private void randomMethod(Action something)
    {
        something();
        return;
    }
    randomMethod(()=>print("does this work"));
*************************************************************************************************************/
	void OnGUI()
	{
		textBox = GUI.TextField(new Rect(positionx,positiony,length,width), textBox);
	}

	// Update is called once per frame
	void Update ()
	{
		if(!pause)
		{
			if(counter > max)
			{
				counter = 0;
				seconds++;
				textBox = "" + seconds;
				if(roundRobin)
				{
                    if(quantumReset)
                    {
					    rrCounter++;
					    print(rrCounter);
                    }

					if(rrCounter > quantum)
					{
						manager.GetComponent<ProcessManager>().quantumTick(seconds);
					}
					else
					{
						manager.GetComponent<ProcessManager>().tick(seconds);
					}
				}
				else
				{
					manager.GetComponent<ProcessManager>().tick(seconds);
				}

				if(randomCreate(percentage))
				{
                    memorySize = ((Func<int>)(() => {
                            System.Random rnd = new System.Random();
                            int dice = rnd.Next(16,257);
                            return dice;
                        }))();
                    manager.GetComponent<ProcessManager>().createNewProcessm(totalProcess,memorySize);
					manager.GetComponent<ProcessManager>().createNewProcess(calculateDuration(duration),totalProcess,ioTimer,calculateWaitingTime(duration),seconds);
					totalProcess++;
				}
			}
			else
			{
				counter++;
			}
		}
	}


	private bool randomCreate(int percentage)
	{
		System.Random rnd = new System.Random();
		int dice = rnd.Next(1,101);
		return (dice <= percentage);
	}

	public void setMax(float num)
	{
		if(num == 0.0)
			max = 150;
		if(num == 1.0)
			max = 100;
		if(num == 2.0)
			max = 50;
	}

	public void setPause()
	{
		pause = !pause;
	}

	public int getCounter()
	{
		return counter;
	}

	public void setQuantum(string quantumString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(quantumString, out quantum);
		if(!parseSuccess)
		{
			quantum = 10;
			print(quantum);
		}
		if(quantum == 0)
			quantum = 1;
	}

	public void setPercentage(string percentageString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(percentageString, out percentage);
		if(!parseSuccess)
		{
			percentage = 50;
			print(percentage);
		}
	}

	public void changeType(bool type)
	{
		roundRobin = type;
	}

	public void startQuantum()
	{
        quantumReset = true;
		rrCounter = 0;
		print("quantum should start counting");
	}

	public void setDuration(string durationString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(durationString, out duration);
		if(!parseSuccess)
		{
			duration = 10;
		}
	}

	private int calculateDuration(int duration)
	{
		System.Random rnd = new System.Random();
		int dice = rnd.Next(calculateLower(duration),calculateUpper(duration));
		return dice;
	}

	private int calculateLower(int n)
	{
		int lower = (int)(n * .75);
		return lower;
	}

	private int calculateUpper(int n)
	{
		int upper = (int)(n * 1.25);
		return upper;
	}

	private int calculateWaitingTime(int n)
	{
		System.Random rnd = new System.Random();
		int dice = rnd.Next(1,calculateLower(n));
		return dice;
	}

	public void setIOTimer(string ioTimerString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(ioTimerString, out ioTimer);
		if(!parseSuccess)
		{
			ioTimer = 5;
		}
	}

    public void pressPlay()
    {
        pause = false;
        stopped = false;
    }

    public void pressPause()
    {
        pause = true;
    }

    public void pressStop()
    {
        thetaProtocol();
        print ("activate theta protocol");
    }

    private void thetaProtocol()
    {
        stopped = true;
        pause = true;
        seconds = 0;
        counter = 0;
        textBox = "0";
        totalProcess = 1;
        rrCounter = 0;
        quantumReset = false;
        manager.GetComponent<ProcessManager>().thetaProtocol();
    }

    public void setMemorySize(int n)
    {
        if(stopped)
        {
            if(!(n < pageSize))
            {
                memorySize = n;
                manager.GetComponent<MemoryManager>().setSize(n);
            }
        }
    }

    public void setPageSize(int n)
    {
        if(stopped)
        {
            if(!(n > memorySize))
            {
                manager.GetComponent<MemoryManager>().setPage(n);
                pageSize = n;
                print (pageSize);
            }
        }
    }
}
