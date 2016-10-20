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

		public ResourceBox (Resources typeOfResource, int volumeOfResource)
		{
			_t = typeOfResource;
			_q = volumeOfResource;
		}
	}
}
