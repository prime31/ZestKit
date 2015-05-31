using UnityEngine;
using System.Collections;
using Prime31.ZestKit;


public class ZestKitSmoothedValues : MonoBehaviour
{
	public Transform cubeTransform;

	SmoothedFloat _smoothedFloat;
	SmoothedVector3 _smoothedVector3;


	void Awake()
	{
		_smoothedFloat = new SmoothedFloat( 0f, 2f );
		_smoothedVector3 = new SmoothedVector3( cubeTransform.position, 0.5f );
	}


	void Update()
	{
		_smoothedFloat.easeType = ZestKit.defaultEaseType;
		_smoothedVector3.easeType = ZestKit.defaultEaseType;

		if( Input.GetMouseButtonDown( 0 ) )
		{
			var newTargetValue = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, 10f ) );
			newTargetValue.z = cubeTransform.position.z;
			_smoothedVector3.setToValue( newTargetValue );
		}

		cubeTransform.position = _smoothedVector3.value;
	}


	void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();


		GUILayout.Label( "Smoothed Float with Slider to Visualize Easing" );
		GUILayout.BeginHorizontal();

		if( GUILayout.Button( "Set To Value to 10" ) )
		{
			_smoothedFloat.setToValue( 10f );
		}


		if( GUILayout.Button( "Set To Value to -10" ) )
		{
			_smoothedFloat.setToValue( -10f );
		}

		GUILayout.EndHorizontal();

		GUILayout.HorizontalSlider( _smoothedFloat.value, -10f, 10f, GUILayout.Width( 250 ) );


		DemoGUIHelpers.easeTypesGUI();
	}
}
