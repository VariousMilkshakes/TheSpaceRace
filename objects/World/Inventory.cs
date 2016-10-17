using System;
using System.Collections;

public class Inventory
{
	private Dictionary<string, int> resources;  
	public Inventory ()
	{
		resources = new Dictionary<string, int>();
	}

	public bool CheckResource (string targetResource)
	{
		int count = resources.Get(targetResource);

		if (targetResource)
	}
}