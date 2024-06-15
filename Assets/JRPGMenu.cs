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
	List<Character> characterlist = new List<Character>();

	private void Start()
	{
		characterlist.Add(new Character("Steven"));
		characterlist.Add(new Character("Baron"));
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

		GenerateCharaterList(characterlist, leftButtonContainer);
		GenerateCharacterBarsContainer(name, rightmenuContainer);
	}


	VisualElement Create(params string[] className)
	{
		return Create<VisualElement>(className);
	}

	T Create<T>(params string[] classNames) where T : VisualElement, new()
	{
		var element = new T();
		foreach (var className in classNames)
		{

			element.AddToClassList(className);

		}
		return element;
	}

	void GenerateMenuList(Character character, VisualElement target)
	{
		var GeneratedMenuList = Create("menu-list-container");


		for (int i = 0; i < character.actions.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var menuOption = character.actions[i];

			var button = new UnityEngine.UIElements.Button();
			button.text = character.actions[i].menuName;
			button.clicked += () => CreateSubMenu(character.actions, target);
			button.AddToClassList("submenubutton");

			GeneratedMenuList.Add(button);
		}

		document.rootVisualElement.Add(GeneratedMenuList);
	}

	void CreateSubMenu(List<MenuActions> actions, VisualElement ele)
	{
		var subMenuContainer = Create("submenucontainer");

		var scrollMenu = new ScrollView(ScrollViewMode.Horizontal);
		scrollMenu.contentContainer.AddToClassList("scroll-menu-content");
		scrollMenu.AddToClassList("scroll-menu");
		scrollMenu.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

		subMenuContainer.Add(scrollMenu);

		for(int i = 0;i < actions.Count;i++)
		{
			GenerateOptionsList(actions[i], scrollMenu);
		}

		document.rootVisualElement.Add(subMenuContainer);
	}

	void GenerateCharaterList(List<Character> party, VisualElement target)
	{
		var characterlistlabel = new UnityEngine.UIElements.Label();
		characterlistlabel.text = "chars";
		characterlistlabel.AddToClassList("character-list-label");
		target.Add(characterlistlabel);
		for (int i = 0; i < party.Count; i++)
		{
			var character = party[i];
			CreateCharacterDataContainer(party[i], target);
		}
	}

	void CreateCharacterDataContainer(Character character, VisualElement target)
	{
		var characterDataContainer = Create("value-bar-container");

		var characterImage = new UnityEngine.UIElements.Image();
		characterImage.sprite = selectorSprite;
		characterImage.AddToClassList("character-image");

		var characterButton = new UnityEngine.UIElements.Button();
		characterButton.text = character.characterName;
		characterButton.clicked += () => GenerateMenuList(character, document.rootVisualElement);
		characterButton.AddToClassList("character-button");

		characterDataContainer.Add(characterImage);
		characterDataContainer.Add(characterButton);

		target.Add(characterDataContainer);
	}



	void GenerateOptionsList(MenuActions action, VisualElement target)
	{
		for (int i = 0; i < action.skill.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var skillList = action.skill[i];

			CreateButton(action.skill[i].skillName, target);
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

	void GenerateCharacterBarsContainer(string value, VisualElement target)
	{
		var characterBarsContainer = Create("character-bars-container");

		var labelcontainer = Create("label-container");
		characterBarsContainer.Add(labelcontainer);

		CreateStatusBars(labelcontainer);

		target.Add(characterBarsContainer);
	}

	void CreateStatusBars(VisualElement target)
	{
		var healthContainer = CreateBarContainer("health");
		var manaContainer = CreateBarContainer("mana");
		var xpContainer = CreateBarContainer("xp");


		for (int i = 0; i < characterlist.Count; i++)
		{
			CreateProgressBar(100, healthContainer);
			CreateProgressBar(200, manaContainer);
			CreateProgressBar(300, xpContainer);
		}

		target.Add(healthContainer);
		target.Add(manaContainer);
		target.Add(xpContainer);
	}

	VisualElement CreateBarContainer(string value)
	{
		var barContainer = Create("bar-container");
		var barlabel = new UnityEngine.UIElements.Label();
		barlabel.text = value;
		barContainer.Add(barlabel);

		return barContainer;
	}

	void CreateProgressBar(int value, VisualElement target)
	{
		var container = Create("progress-bar-container");
		target.Add(container);

		var barNumber = new UnityEngine.UIElements.Label();
		barNumber.AddToClassList("bar-number");
		barNumber.text = $"{value.ToString()} / {value.ToString()}";
		container.Add(barNumber);

		var progressBar = new UnityEngine.UIElements.ProgressBar();
		progressBar.AddToClassList("progress-bar");
		progressBar.value = 100;
		container.Add(progressBar);

		target.Add(container);
	}

}

