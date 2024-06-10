using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class JRPGMenu : MonoBehaviour
{
	[SerializeField] private UIDocument document;
	[SerializeField] private StyleSheet styles;

	List<Skill> menuOptions = new List<Skill>();

	private void Start()
	{
		menuOptions.Add(new Skill("Alpha"));
		menuOptions.Add(new Skill("Beta"));
		StartCoroutine(Generate());
	}

	private void OnValidate()
	{
		if (Application.isPlaying) return;
		StartCoroutine(Generate());
	}


	//Used IEnumerator to address potential race condition
	private IEnumerator Generate()
	{
		yield return null;
		var root = document.rootVisualElement;
		root.Clear();

		root.styleSheets.Add(styles);

		var menuContainer = Create("menu-container");
		var leftButtonContainer = Create("left-button-container");
		var rightmenuContainer = Create("right-menu-container");
		root.Add(menuContainer);
		menuContainer.Add(leftButtonContainer);
		menuContainer.Add(rightmenuContainer);
		GenerateList(menuOptions, leftButtonContainer);
		//GenerateList(menuOptions, rightmenuContainer);

	}


	VisualElement Create(params string[] className)
	{
		return Create<VisualElement>(className);
	}

	T Create<T>(params string[] classNames) where T : VisualElement, new()
	{
		var element = new T();
		foreach(var className in classNames)
		{

				element.AddToClassList(className);

		}
		return element;
	}

	void GenerateList(List<Skill> list, VisualElement target)
	{
		for (int i = 0; i < list.Count; i++)
		{;
			var button = Create<Button>(list[i].skillName);
			button.clicked += () => list[i].ActionSkill();
			
			button.text = list[i].skillName;
			target.Add(button);
		}
	}

	void CreateSubMenu(VisualElement ele)
	{
		Debug.Log("SubmenuCreated");
	}
}

public class Skill
{
	public Action aSkillToBeUsed;

	public string skillName;

	public Skill(string skill) {
		aSkillToBeUsed += ActionSkill;
		skillName = skill;
	}

	public void ActionSkill()
	{
		Debug.Log($"Skill ${skillName}");
	}	
}
