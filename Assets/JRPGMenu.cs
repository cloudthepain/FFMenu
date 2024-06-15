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
	List<string> characterlist = new List<string>();

	private void Start()
	{
		menuOptions.Add("Attack");
		menuOptions.Add("Magic");
		menuOptions.Add("Item");

		characterlist.Add("Cloud");
		characterlist.Add("Cloud");
		characterlist.Add("Cloud");

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
		//GenerateMenuList(menuOptions, leftButtonContainer);
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

	void GenerateCharaterList(List<string> list, VisualElement target)
	{
		var characterlistlabel = new UnityEngine.UIElements.Label();
		characterlistlabel.text = "chars";
		characterlistlabel.AddToClassList("character-list-label");
		target.Add(characterlistlabel);
		for(int i = 0; i < list.Count;i++)
		{
			var character = list[i];
			CreateCharacterDataContainer(list[i], target);
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
		menuOptions.Add("Attack");
		menuOptions.Add("Magic");
		menuOptions.Add("Item");
		var subMenuContainer = Create("submenucontainer");
		
		var scrollMenu = new ScrollView(ScrollViewMode.Horizontal);
		scrollMenu.contentContainer.AddToClassList("scroll-menu-content");
		scrollMenu.AddToClassList("scroll-menu");
		scrollMenu.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

		subMenuContainer.Add(scrollMenu);
		GenerateOptionsList(menuOptions, scrollMenu);

		document.rootVisualElement.Add(subMenuContainer);
	}

	void CreateCharacterDataContainer(string value, VisualElement target)
	{
		var characterDataContainer = Create("value-bar-container");

		var characterButton = new UnityEngine.UIElements.Button();
		characterButton.text = value;
		characterButton.clicked += () => CreateSubMenu(target);
		characterButton.AddToClassList("character-button");

		characterDataContainer.Add(characterButton);

		target.Add(characterDataContainer);
	}

	void GenerateCharacterBarsContainer(string value, VisualElement target) 
	{
		var characterBarsContainer = Create("character-bars-container");

		var labelcontainer = Create("label-container");
		characterBarsContainer.Add(labelcontainer);

		CreateHealthManaBars(labelcontainer);

		target.Add(characterBarsContainer);
	}

	void CreateHealthManaBars(VisualElement target)
	{
		var healthcontainer = Create("health-container");
		var healthlabel = new UnityEngine.UIElements.Label();
		healthlabel.text = "health";
		healthcontainer.Add(healthlabel);


		var manacontainer = Create("mana-container");
		var manalabel = new UnityEngine.UIElements.Label();
		manalabel.text = "mana";
		manacontainer.Add(manalabel);

		//

		for (int i = 0; i < 3; i++)
		{
			var healthbarcontainer = Create("health-bar-container");
			healthcontainer.Add(healthbarcontainer);

			var manabarcontainer = Create("mana-bar-container");
			manacontainer.Add(manabarcontainer);

			var healthbarnumber = new UnityEngine.UIElements.Label();
			healthbarnumber.AddToClassList("health-bar-number");
			var manabarnumber = new UnityEngine.UIElements.Label();
			manabarnumber.AddToClassList("health-bar-number");
			healthbarcontainer.Add(healthbarnumber);

			var healthbar = new UnityEngine.UIElements.ProgressBar();
			healthbar.AddToClassList("healthbar");
			healthbar.value = 100;
			healthbarnumber.text = $"{healthbar.value.ToString()} / {healthbar.value.ToString()}";
			healthbarcontainer.Add(healthbar);

			manabarcontainer.Add(manabarnumber);
			var manabar = new UnityEngine.UIElements.ProgressBar();

			manabarcontainer.Add(manabar);
			manabar.value = 100;
			manabarnumber.text = $"{manabar.value.ToString()} / {manabar.value.ToString()}";
			manabar.AddToClassList("healthbar");

		}

		target.Add(healthcontainer);
		target.Add(manacontainer);
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
