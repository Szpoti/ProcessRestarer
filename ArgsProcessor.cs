using System.Linq;

namespace ProcessRestarter;

/// <summary>
/// Class for processing program's command line arguments.
/// </summary>
public class ArgsProcessor
{
	public ArgsProcessor(string[] args)
	{
		Args = args;
	}

	public string[] Args { get; }

	public bool IsContinuousDeliveryMode() => IsOneOf("path");


	private bool IsOneOf(params string[] values)
	{
		foreach (var value in values)
		{
			foreach (var arg in Args)
			{
				if (arg.Trim().TrimStart('-').Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}

		return false;
	}
}
