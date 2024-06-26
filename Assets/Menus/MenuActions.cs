using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActions : MonoBehaviour
{
	public string menuName;
	public List<Skill> skilllist;

	public MenuActions(string name)
	{
		this.menuName = name;
		skilllist = new List<Skill>();
	}
}
