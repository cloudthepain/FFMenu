using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class JRPGMenu : MonoBehaviour
{
	[SerializeField] private UIDocument document;
	[SerializeField] private StyleSheet styles;
	[SerializeField] public Sprite selectorSprite;

	List<string> menuOptions = new List<string>();

	private void Start()
	{
		menuOptions.Add("Attack");
		menuOptions.Add("Magic");
		menuOptions.Add("Item");
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
		GenerateMenuList(menuOptions, leftButtonContainer);
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

	void GenerateMenuList(List<string> list, VisualElement target)
	{
		for (int i = 0; i < list.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var skill = list[i];
			var button = new UnityEngine.UIElements.Button();
			button.text = list[i];
			button.clicked += () => CreateSubMenu(target);
			button.AddToClassList("submenubutton");
			target.Add(button);
		}
	}


	void GenerateOptionsList(List<string> list, VisualElement target)
	{
		for (int i = 0; i < list.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var skill = list[i];

			CreateButton(list[i], target);
		}
	}

	void CreateButton(string data, VisualElement target)
	{
		var newContainer = Create("buttonContainer");
		
		var newButton = new UnityEngine.UIElements.Button();
		newButton.text = data;
		newButton.clicked += () => Debug.Log("Button Pressed");
		newButton.AddToClassList("submenubutton");
		
		var selector = new UnityEngine.UIElements.Image();
		selector.AddToClassList("button-selector-bullet");
		selector.sprite = selectorSprite;

		newContainer.Add(selector);
		newContainer.Add(newButton);

		target.Add(newContainer);
	}

	void CreateSubMenu(VisualElement ele)
	{
		for(int i = 0; i < 30; i++)
		{
			menuOptions.Add($"{i}");
		}
		menuOptions.Add("Attack");
		menuOptions.Add("Magic");
		menuOptions.Add("Item");
		Debug.Log("Menu Created");
		var subMenuContainer = Create("submenucontainer");
		
		var scrollMenu = new ScrollView(ScrollViewMode.Horizontal);
		scrollMenu.contentContainer.AddToClassList("scroll-menu-content");
		scrollMenu.AddToClassList("scroll-menu");
		scrollMenu.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

		subMenuContainer.Add(scrollMenu);
		GenerateOptionsList(menuOptions, scrollMenu);

		document.rootVisualElement.Add(subMenuContainer);

	}

}

public class Skill
{
	public string skillName;

	public Skill(string skill) {
		skillName = skill;
	}

	public void ActionSkill()
	{
		Debug.Log($"Skill {skillName}");
	}	
}
