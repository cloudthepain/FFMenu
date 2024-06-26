using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// SubMenu includes the Skill Menu for Characters. When opened a scrollview will populate with
/// character skills that can be picked for use.
/// </summary>
public class SubMenu : MonoBehaviour
{
	private UIDocument document;

	public VisualElement subMenuContainer;
	ScrollView scrollMenu;
	Sprite selectorSprite;
	public List<Character> characterTurnComplete;

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

	public void FillOptionsList(Character character, MenuActions action)
	{
		scrollMenu.Clear();
		for (int i = 0; i < action.skilllist.Count; i++)
		{
			//Local Variable made to address error condition with lambda where value was not being properly assigned.
			Debug.Log(action.skilllist[i].skillName);
			var skill = action.skilllist[i];

			CreateButton(character, skill, scrollMenu);
		}
	}

	public void CreateButton(Character character, Skill skill, VisualElement target)
	{
		var newContainer = new VisualElement();
		newContainer.AddToClassList("buttonContainer");

		var newButton = new UnityEngine.UIElements.Button();

		newButton.text = skill.skillName;
		newButton.clicked += () =>
		{
			//hides the previous men
			newButton.parent.parent.visible = false;
			skill.ActionSkill();
			character.turnOver = true;
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
