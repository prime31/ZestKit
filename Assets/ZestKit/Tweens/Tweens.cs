using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// concrete implementations of all tweenable types
/// </summary>
namespace Prime31.ZestKit
{
	public class IntTween : Tween<int>
	{
		public IntTween()
		{}


		public IntTween( ITweenTarget<int> target, int from, int to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<int> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( (int)Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( (int)Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheIntTweens )
				QuickCache<IntTween>.push( this );
		}
	}


	public class FloatTween : Tween<float>
	{
		public FloatTween()
		{}


		public FloatTween( ITweenTarget<float> target, float from, float to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<float> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheFloatTweens )
				QuickCache<FloatTween>.push( this );
		}
	}


	public class Vector2Tween : Tween<Vector2>
	{
		public Vector2Tween()
		{}


		public Vector2Tween( ITweenTarget<Vector2> target, Vector2 from, Vector2 to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Vector2> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheVector2Tweens )
				QuickCache<Vector2Tween>.push( this );
		}
	}


	public class Vector3Tween : Tween<Vector3>
	{
		public Vector3Tween()
		{}


		public Vector3Tween( ITweenTarget<Vector3> target, Vector3 from, Vector3 to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Vector3> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheVector3Tweens )
				QuickCache<Vector3Tween>.push( this );
		}
	}


	public class Vector4Tween : Tween<Vector4>
	{
		public Vector4Tween()
		{}


		public Vector4Tween( ITweenTarget<Vector4> target, Vector4 from, Vector4 to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Vector4> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheVector4Tweens )
				QuickCache<Vector4Tween>.push( this );
		}
	}


	public class QuaternionTween : Tween<Quaternion>
	{
		public QuaternionTween()
		{}


		public QuaternionTween( ITweenTarget<Quaternion> target, Quaternion from, Quaternion to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Quaternion> setIsRelative()
		{
			_isRelative = true;
			_toValue *= _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheQuaternionTweens )
				QuickCache<QuaternionTween>.push( this );
		}
	}


	public class ColorTween : Tween<Color>
	{
		public ColorTween()
		{}


		public ColorTween( ITweenTarget<Color> target, Color from, Color to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Color> setIsRelative()
		{
			_isRelative = true;
			_toValue += _fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheColorTweens )
				QuickCache<ColorTween>.push( this );
		}
	}


	public class Color32Tween : Tween<Color32>
	{
		public Color32Tween()
		{}


		public Color32Tween( ITweenTarget<Color32> target, Color32 from, Color32 to, float duration )
		{
			initialize( target, from, to, duration );
		}


		public override ITween<Color32> setIsRelative()
		{
			_isRelative = true;
			_toValue = (Color)_toValue + (Color)_fromValue;
			return this;
		}


		protected override void updateValue()
		{
			if( _animationCurve != null )
				_target.setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				_target.setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween && ZestKit.cacheColor32Tweens )
				QuickCache<Color32Tween>.push( this );
		}
	}

}
