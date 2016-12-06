using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NoTileException : Exception {

	/// <summary>
	/// Gets or sets the tile.
	/// </summary>
	/// <value>The tile.</value>
	public Type Tile { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="NoTileException"/> class.
	/// </summary>
	public NoTileException() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="NoTileException"/> class.
	/// </summary>
	/// <param name="message">Message.</param>
	public NoTileException(string message) : base(message) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="NoTileException"/> class.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="inner">Inner.</param>
	public NoTileException(string message, Exception inner) : base(message, inner) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="NoTileException"/> class.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="tile">Tile.</param>
	public NoTileException(string message, Type tile)
	{
		Tile = tile;
	}
}
