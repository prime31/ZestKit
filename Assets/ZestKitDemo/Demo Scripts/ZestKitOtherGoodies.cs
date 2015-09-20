using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.ZestKit;


public class ZestKitOtherGoodies : MonoBehaviour
{
	public Transform cube;

	float _duration = 0.5f;
	TransformSpringTween _springTween;

	// custom property example
	public float wackyDoodleWidth
	{
		set
		{
			cube.localScale = new Vector3( value, cube.localScale.y, cube.localScale.z );
		}
		get
		{
			return cube.localScale.x;
		}
	}


	void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();


		if( _springTween == null )
		{
			_duration = DemoGUIHelpers.durationSlider( _duration );


			if( GUILayout.Button( "Custom Property Tween (wackyDoodleWidth)" ) )
			{
				PropertyTweens.floatPropertyTo( this, "wackyDoodleWidth", 6f, _duration )
					.setFrom( 1f )
					.setLoops( LoopType.PingPong )
					.start();
			}


			if( GUILayout.Button( "Position via Property Tween" ) )
			{
				PropertyTweens.vector3PropertyTo( cube, "position", new Vector3( 5f, 5f, 5f ), _duration )
					.setLoops( LoopType.PingPong )
					.start();
			}



			if( GUILayout.Button( "Tween Party (color, position, scale and rotation)" ) )
			{
				var party = new TweenParty( _duration );
				party.addTween( cube.GetComponent<Renderer>().material.ZKcolorTo( Color.black ) )
				    .addTween( cube.ZKpositionTo( new Vector3( 7f, 4f ) ) )
					.addTween( cube.ZKlocalScaleTo( new Vector3( 1f, 4f ) ) )
					.addTween( cube.ZKrotationTo( Quaternion.AngleAxis( 180f, Vector3.one ) ) )
					.setLoops( LoopType.PingPong )
					.start();
			}


			if( GUILayout.Button( "Tween Chain (same props as the party)" ) )
			{
				var chain = new TweenChain();
				chain.appendTween( cube.GetComponent<Renderer>().material.ZKcolorTo( Color.black, _duration ).setLoops( LoopType.PingPong ) )
					.appendTween( cube.ZKpositionTo( new Vector3( 7f, 4f ), _duration ).setLoops( LoopType.PingPong ) )
					.appendTween( cube.ZKlocalScaleTo( new Vector3( 1f, 4f ), _duration ).setLoops( LoopType.PingPong ) )
					.appendTween( cube.ZKrotationTo( Quaternion.AngleAxis( 180f, Vector3.one ), _duration ).setLoops( LoopType.PingPong ) )
					.start();
			}


			if( GUILayout.Button( "Chaining Tweens Directly (same props as the party)" ) )
			{
				cube.GetComponent<Renderer>().material.ZKcolorTo( Color.black, _duration ).setLoops( LoopType.PingPong )
					.setNextTween
					(
						cube.ZKpositionTo( new Vector3( 7f, 4f ), _duration ).setLoops( LoopType.PingPong ).setNextTween
						(
							cube.ZKlocalScaleTo( new Vector3( 1f, 4f ), _duration ).setLoops( LoopType.PingPong ).setNextTween
							(
								cube.ZKrotationTo( Quaternion.AngleAxis( 180f, Vector3.one ), _duration ).setLoops( LoopType.PingPong )
							)
						)
					)
					.start();
			}


			GUILayout.Space( 10 );
			if( GUILayout.Button( "Start Spring Position" ) )
			{
				_springTween = new TransformSpringTween( cube, TransformTargetType.Position, cube.position );
			}


			if( GUILayout.Button( "Start Spring Position (overdamped)" ) )
			{
				_springTween = new TransformSpringTween( cube, TransformTargetType.Position, cube.position );
				_springTween.dampingRatio = 1.5f;
				_springTween.angularFrequency = 20f;
			}


			if( GUILayout.Button( "Start Spring Scale" ) )
			{
				_springTween = new TransformSpringTween( cube, TransformTargetType.LocalScale, cube.localScale );
			}
			GUILayout.Space( 10 );


			if( GUILayout.Button( "Run Action Every 1s After 2s Delay" ) )
			{
				ActionTask.every( 2f, 1f, this, task =>
				{
					// by using the context we get away with not allocating when passing this Action around!
					( task.context as ZestKitOtherGoodies ).methodCalledForDemonstrationPurposes();
				});
			}


			if( GUILayout.Button( "ActionTask Interoperability" ) )
			{
				Debug.Log( "The Story: An ActionTask with a 2s delay will be created with a continueWith ActionTask appended to it that will tick every 0.3s for 2s. The original ActionTask will have a waitFor called that is an ActionTask with a 1s delay. Follow?" );
				Debug.Log( "--- current time: " + Time.time );

				ActionTask.afterDelay( 2f, this, task =>
				{
					Debug.Log( "--- root task ticked: " + Time.time );
				}).continueWith
				(
					ActionTask.create( this, task =>
					{
						Debug.Log( "+++ continueWith task elapsed time: " + task.elapsedTime );
						if( task.elapsedTime > 2f )
							task.stop();
					})
					.setDelay( 1f )
					.setRepeats( 0.3f )
				).waitFor
				(
					ActionTask.afterDelay( 1f, this, task =>
					{
						Debug.Log( "--- waitFor ticked: " + Time.time );
					})
				);
			}

			DemoGUIHelpers.easeTypesGUI();
		}
		else
		{
			GUILayout.Label( "While the spring tween is active the cube will spring to\n" + 
							 "whichever location you click or scale x/y to that location\n" +
							 "if you chose a scale spring. The sliders below let you tweak\n" + 
							 "the spring contants.\n\n" + 
							 "For the scale tween, try clicking places on a horizontal or vertical\n" + 
							 "axis to get a feel for how it works.");

			springSliders();

			var prefix = _springTween.targetType == TransformTargetType.Position ? "Spring position to:" : "Spring scale to:";
			var mousePosWorld = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			var labelText = string.Format( "{0}\nx: {1:F1}\ny: {2:F1}", prefix, mousePosWorld.x, mousePosWorld.y );
			GUI.Label( new Rect( Input.mousePosition.x, Screen.height - Input.mousePosition.y - 50f, 130f, 50f ), labelText );


			if( GUILayout.Button( "Stop Spring" ) )
			{
				_springTween.stop();
				_springTween = null;
				cube.position = new Vector3( -1f, -2f );
				cube.localScale = Vector3.one;
			}
		}
	}


	void methodCalledForDemonstrationPurposes()
	{
		Debug.Log( "methodCalledForDemonstrationPurposes was called at " + Time.time );
	}


	void Update()
	{
		if( _springTween != null && Input.GetMouseButtonDown( 0 ) )
		{
			// fetch the clicked position but keep z 1 so we dont move the cube behind the camera
			var newTargetValue = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			newTargetValue.z = 1f;
			_springTween.setTargetValue( newTargetValue );
		}
	}


	// helpers for the GUI sliders
	void springSliders()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Damping Ratio", GUILayout.Width( 110 ) );
		GUI.skin.horizontalSlider.margin = new RectOffset( 4, 4, 10, 4 );
		_springTween.dampingRatio = GUILayout.HorizontalSlider( _springTween.dampingRatio, 0.1f, 0.75f, GUILayout.ExpandWidth( true ) );
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		GUILayout.Label( "Angular Frequency", GUILayout.Width( 110 ) );
		GUI.skin.horizontalSlider.margin = new RectOffset( 4, 4, 10, 4 );
		_springTween.angularFrequency = GUILayout.HorizontalSlider( _springTween.angularFrequency, 1f, Mathf.PI * 12f, GUILayout.ExpandWidth( true ) );
		GUILayout.EndHorizontal();
	}
}
