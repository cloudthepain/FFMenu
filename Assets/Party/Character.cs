using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public string characterName;
	Sprite image;

	public List<MenuActions> actions;
	public List<float> stats;

	public MenuActions AttackMenu;
	public MenuActions MagicMenu;
	public MenuActions FleeMenu;

	public bool turnOver;

	public float health;
	public float maxHealth;
	public float mana;
	public float maxMana;

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

		turnOver = false;

		actions.Add(attack);
		actions.Add(magic);
		actions.Add(flee);

		attack.skilllist.Add(fireball);
		magic.skilllist.Add(wish);
		flee.skilllist.Add(run);

		health = 100;
		maxHealth = 200;
		mana = 300;
		maxMana = 400;
	}

}

