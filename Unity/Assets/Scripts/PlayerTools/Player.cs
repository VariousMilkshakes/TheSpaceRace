﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerTools
{
	[System.Serializable]
	class Player
	{

		private Inventory inventory;

		public Inventory Inventory
		{
			get { return inventory; }
		}

		public Player ()
		{
			inventory = new Inventory();
		}

	}
}
