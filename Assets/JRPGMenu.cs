using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using static UnityEditor.Progress;
using System.Linq;

public class JRPGMenu : MonoBehaviour
{
	[SerializeField] private UIDocument document;
	[SerializeField] private StyleSheet styles;
	[SerializeField] public Sprite selectorSprite;

	List<string> menuOptions = new List<string>();
	List<Character> characterlist = new List<Character>();

	List<UnityEngine.UIElements.Button> buttonlist = new List<UnityEngine.UIElements.Button>();

	SubMenu submenu;
	ActionMenu actionMenu;

	private void Start()
	{
		characterlist.Add(new Character("Steven"));
		characterlist.Add(new Character("Baron"));
		characterlist.Add(new Character("Yo-Ho"));
		StartCoroutine(Generate());

	}

	private void OnValidate()
	{
		if (Application.isPlaying) return;
		StartCoroutine(Generate());
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			ResetButtons();
		}
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


		actionMenu = new ActionMenu(document);
		actionMenu.GenerateMenuList();
		actionMenu.Hide();

		submenu = new SubMenu(document, selectorSprite);
		submenu.Hide();

		GenerateCharaterList(characterlist, leftButtonContainer);
		GenerateCharacterBarsContainer(name, rightmenuContainer);
	}

	//Create
	#region
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
	#endregion

	//Character
	#region
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


	//Character Name button
	void CreateCharacterDataContainer(Character character, VisualElement target)
	{
		var characterDataContainer = Create("value-bar-container");

		var characterImage = new UnityEngine.UIElements.Image();
		characterImage.sprite = selectorSprite;
		characterImage.AddToClassList("character-image");

		var characterButton = new UnityEngine.UIElements.Button();
		characterButton.text = character.characterName;
		characterButton.clicked += () =>
		{
			characterButton.SetEnabled(false);
			actionMenu.Reveal();
			actionMenu.FillMenuList(character, submenu);
			CheckAllCharactersUsed();
		};
		
		characterButton.AddToClassList("character-button");

		characterDataContainer.Add(characterImage);
		characterDataContainer.Add(characterButton);

		target.Add(characterDataContainer);
		buttonlist.Add(characterButton);
	}

	void ResetButtons()
	{
		for(int i = 0; i < buttonlist.Count; i++)
		{
			buttonlist[i].SetEnabled(true);
		}	
	}

	bool CheckAllCharactersUsed()
	{
		bool alldisabled = true;

		for (int i = 0; i < buttonlist.Count; i++)
		{
			if (buttonlist[i].enabledInHierarchy)
			{
				return(!alldisabled);
			}
		}
		return alldisabled;
	}

	void GenerateCharacterBarsContainer(string value, VisualElement target)
	{
		var characterBarsContainer = Create("character-bars-container");

		var labelcontainer = Create("label-container");
		characterBarsContainer.Add(labelcontainer);

		CreateStatusBars(labelcontainer);

		target.Add(characterBarsContainer);
	}
	#endregion

	//Progress Bars
	#region
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
	#endregion
}

class ActionMenu
{

	private UIDocument document;
	VisualElement generatedMenuList;

	public ActionMenu(UIDocument document)
	{
		this.document = document;
		GenerateMenuList();
	}

	public void GenerateMenuList()
	{
		generatedMenuList = new VisualElement();
		generatedMenuList.AddToClassList("menu-list-container");

		document.rootVisualElement.Add(generatedMenuList);
	}

	public void Hide()
	{
		generatedMenuList.visible = false;
	}
	public void Reveal()
	{
		generatedMenuList.visible = true;
	}

	public void FillMenuList(Character character, SubMenu subMenu)
	{

		//Clear the old menu options & brings the menu to the front. Otherwise the menu will have the old options & will be covered by the submenu.
		generatedMenuList.Clear();
		generatedMenuList.BringToFront();
		for (int i = 0; i < character.actions.Count; i++)
		{

			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var menuOption = character.actions[i];

			var button = new UnityEngine.UIElements.Button();
			button.text = character.actions[i].menuName;
			button.clicked += () =>
			{
				generatedMenuList.visible = false;
				subMenu.Reveal();
				subMenu.FillOptionsList(menuOption);
			};
			button.AddToClassList("submenubutton");

			generatedMenuList.Add(button);
		}
	}
}

class SubMenu
{
	private UIDocument document;
	
	public VisualElement subMenuContainer;
	ScrollView scrollMenu;
	Sprite selectorSprite;
	VisualElement childElement;

	public SubMenu(UIDocument document, Sprite selectorSprite)
	{
		this.document = document;
		this.selectorSprite = selectorSprite;
		GenerateSubMenu();
	}

	public void GenerateSubMenu()
	{
		subMenuContainer = new VisualElement();
		subMenuContainer.AddToClassList("submenucontainer");

		scrollMenu = new ScrollView(ScrollViewMode.Horizontal);
		scrollMenu.contentContainer.AddToClassList("scroll-menu-content");
		scrollMenu.AddToClassList("scroll-menu");
		scrollMenu.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

		subMenuContainer.Add(scrollMenu);

		document.rootVisualElement.Add(subMenuContainer);
	}

	public void Reveal()
	{
		subMenuContainer.visible = true;
		scrollMenu.visible = true;
	}

	public void Hide()
	{
		subMenuContainer.visible = false;
		scrollMenu.visible = false;
	}


	public void FillSubMenu(List<MenuActions> actions)
	{
		scrollMenu.Clear();
		for (int i = 0; i < actions.Count; i++)
		{
			FillOptionsList(actions[i]);
		}

	}

	public void FillOptionsList(MenuActions action)
	{
		scrollMenu.Clear();
		for (int i = 0; i < action.skilllist.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			Debug.Log(action.skilllist[i].skillName);
			var skill = action.skilllist[i];

			CreateButton(skill, scrollMenu);
		}
	}

	public void CreateButton(Skill skill, VisualElement target)
	{
		var newContainer = new VisualElement();
		newContainer.AddToClassList("buttonContainer");

		var newButton = new UnityEngine.UIElements.Button();
		
		newButton.text = skill.skillName;
		newButton.clicked += () =>
		{
			newButton.parent.parent.visible = false;
			skill.ActionSkill();
		};

		
		newButton.AddToClassList("submenubutton");

		var selector = new UnityEngine.UIElements.Image();
		selector.AddToClassList("button-selector-bullet");
		selector.sprite = selectorSprite;

		newContainer.Add(selector);
		newContainer.Add(newButton);

		target.Add(newContainer);
	}
}

