using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public string characterName;
	Sprite image;

	public List<MenuActions> actions;
	
	public MenuActions AttackMenu;
	public MenuActions MagicMenu;
	public MenuActions FleeMenu;


	public int health;
	public int maxHealth;
	public int mana;
	public int maxMana;

	public Character(string characterName)
	{
		actions = new List<MenuActions>();
		this.characterName = characterName;
		MenuActions attack = new MenuActions("Attack");
		Skill fireball = new Skill("fireball");
		attack.skill.Add(fireball);

		actions.Add(attack);
		actions.Add(new MenuActions("Magic"));
		actions.Add(new MenuActions("Flee"));
	}

}

public class MenuActions
{
	public string menuName;
	public List<Skill> skill;

	public MenuActions(string name)
	{
		this.menuName = name;
		skill = new List<Skill>();
	}
}

public class Skill
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
