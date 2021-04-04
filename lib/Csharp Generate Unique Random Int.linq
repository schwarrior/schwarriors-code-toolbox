<Query Kind="Program" />

void Main()
{
	var ur = new UniqueRandomIntGenerator(1,3);
	int one, two, three, four;
	ur.GetNext(out one).Dump();
	one.Dump();
	ur.GetNext(out two).Dump();
	two.Dump();
	ur.GetNext(out three).Dump();
	three.Dump();
	ur.GetNext(out four).Dump();
	four.Dump();
	
}

public class UniqueRandomIntGenerator
    {
        private readonly Random _rnd = new Random();
        private readonly SortedSet<int> _previousValues = new SortedSet<int>();
        
        private readonly int _min;
        private readonly int _max;

        public UniqueRandomIntGenerator(int min, int max)
        {
            if(min > max) throw new ArgumentException("Min cannot be greater than max");
            _min = min;
            _max = max;
        }

        public bool GetNext(out int value)
        {
            value = 0;
            var rawValueRangeCount = _max - _min + 1;
            var previousValueCount = _previousValues.Count;
            if (previousValueCount >= rawValueRangeCount) return false;
            do { value = RandomInt(_min, _max); } 
            while (_previousValues.Contains(value));
			_previousValues.Add(value);
            return true;
        }

        private int RandomInt(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue + 1);
        }
    }
