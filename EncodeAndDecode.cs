using System;
using System.Text;

public class Program
{
	const int mod = 27;
	public static void Main()
	{
		Console.WriteLine("Type a message: ");
		string message = Console.ReadLine();
		Encode(message);
		Console.WriteLine("Encoded Message: ");
		string encodedMessage = Console.ReadLine();
		Console.WriteLine("Parity: ");
		int parity = int.Parse(Console.ReadLine());
		Console.WriteLine("Check Digit: ");
		int checkDigit = int.Parse(Console.ReadLine());
		Decode(encodedMessage, parity, checkDigit);
	}

	public static void Encode(string message)
	{
		byte[] asciiEncodeMsg = Encoding.ASCII.GetBytes(message);
		string encodedMessage = GetByteString(asciiEncodeMsg);
		string binaryEncodedMsg = ToByteStringBinary(asciiEncodeMsg);
		string hexEncodedMsg = ToByteStringHex(asciiEncodeMsg);
		int checkDigit = GetCheckDigit(asciiEncodeMsg);
		Console.WriteLine("Encoded Hex: " + hexEncodedMsg);
		Console.WriteLine("Binary: " + binaryEncodedMsg);
		Console.WriteLine("Number of 1s: " + NumberOfOnes(binaryEncodedMsg));
		Console.WriteLine("Check Digit: " + checkDigit);
	}

	public static void Decode(string input, int parity, int checkDigit)
	{
		string toStringInput = hexToString(input);
		byte[] result = stringToByte(toStringInput);
		if (!checkParity(result, parity) && !CheckDigitValid(result, checkDigit))
		{
			Console.WriteLine("Validation Check Error 001: Parity and Check Digit failed, Parity digit and Check digit should not be " + parity.ToString() + " and " + checkDigit.ToString() + ".");
			return;
		}
		else if (!checkParity(result, parity))
		{
			Console.WriteLine("Validation Check Error 002: Parity Check failed, Parity digit should not be " + parity.ToString() + ".");
			return;
		}
		else if (!CheckDigitValid(result, checkDigit))
		{
			Console.WriteLine("Validation Check Error 003: Check Digit Check failed, Check digit should not be " + checkDigit.ToString() + ".");
			return;
		}
		else
		{
			Console.WriteLine("Decoded Message: " + asciiByteToString(result));
			return;
		}
	}

	static string GetByteString(byte[] b)
	{
		return string.Join(" ", b);
	}

	static string ToByteStringBinary(byte[] b)
	{
		string result = "";
		foreach (byte a in b)
		{
			result += Convert.ToString(Convert.ToInt32(a.ToString()), 2);
		}

		return result;
	}

	static string ToByteStringHex(byte[] b)
	{
		string[] result = new string[b.Length];
		int index = 0;
		foreach (byte a in b)
		{
			result[index] = a.ToString("X");
			index++;
		}

		return string.Join(" ", result);
	}

	static int NumberOfOnes(string binaryCode)
	{
		return (binaryCode.Split('1').Length - 1);
	}

	static int ParityCheck(int noOfOnes)
	{
		bool isEven = (noOfOnes % 2) == 0;
		if (isEven)
			return 0;
		else
			return 1;
	}

	static string hexToString(string input)
	{
		string[] toStringArray = (input.Split(' '));
		string[] last = new string[toStringArray.Length];
		int index = 0;
		foreach (string s in toStringArray)
		{
			last[index] = Convert.ToInt32(s, 16).ToString();
			index++;
		}
		return string.Join(" ", last);
	}

	static byte[] stringToByte(string input)
	{
		string[] toStringArray = (input.Split(' '));
		byte[] result = new byte[toStringArray.Length];
		int index = 0;
		foreach (string s in toStringArray)
		{
			int a;
			int.TryParse(s, out a);
			result.SetValue((byte)a, index);
			index++;
		}

		return result;
	}

	static bool checkParity(byte[] input, int parity)
	{
		string byteString = ToByteStringBinary(input);
		byteString += parity.ToString();
		return ((NumberOfOnes(byteString) % 2) == 0);
	}

	static string asciiByteToString(byte[] input)
	{
		return Encoding.Default.GetString(input);
	}

	static int GetCheckDigit(byte[] input)
	{
		int index = 1;
		int sum = 0;
		foreach (byte b in input)
		{
			int bNum = 0;
			int.TryParse(b.ToString(), out bNum);
			sum += bNum * index;
			index++;
		}

		int result = mod - (sum % mod);
		return result;
	}

	static bool CheckDigitValid(byte[] input, int checkDigit)
	{
		int index = 1;
		int sum = 0;
		foreach (byte b in input)
		{
			int bNum = 0;
			int.TryParse(b.ToString(), out bNum);
			sum += bNum * index;
			index++;
		}

		sum += checkDigit;
		return ((sum % mod) == 0);
	}
}