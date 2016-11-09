using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NoTileException : Exception {

	public Type Tile { get; set; }

	public NoTileException() { }

	public NoTileException(string message) : base(message) { }

	public NoTileException(string message, Exception inner) : base(message, inner) { }

	public NoTileException(string message, Type tile)
	{
		Tile = tile;
	}
}
