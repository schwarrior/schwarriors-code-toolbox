<Query Kind="Program" />

void Main()
{
	ulong primeSeed = int.MaxValue;
	for(var iteration = 1; iteration <= 100; iteration++)
	{
		var checkStopWatch = new Stopwatch();
		var eventName = "Find prime past " + primeSeed.ToString();
		using(new ExecStopWatch(eventName))
		{
			checkStopWatch.Start();
			primeSeed = MathUtilities.GetNextPrime(primeSeed);
			checkStopWatch.Stop();
		}
		string.Format(ExecStopWatch.DebugOutputStringFormat + " (check)", eventName, checkStopWatch.ElapsedMilliseconds).Dump();
		primeSeed.Dump();
	}
}

/// <summary>
/// Usage:
/// using(new ExecStopWatch("Make Widget")){ MakeWidget();}
/// outputs a debug writeline with the time spent inside the using block 
/// </summary>
/// <remarks>
/// this may not prove to be as accurate as just starting and stopping a stopwatch before and after work. 
/// a class may be flagged for disposal before its actually disposed
/// </remarks>
public class ExecStopWatch : Stopwatch, IDisposable
{
	private string EventName { get; set; }
	private bool Enabled { get; set; }
	private bool Disposed = false;
	public const string DebugOutputStringFormat = "{0}: {1:#,##0} ms";
	public ExecStopWatch(string eventName, bool enabled = true)
	{
		EventName = eventName;
		Enabled = enabled;
		Start();
	}
	
	private void PrintReport()
	{
		if (!Enabled) return;
		Stop();
		//Debug.WriteLine(DebugOutputStringFormat, EventName, ElapsedMilliseconds);
		string.Format(DebugOutputStringFormat, EventName, ElapsedMilliseconds).Dump();
	}
	public void Dispose()
	{
		if(!Disposed) PrintReport();
		Disposed = true;
	}
}

public class MathUtilities
{
	public static ulong GetNextPrime(ulong startingNumber)
	{
		var currentNumber = startingNumber;
		while(true)
		{
			currentNumber ++;
			if(currentNumber==1) return currentNumber;
			if(currentNumber%2==0) continue;
			var isPrime = true;
			for(ulong divisor = 3;divisor < currentNumber; divisor+=2)
			{
				if(currentNumber%divisor==0){isPrime = false; break;}
			}
			if(isPrime) return currentNumber;
		}
	}
}



