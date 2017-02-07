using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRace.PlayerTools
{
	public class PlayerException : Exception
	{
		public Player Player { get; set; }

		public PlayerException() { }

		public PlayerException(string message) : base(message) { }

		public PlayerException(string message, Exception inner) : base(message, inner) { }

		public PlayerException(string message, Player player)
		{
			Player = player;
		}
	}
}
