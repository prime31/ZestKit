using UnityEngine;
using System.Collections;
using Prime31.ZestKit;


public class ZestKitSplineDemo : MonoBehaviour
{
	public Transform quad;

	float _duration = 2.5f;


	void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();
		_duration = DemoGUIHelpers.durationSlider( _duration, 5f );
		DemoGUIHelpers.easeTypesGUI();


		GUILayout.Label( "The splines used in this scene are on the\n*DummySpline GameObjects so you can\nhave a look at them." );
		GUILayout.Label( "Just select the GameObject and the gizmos\nwill be drawn in the scene view." );
		GUILayout.Space( 20 );

		if( GUILayout.Button( "Figure Eight Spline Tween (relative)" ) )
		{
			var spline = new Spline( "figureEight" );
			spline.closePath(); // we have to let the spline know it should close itself if using EaseTypes that go negative like Elastic to avoid clamping

			new SplineTween( quad, spline, _duration )
				.setIsRelative()
				.start();
		}
			

		if( GUILayout.Button( "Figure Eight Spline Tween (absolute)" ) )
		{
			var spline = new Spline( "figureEight" );
			spline.closePath();

			new SplineTween( quad, spline, _duration )
				.start();
		}


		if( GUILayout.Button( "Cicle Position Tween (relative with PingPong)" ) )
		{
			var spline = new Spline( "circle", true );
			spline.closePath();

			new SplineTween( quad, spline, _duration )
				.setIsRelative()
				.setLoops( LoopType.PingPong )
				.start();
		}


		if( GUILayout.Button( "DemoRoute Tween (relative with PingPong)" ) )
		{
			var spline = new Spline( "demoRoute", true );
			spline.closePath();

			new SplineTween( quad, spline, _duration )
				.setIsRelative()
				.setLoops( LoopType.PingPong )
				.start();
		}


		if( GUILayout.Button( "Runtime Spline (relative with PingPong)" ) )
		{
			var nodes = new Vector3[] { new Vector3( 0, 0 ), new Vector3( 0, 0 ), new Vector3( 4f, 4f, 4f ), new Vector3( -4f, 5f, 6f ), new Vector3( -2f, 2f, 0f ), new Vector3( 0f, 0f ) };
			var spline = new Spline( nodes );
			spline.closePath();

			// setup the tween
			new SplineTween( quad, spline, _duration )
				.setIsRelative()
				.setLoops( LoopType.PingPong )
				.start();
		}

	}
}
