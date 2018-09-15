using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var pogoStick = new PogoStick(vm.Instructions);
			vm.RegisterCommand('[', b => pogoStick.OnBrace(b));
			vm.RegisterCommand(']', b => pogoStick.OnBrace(b));
		}
		
		class PogoStick
		{
			Dictionary<int, int> pogos;

			public PogoStick(string direct)
			{
				var openParent = new Stack<int>();
				pogos = new Dictionary<int, int>();
				var ab = -1;
				char[] patenthesis = { '[', ']' };
				for (; ; )
				{
					ab = direct.IndexOfAny(patenthesis, ab + 1);
					if (ab < 0) return;
					switch (direct[ab])
					{
						case '[':
							openParent.Push(ab);
							break;
						case ']':
							var corkscrew = openParent.Pop();
							pogos[ab] = corkscrew;
							pogos[corkscrew] = ab;
							break;
					}
				}
			}

			public void OnBrace(IVirtualMachine vm)
			{
				var noPart = 0 == vm.Memory[vm.MemoryPointer];
				var firstPart = '[' == vm.Instructions[vm.InstructionPointer];
				if (firstPart != noPart) return;
				vm.InstructionPointer = pogos[vm.InstructionPointer];
			}
		}
	}
}