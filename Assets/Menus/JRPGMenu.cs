using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using static UnityEditor.Progress;
using System.Linq;
using UnityEngine.TextCore.Text;

public class JRPGMenu : MonoBehaviour
{
	[SerializeField] private UIDocument document;
	[SerializeField] private StyleSheet styles;

	//Sprite used to designate the "selector or highlighter" for the buttons.
	[SerializeField] public Sprite selectorSprite;

	List<UnityEngine.UIElements.Button> buttonlist = new List<UnityEngine.UIElements.Button>();

	SubMenu submenu;
	ActionMenu actionMenu;
	PartyManager partyManager;

	VisualElement menuContainer;
	VisualElement leftButtonContainer;
	VisualElement rightmenuContainer;

	private void Start()
	{
		StartCoroutine(Generate());
	}

	private void OnValidate()
	{
		if (Application.isPlaying) return;
		StartCoroutine(Generate());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ResetButtons();
		}
	}

	/// <summary>
	/// Cancels the button choice & allows for a different option to be selected.
	/// </summary>
	void ResetButtons()
	{
		submenu.Hide();
		actionMenu.Hide();
		if (CheckAllCharactersUsed()) { return; }
		GenerateCharaterList(partyManager.characterlist, leftButtonContainer);
	}

	//Used IEnumerator to address potential race condition
	private IEnumerator Generate()
	{
		yield return null;
		var root = document.rootVisualElement;
		root.Clear();

		root.styleSheets.Add(styles);

		menuContainer = Create("menu-container");
		leftButtonContainer = Create("left-button-container");
		rightmenuContainer = Create("right-menu-container");
		root.Add(menuContainer);

		menuContainer.Add(leftButtonContainer);
		menuContainer.Add(rightmenuContainer);

		partyManager = new PartyManager();

		actionMenu = new ActionMenu(document);
		actionMenu.GenerateMenuList();
		actionMenu.Hide();

		submenu = new SubMenu(document, selectorSprite);
		submenu.Hide();

		GenerateCharaterList(partyManager.characterlist, leftButtonContainer);
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
		target.Clear();
		var characterlistlabel = new UnityEngine.UIElements.Label();
		characterlistlabel.text = "chars";
		characterlistlabel.AddToClassList("character-list-label");
		target.Add(characterlistlabel);

		for(int i= 0; i < party.Count; i++)
		{
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
		
		if (character.turnOver)
		
		{
			characterButton.SetEnabled(false);
		}

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

		buttonlist.Add(characterButton);

		target.Add(characterDataContainer);
	}

	bool CheckAllCharactersUsed()
	{
		for(int i = 0; i < partyManager.characterlist.Count; i++)
		{
			if (!partyManager.characterlist[i].turnOver)
			{
				Debug.Log($"{partyManager.characterlist[i].characterName} hasn't gone!");
				return false;
			}
		}
		return true;
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


		for (int i = 0; i < 3; i++)
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

