using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZestKit
{
	public class IntTween : Tween<int>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( (int)Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( (int)Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class FloatTween : Tween<float>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class Vector2Tween : Tween<Vector2>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class Vector3Tween : Tween<Vector3>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class Vector4Tween : Tween<Vector4>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class QuaternionTween : Tween<Quaternion>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class ColorTween : Tween<Color>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}


	public class Color32Tween : Tween<Color32>
	{
		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}
	}

}
