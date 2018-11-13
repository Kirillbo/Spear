using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SpritesBackgrounds")]
public class SpritesBase : ScriptableObject
{

   
	public Sprite[] Sprites;


	public int Lengths
	{
		get { return Sprites.Length; }
	}


}
