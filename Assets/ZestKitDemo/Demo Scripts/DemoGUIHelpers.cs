using UnityEngine;
using System.Collections;
using Prime31.ZestKit;
using System;


public static class DemoGUIHelpers
{
	static string[] _allEaseTypes;
	static GUIContent[] comboBoxList;
	static ComboBox comboBoxControl;
	static GUIStyle listStyle = new GUIStyle();

	static private bool _didRetinaIpadCheck;
	static private bool _isRetinaIpad;


	static bool isRetinaOrLargeScreen()
	{
		return Screen.width >= 960 || Screen.height >= 960;
	}


	static bool isRetinaIpad()
	{
		if( !_didRetinaIpadCheck )
		{
			if( Screen.height >= 2048 || Screen.width >= 2048 )
				_isRetinaIpad = true;

			_didRetinaIpadCheck = true;
		}

		return _isRetinaIpad;
	}


	static int buttonHeight()
	{
#if !UNITY_EDITOR
		if( isRetinaOrLargeScreen() )
		{
			if( isRetinaIpad() )
				return 140;
			return 70;
		}
#endif

		return 30;
	}


	static int buttonFontSize()
	{
#if !UNITY_EDITOR
		if( isRetinaOrLargeScreen() )
		{
			if( isRetinaIpad() )
				return 40;
			return 25;
		}
#endif

		return 15;
	}


	static DemoGUIHelpers()
	{
		_allEaseTypes = Enum.GetNames( typeof( EaseType ) );

		comboBoxList = new GUIContent[_allEaseTypes.Length];
		for( var i = 0; i < _allEaseTypes.Length; i++ )
			comboBoxList[i] = new GUIContent( _allEaseTypes[i] );

		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background = listStyle.hover.background = new Texture2D( 2, 2 );
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;

		comboBoxControl = new ComboBox( new Rect( Screen.width - 140, 20, 120, buttonHeight() ), comboBoxList[0], comboBoxList, "button", "box", listStyle );
		comboBoxControl.SelectedItemIndex = 14;
	}


	public static void easeTypesGUI()
	{
		if( comboBoxControl.Show() )
		{
			var easeType = (EaseType)Enum.Parse( typeof( EaseType ), _allEaseTypes[comboBoxControl.SelectedItemIndex] );
			ZestKit.defaultEaseType = easeType;
		}
	}


	public static void setupGUIButtons()
	{
		GUI.skin.button.fontSize = buttonFontSize();
		GUI.skin.button.margin = new RectOffset( 0, 0, 10, 0 );
		GUI.skin.button.stretchWidth = true;
		GUI.skin.button.fixedHeight = buttonHeight();
		GUI.skin.button.wordWrap = false;
		GUI.skin.button.active.textColor = Color.black;
	}


	public static float durationSlider( float duration, float maxDuration = 2f )
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label( string.Format( "Duration: {0:0.0}", duration ), GUILayout.Width( 80 ) );
		GUI.skin.horizontalSlider.margin = new RectOffset( 4, 4, 10, 4 );
		var result = GUILayout.HorizontalSlider( duration, 0f, maxDuration, GUILayout.ExpandWidth( true ) );
		GUILayout.EndHorizontal();

		return result;
	}
}



/*
 * 
// Popup list created by Eric Haines
// ComboBox Extended by Hyungseok Seo.(Jerry) sdragoon@nate.com
// Refactored by zhujiangbo jumbozhu@gmail.com
// Slight edit for button to show the previously selected item AndyMartin458 www.clubconsortya.blogspot.com
// 
// -----------------------------------------------
// This code working like ComboBox Control.
// I just changed some part of code, 
// because I want to seperate ComboBox button and List.
// ( You can see the result of this code from Description's last picture )
// -----------------------------------------------
*/

public class ComboBox
{
	private static bool forceToUnShow = false; 
	private static int useControlID = -1;
	private bool isClickedComboButton = false;
	private int selectedItemIndex = 0;

	private Rect rect;
	private GUIContent buttonContent;
	private GUIContent[] listContent;
	private string buttonStyle;
	private string boxStyle;
	private GUIStyle listStyle;
	Vector2 _scrollPos;


	public ComboBox( Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle )
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
	}

	public ComboBox( Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle )
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
	}

	public bool Show()
	{
		if( forceToUnShow )
		{
			forceToUnShow = false;
			isClickedComboButton = false;
		}

		bool result = false;
		bool done = false;
		int controlID = GUIUtility.GetControlID( FocusType.Passive );       

		switch( Event.current.GetTypeForControl(controlID) )
		{
			case EventType.MouseUp:
				{
					if( isClickedComboButton )
					{
						done = true;
					}
				}
				break;
		}       

		if( GUI.Button( rect, buttonContent, buttonStyle ) )
		{
			if( useControlID == -1 )
			{
				useControlID = controlID;
				isClickedComboButton = false;
			}

			if( useControlID != controlID )
			{
				forceToUnShow = true;
				useControlID = controlID;
			}
			isClickedComboButton = true;
		}

		if( isClickedComboButton )
		{
			Rect listRect = new Rect( rect.x, rect.y + listStyle.CalcHeight( listContent[0], 1.0f ),
				rect.width, listStyle.CalcHeight( listContent[0], 1.0f ) * listContent.Length );

			_scrollPos = GUI.BeginScrollView( new Rect( rect.xMin, rect.yMin + rect.height, rect.width + 18, Screen.height - 100f ), _scrollPos, listRect, false, true );
			GUI.Box( listRect, "", boxStyle );

			int newSelectedItemIndex = GUI.SelectionGrid( listRect, selectedItemIndex, listContent, 1, listStyle );
			if( newSelectedItemIndex != selectedItemIndex )
			{
				selectedItemIndex = newSelectedItemIndex;
				buttonContent = listContent[selectedItemIndex];
				result = true;
			}
			GUI.EndScrollView( true );
		}

		if( done )
			isClickedComboButton = false;

		return result;
	}


	public int SelectedItemIndex
	{
		get{
			return selectedItemIndex;
		}
		set{
			selectedItemIndex = value;
			buttonContent = listContent[selectedItemIndex];
		}
	}
}