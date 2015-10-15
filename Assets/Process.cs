using UnityEngine;
using System.Collections;

public class Process
{
	private string name;
	public int duration { get; private set;}
    public int staticDuration {get; private set;}
	public int ioTimer {get; private set;}
	private int waitingTime;
    public int staticWaitingTime {get; private set;}
    public int arrivalTime { get; private set; }
    public int acumulated {get; private set;}
    public int endTime {get; set;}
    public int durationCounter {get; private set;}
    public int ioCounter {get; private set;}

	public Process()
	{
		name = "P";
		duration = 10;
		ioTimer = 5;
		waitingTime = 2;
	}

	public Process(string givenName, int givenDuration, int givenTimer, int waitingForIO, int arrivalTime)
	{
		name = givenName;
		duration = givenDuration;
		ioTimer = givenTimer;
		waitingTime = waitingForIO;
        this.arrivalTime = arrivalTime;
        acumulated = 0;
        staticDuration = givenDuration;
        staticWaitingTime = waitingTime;
        endTime = 0;
        durationCounter = 0;
        ioCounter = 0;
	}

	public bool isFinished()
	{
		if(duration == 0)
			return true;
		return false;
	}

	public void timePass()
	{
		duration--;
        durationCounter++;
	}

	public string print()
	{
		return name;
	}

	public bool ioReady()
	{
		if(duration == ioTimer)
			return true;
		return false;
	}

	public bool waitedEnough()
	{
		if(waitingTime == 0)
			return true;
		return false;
	}

	public void waitPass()
	{
		waitingTime--;
	}

	public void ioPass()
	{
		ioTimer--;
        ioCounter++;
	}

	public bool ioDone()
	{
		if(ioTimer == 0)
			return true;
		return false;
	}

	public void error()
	{
		name = name + "error";
	}

    public int TotalTime()
    {
        return endTime - arrivalTime;
    }
}
