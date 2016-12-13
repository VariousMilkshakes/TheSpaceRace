using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRace.PlayerTools
{

	/// <summary>
	/// Datatype for transferring resources
	/// </summary>
	public struct ResourceBox
	{
		/// <summary>
		/// Used for returning an empty resource box
		/// </summary>
		/// <returns>Empty resource box</returns>
		public static ResourceBox EMPTY ()
		{
			return new ResourceBox(Resource.None, 0);
		}

		public Resource Type
		{
			get { return _t; }
		}
		public int Quantity
		{
			get { return _q; }
		}
		public int Cap
		{
			get { return _c; }
			set { _c = value; }
		}

		private Resource _t;
		private int _q;
		private int _c;

		public ResourceBox (Resource typeOfResource)
		{
			_t = typeOfResource;
			_q = 0;
			_c = -1;
		}

		public ResourceBox (Resource typeOfResource, int volumeOfResource)
		{
			_t = typeOfResource;
			_q = volumeOfResource;
			_c = -1;
		}

		public ResourceBox(Resource typeOfResource, int volumeOfResource, int volumeCap)
		{
			_t = typeOfResource;
			_q = volumeOfResource;
			_c = volumeCap;
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
		/// Max out resource box to capacity
		/// </summary>
		/// <param name="volume">The amount of resource that is trying to fit in the box</param>
		/// <returns>The quantity of resource used to fill box</returns>
		public int Fill (int volume)
		{
			if (_c == -1 || (volume + _q) <= _c)
			{
				IncreaseQuantity(volume);
				return volume;
			}

			int dif = _c - _q;
			_q = _c;
			return dif;
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

		public bool IsFull ()
		{
			if (_q == _c) return true;
			return false;
		}

		public void Empty ()
		{
			_q = 0;
		}
	}
}
