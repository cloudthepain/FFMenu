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
		MenuActions magic = new MenuActions("Magic");
		MenuActions flee = new MenuActions("Flee");
		
		Skill fireball = new Skill("fireball");
		Skill wish = new Skill("wish");
		Skill run = new Skill("Run");

		actions.Add(attack);
		actions.Add(magic);
		actions.Add(flee);

		attack.skilllist.Add(fireball);
		magic.skilllist.Add(wish);
		flee.skilllist.Add(run);

	}

}

public class MenuActions
{
	public string menuName;
	public List<Skill> skilllist;

	public MenuActions(string name)
	{
		this.menuName = name;
		skilllist = new List<Skill>();
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
