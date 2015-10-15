using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProcessManager : MonoBehaviour
{
	//all the queue's to hold the process
	private Queue<Process> newQueue = new Queue<Process>();
	private Queue<Process> readyQueue = new Queue<Process>();
	private Queue<Process> waitingQueue = new Queue<Process>();
	private Queue<Process> inProcess = new Queue<Process>();
	private Queue<Process> done = new Queue<Process>();
	private Queue<Process> inIO = new Queue<Process>();
	private List<Process> everything = new List<Process>();

	//temp process holder
	private Process holder;

	//texts for the different lists to print
	private Text newText;
	private Text readyText;
	private Text processText;
	private Text doneText;
	private Text waitingText;
	private Text inIOText;

    //text for the PCB sadly I don't think I can put it on another file since I need to be able to update the
    //processes as they go
    private Text idText;
    private Text arrivedText;
    private Text CPUuseText;
    private Text acumulatedText;
    private Text scheduleIOText;
    private Text ioDurationText;
    private Text endTimeText;
    private Text systemTimeText;
    private Text waitingTimeText;

	//control variables
	private bool update = false;
	private bool canMove = true;

	//limits of the different lists
	private int newLimit;
	private int readyLimit;
	private int waitingLimit;
    private int actualTime = 0;

	// Use this for initialization
	void Start ()
	{
		//initializes all the texts that will be printed, and assigns them to their text objects
		newText = GameObject.Find("NewList").GetComponent<Text>();
		newText.text = "";

		readyText = GameObject.Find("ReadyList").GetComponent<Text>();
		readyText.text = "";

		processText = GameObject.Find("ProcessList").GetComponent<Text>();
		processText.text = "";

		doneText = GameObject.Find("DoneList").GetComponent<Text>();
		doneText.text = "";

		waitingText = GameObject.Find("WaitingIOList").GetComponent<Text>();
		waitingText.text = "";

		inIOText = GameObject.Find("UsingIOList").GetComponent<Text>();
		inIOText.text = "";

        //after this it's the pcb texts
        idText = GameObject.Find("IDInfoText").GetComponent<Text>();
        idText.text = "";

        arrivedText = GameObject.Find("ArriveInfoText").GetComponent<Text>();
        arrivedText.text = "";

        CPUuseText = GameObject.Find("CPUuseInfoText").GetComponent<Text>();
        CPUuseText.text = "";

        acumulatedText = GameObject.Find("AcumulatedInfoText").GetComponent<Text>();
        acumulatedText.text = "";

        scheduleIOText = GameObject.Find("ScheduleIOText").GetComponent<Text>();
        scheduleIOText.text = "";

        ioDurationText = GameObject.Find("IODurationInfoText").GetComponent<Text>();
        ioDurationText.text = "";

        endTimeText = GameObject.Find("EndTimeInfoText").GetComponent<Text>();
        endTimeText.text = "";

        systemTimeText = GameObject.Find("SystemTimeInfoText").GetComponent<Text>();
        systemTimeText.text = "";

        waitingTimeText = GameObject.Find("WaitingTimeText").GetComponent<Text>();
        waitingTimeText.text = "";

	}

	// Update is called once per frame
	void Update ()
	{
		if(update)
		{
			newText.text = "";
			printQueue(newQueue,newText);
			readyText.text = "";
			printQueue(readyQueue,readyText);
			processText.text = "";
			printQueue(inProcess,processText);
			doneText.text = "";
			printQueue(done,doneText);
			waitingText.text = "";
			printQueue(waitingQueue,waitingText);
			inIOText.text = "";
			printQueue(inIO,inIOText);
            //this will do the same thing as ^ there, but for all the pcb stuff
            resetPCB();
            printPCB();
			update = false;
		}
	}

	public bool isInProcess()
	{
		if(inProcess.Count != 0){return true;}
		return false;
	}

	public void quantumTick(int seconds)
	{
		if(!(readyQueue.Count == 0))
			canMove = false;
		readyQueue.Enqueue(inProcess.Dequeue());
		tick(seconds);
        GameObject.Find("Tick&Quantum").GetComponent<TextBox>().startQuantum();
	}

    private void checkIO()
    {
        if(inIO.Count != 0)
		{
			if(inIO.Peek().ioDone())
			{
				if(readyQueue.Count == 0)
					canMove = false;
                if(isReadyFull())
                {
                    inProcess.Peek().error();
                    done.Enqueue(inIO.Dequeue());
                }
                else
                {
				    readyQueue.Enqueue(inIO.Dequeue());
                }
			}
			else
			{
				inIO.Peek().ioPass();
			}
		}
    }

    private void checkWaiting()
    {
        if(waitingQueue.Count != 0)
		{
			if(waitingQueue.Peek().waitedEnough())
			{
				if(inIO.Count == 0)
				{
					inIO.Enqueue(waitingQueue.Dequeue());
				}
			}
			else
			{
				waitingQueue.Peek().waitPass();
			}
		}
    }

    private void checkProcess(int seconds)
    {
        if(inProcess.Count != 0)
		{
			if(inProcess.Peek().isFinished())
			{
                inProcess.Peek().endTime = seconds;
				done.Enqueue(inProcess.Dequeue());
			}
			else
			{
				if(inProcess.Peek().ioReady())
				{
					if(!isWaitingFull())
						waitingQueue.Enqueue(inProcess.Dequeue());
					else
					{
						inProcess.Peek().error();
                        inProcess.Peek().endTime = seconds;
						done.Enqueue(inProcess.Dequeue());
					}
				}
				else
                {
					inProcess.Peek().timePass();
                }
			}
		}
    }

    private void checkReady()
    {
        if(readyQueue.Count != 0 && canMove)
		{
			if(inProcess.Count == 0)
			{
				inProcess.Enqueue(readyQueue.Dequeue());
			}
		}
    }

    private void checkNew()
    {
        if(newQueue.Count != 0)
		{
			if(!isReadyFull())
				readyQueue.Enqueue(newQueue.Dequeue());
		}
    }

	public void tick(int seconds)
	{
        checkIO();
		checkWaiting();
		checkProcess(seconds);
        checkReady();
        checkNew();
		canMove = true;
        actualTime = seconds;
		update = true;
	}

	public void createNewProcess(int duration, int n, int ioTimer, int waitingTime, int arrivalTime)
	{
		if(!isNewFull())
		{
			//here is where you send each process to the pcb.
			holder = new Process("P" + n, duration, ioTimer,waitingTime,arrivalTime);
			everything.Add(holder);
			newQueue.Enqueue(holder);
		}
	}

    private void resetPCB()
    {
        idText.text = "";
        arrivedText.text = "";
        CPUuseText.text = "";
        acumulatedText.text = "";
        scheduleIOText.text = "";
        ioDurationText.text = "";
        endTimeText.text = "";
        systemTimeText.text = "";
        waitingTimeText.text = "";
    }

    private void printPCB()
    {
        if(everything.Count == 0)
        {
            idText.text = "";
            arrivedText.text = "";
            CPUuseText.text = "";
            acumulatedText.text = "";
            scheduleIOText.text = "";
            ioDurationText.text = "";
            systemTimeText.text = "";
            waitingTimeText.text = "";
        }

        foreach(Process iterator in everything)
        {
             print("im doing stuff");
            idText.text = idText.text + iterator.print() + "\n";
            arrivedText.text = arrivedText.text + iterator.arrivalTime + "\n";
            CPUuseText.text = CPUuseText.text + iterator.staticDuration + "\n";
            acumulatedText.text = acumulatedText.text + (iterator.durationCounter) + "\n";
            scheduleIOText.text = scheduleIOText.text + iterator.staticWaitingTime + "\n";
            ioDurationText.text = ioDurationText.text + iterator.ioTimer + "\n";
            endTimeText.text = endTimeText.text + iterator.endTime + "\n";

            if(iterator.isFinished())
            {
                systemTimeText.text = systemTimeText.text + iterator.TotalTime() + "\n";
                waitingTimeText.text = waitingText.text + (iterator.TotalTime() - (iterator.ioCounter + iterator.durationCounter)) + "\n";
            }
            else
            {
                systemTimeText.text = systemTimeText.text + (actualTime - iterator.arrivalTime) + "\n";
                waitingTimeText.text = waitingTimeText.text + ((actualTime - iterator.arrivalTime) - (iterator.ioCounter + iterator.durationCounter)) + "\n";
            }

        }
    }

	private void printQueue(Queue<Process> stuff, Text thing)
	{
		if(stuff.Count == 0)
			thing.text = "";
		foreach( Process iterator in stuff )
        {
            thing.text = thing.text + iterator.print() + "\n";
        }
	}

	private bool isNewFull()
	{
		if(newLimit == 0){return false;}
		return newQueue.Count >= newLimit;
	}

	private bool isWaitingFull()
	{
		if(waitingLimit == 0){return false;}
		return waitingQueue.Count >= waitingLimit;
	}

	private bool isReadyFull()
	{
		if(readyLimit == 0){return false;}
		return readyQueue.Count >= readyLimit;
	}

	public void setNewLimit(string limitString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(limitString, out newLimit);
		if(!parseSuccess)
		{
			newLimit = 0;
		}
	}

	public void setWaitingLimit(string limitString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(limitString, out waitingLimit);
		if(!parseSuccess)
		{
			waitingLimit = 0;
		}
	}

	public void setReadyLimit(string limitString)
	{
		bool parseSuccess;
		parseSuccess = Int32.TryParse(limitString, out readyLimit);
		if(!parseSuccess)
		{
			readyLimit = 0;
		}
	}

    public void thetaProtocol()
    {
        everything.Clear();
        newQueue.Clear();
        readyQueue.Clear();
        waitingQueue.Clear();
        inProcess.Clear();
        done.Clear();
        inIO.Clear();
        update = true;
    }
}
