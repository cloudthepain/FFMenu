using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
	public string skillName;

	public Skill(string skill)
	{
		skillName = skill;
	}

	public void ActionSkill()
	{
		Debug.Log($"Skill {skillName}");
	}
}
