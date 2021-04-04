<Query Kind="Program" />

void Main()
{
	var padEncrypt = new PadEncryption();
	var key = "Mastodons running rampage over the himalayas!";
	var sourceString = "Super Secret Stuff, Man!";
	var encryptedString = padEncrypt.Transform(true,key,sourceString);
	var decryptedString = padEncrypt.Transform(false,key,encryptedString);
	string.Format("Source String: \"{0}\". Encrypted String: \"{1}\". Check: \"{2}\". Key: \"{3}\"",
		sourceString, encryptedString, decryptedString, key).Dump();
}

// Define other methods and classes here

public class PadEncryption
{
    private const int FirstPrintableAsciiCode = ' '; //32
    private const int LastPrintableAsciiCode = '~'; //126;
    private const int CharOffset = LastPrintableAsciiCode - FirstPrintableAsciiCode + 1;
    private SortedDictionary<int, char> _charMap;
	private SortedDictionary<int, char> CharMap {
		get {

			if (_charMap == null) {
				_charMap = new SortedDictionary<int, char>();
				for (int charCode = FirstPrintableAsciiCode; charCode <= LastPrintableAsciiCode; charCode++) {
					_charMap.Add(charCode, (char)charCode);
                    _charMap.Add(charCode + CharOffset, (char)charCode);
                    _charMap.Add(charCode + CharOffset + CharOffset, (char)charCode);
                    _charMap.Add(charCode - CharOffset, (char)charCode);
                    _charMap.Add(charCode - CharOffset - CharOffset, (char)charCode);
				}
			}
			return _charMap;
		}
	}
	public string Transform(bool encrypt, string key, string source)
	{
		var destination = string.Empty;
	    //Make sure the key is the same size as the DecryptionBits
		while (key.Length < source.Length) {
			key += key;
		}
		key = key.Substring(0, source.Length);
		//encrypt each source character using the key 
		for (var charPosition = 0; charPosition <= key.Length - 1; charPosition++) {
            var keyCharCode = (int)key[charPosition];
            var sourceCharCode = (int)source[charPosition];
			if (keyCharCode < FirstPrintableAsciiCode | keyCharCode > LastPrintableAsciiCode) {
				throw new System.ApplicationException("Key string has characters outside printable ASCII range");
			}
			if (sourceCharCode < FirstPrintableAsciiCode | sourceCharCode > LastPrintableAsciiCode) {
				throw new System.ApplicationException("Source string has characters outside printable ASCII range");
			}
			int destinationCharPostion;
			if (encrypt) {
				destinationCharPostion = sourceCharCode + keyCharCode;
			} else {
				destinationCharPostion = sourceCharCode - keyCharCode;
			}
			destination += CharMap[destinationCharPostion];
		}
		return destination;
	}
}