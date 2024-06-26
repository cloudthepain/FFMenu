using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Creates an action menu to be filled with possible options based on character actions.
/// The Menu Options will lead to the Skill list for the particular character.
/// </summary>
public class ActionMenu : MonoBehaviour
{
	private UIDocument document;
	VisualElement generatedMenuList;

	public ActionMenu(UIDocument document)
	{
		this.document = document;
		GenerateMenuList();
	}


	//Called to Create the Action Menu rather than creating it on start in order to avoid race conditions.
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


	/// <summary>
	/// Creates the buttons for the action menu that lead to the skill menu for the individual characters.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="subMenu"></param>
	public void FillMenuList(Character character, SubMenu subMenu)
	{

		//Clear the old menu options & brings the menu to the front. Otherwise the menu will have the old options & will be covered by the submenu.
		generatedMenuList.Clear();
		generatedMenuList.BringToFront();
		for (int i = 0; i < character.actions.Count; i++)
		{

			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			var menuOption = character.actions[i];
			Character passedCharacter = character;

			var button = new UnityEngine.UIElements.Button();
			button.text = character.actions[i].menuName;
			button.clicked += () =>
			{
				generatedMenuList.visible = false;
				subMenu.Reveal();
				subMenu.FillOptionsList(passedCharacter, menuOption);
			};

			//if button is clicked, reset this button
			button.AddToClassList("submenubutton");

			generatedMenuList.Add(button);
		}
	}
}
