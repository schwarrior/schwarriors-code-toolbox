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