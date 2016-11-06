using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerTools
{

	/// <summary>
	/// Datatype for transferring resources
	/// </summary>
	struct ResourceBox
	{
		/// <summary>
		/// Used for returning an empty resource box
		/// </summary>
		/// <returns>Empty resource box</returns>
		public static ResourceBox EMPTY ()
		{
			return new ResourceBox(Resources.None, 0);
		}

		public Resources Type
		{
			get { return _t; }
		}
		public int Quantity
		{
			get { return _q; }
		}

		private Resources _t;
		private int _q;

		public ResourceBox (Resources typeOfResource)
		{
			_t = typeOfResource;
			_q = 0;
		}

		public ResourceBox (Resources typeOfResource, int volumeOfResource)
		{
			_t = typeOfResource;
			_q = volumeOfResource;
		}

		/// <summary>
		/// Increase the number of resources in box
		/// </summary>
		/// <param name="volume">How much to increase resources by</param>
		public void IncreaseQuantity (int volume)
		{
			_q += volume;
		}

		/// <summary>
		/// Multiply the volume of resources in the box
		/// </summary>
		/// <param name="modifier">Modifier to change the quantity in box</param>
		public void ModifyQuantity (int modifier)
		{
			_q *= modifier;
		}

		public bool Spend (int volume)
		{
			if (volume < _q) return false;

			IncreaseQuantity(volume);
			return true;
		}
	}
}
