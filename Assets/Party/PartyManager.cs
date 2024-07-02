using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
	public List<Character> characterlist = new List<Character>();
	public List<Character> enemyList = new List<Character>();

	public PartyManager()
	{
		characterlist.Add(new Character("Steven"));
		characterlist.Add(new Character("Baron"));


		enemyList.Add(new Character("Skeleton"));
		enemyList.Add(new Character("Skeleton Archer"));
	}
}
