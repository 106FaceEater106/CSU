using System;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			string symbols = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for (var ab = 0; ab < symbols.Length; ab++)
			{
				var character = symbols[ab];
				vm.RegisterCommand(character, b => { b.Memory[b.MemoryPointer] = Convert.ToByte(character); });
			}
			
			vm.RegisterCommand('.', b => write(Convert.ToChar(b.Memory[b.MemoryPointer])));
			vm.RegisterCommand('+', b =>
			{
				b.Memory[b.MemoryPointer] = Convert.ToByte((b.Memory[b.MemoryPointer] + 1) & 0xFF);
			});
			vm.RegisterCommand('-', b =>
			{
				b.Memory[b.MemoryPointer] = Convert.ToByte((b.Memory[b.MemoryPointer] + 255) & 0xFF);
			});
			vm.RegisterCommand('<', b => { b.MemoryPointer = (b.Memory.Length - 1 + b.MemoryPointer) % b.Memory.Length; });
			vm.RegisterCommand('>', b => { b.MemoryPointer = (b.MemoryPointer + 1) % b.Memory.Length; });
			vm.RegisterCommand(',', b => { b.Memory[b.MemoryPointer] = Convert.ToByte(read()); });
		}
	}
}