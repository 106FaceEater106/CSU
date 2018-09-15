using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		Dictionary<char, Action<IVirtualMachine>> directions;
		byte[] ramArray;
		string cipher;
		
		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			directions[symbol] = execute;
		}
		
		public VirtualMachine(string program, int memorySize)
		{
			ramArray = new byte[memorySize];
			cipher = program;
			directions = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public string Instructions { get => cipher; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get => ramArray; }
		public int MemoryPointer { get; set; }
		public void Run()
		{
			MemoryPointer = 0;
			for (InstructionPointer = 0;
				InstructionPointer < Instructions.Length;
				InstructionPointer = InstructionPointer + 1)
			{
				char symbol = Instructions[InstructionPointer];
				if (directions.ContainsKey(symbol))
					directions[symbol](this);
			}
		}
	}
}