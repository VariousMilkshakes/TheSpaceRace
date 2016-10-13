using UnityEngine;

public interface IConstructable
{
	public SpriteRenderer GetSprite ();
	public GameObject GetGameObject ();

	public bool IsConstructable ();

	public void OnConstruct ();
}