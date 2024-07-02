using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
	public string skillName;

	public TargetType target;

	public Skill(string skill, TargetType target)
	{
		skillName = skill;
	}

	public void ActionSkill(Character target)
	{
		Debug.Log($"{skillName} was used against {target.characterName}");
	}

}

public enum TargetType
{
	SingleEnemy,
	AllEnemies,
	SingleAlly,
	AllAllies,
	Self

}
