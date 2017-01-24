using UnityEngine;
using UnityEngine.PlaymodeTests;
using UnityEngine.Assertions;
using System.Collections;
using SpaceRace;

[EditModeTest]
public class MapGeneratorTest {

	[EditModeTest]
	public void MapGeneratorTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

	[EditModeTest]
	public IEnumerator MapGeneratorTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
