using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zones{
	
	public class ZoneManager{

		public ZoneManager(){
		}

		public Zone CombineZones(List<Zone> zones, int size){
			int newSize = zones[0].GetSize() * 2;
			TileFlag[,] resultingZone = new TileFlag[newSize, newSize];
			for (int x = 0; x < size; x++) {
				for (int y = 0; y < size; y++) {
					resultingZone [x, y] = zones[0].GetZone() [x, y];
					resultingZone [x, zones [1].GetSize () + y] = zones [1].GetZone () [x, y];
					resultingZone [zones [2].GetSize () + x, y] = zones [2].GetZone () [x, y];
					resultingZone [zones[3].GetSize() + x, zones[3].GetSize() + y] = zones[3].GetZone() [x, y];
				}
			}

			Zone newZone = new Zone (resultingZone, resultingZone.Length - 1);

			newZone.CreateCoast ();

			return newZone; 
		}

	}
}
