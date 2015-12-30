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
		public static IntTween create()
		{
			return ZestKit.cacheIntTweens ? QuickCache<IntTween>.pop() : new IntTween();
		}


		public IntTween()
		{}


		public IntTween( ITweenTarget<int> target, int to, float duration )
		{
			initialize( target, to, duration );
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
		public static FloatTween create()
		{
			return ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
		}


		public FloatTween()
		{}


		public FloatTween( ITweenTarget<float> target, float from, float to, float duration )
		{
			initialize( target, to, duration );
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
		public static Vector2Tween create()
		{
			return ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
		}


		public Vector2Tween()
		{}


		public Vector2Tween( ITweenTarget<Vector2> target, Vector2 from, Vector2 to, float duration )
		{
			initialize( target, to, duration );
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
		public static Vector3Tween create()
		{
			return ZestKit.cacheVector3Tweens ? QuickCache<Vector3Tween>.pop() : new Vector3Tween();
		}


		public Vector3Tween()
		{}


		public Vector3Tween( ITweenTarget<Vector3> target, Vector3 from, Vector3 to, float duration )
		{
			initialize( target, to, duration );
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
		public static Vector4Tween create()
		{
			return ZestKit.cacheVector4Tweens ? QuickCache<Vector4Tween>.pop() : new Vector4Tween();
		}


		public Vector4Tween()
		{}


		public Vector4Tween( ITweenTarget<Vector4> target, Vector4 from, Vector4 to, float duration )
		{
			initialize( target, to, duration );
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
		public static QuaternionTween create()
		{
			return ZestKit.cacheQuaternionTweens ? QuickCache<QuaternionTween>.pop() : new QuaternionTween();
		}


		public QuaternionTween()
		{}


		public QuaternionTween( ITweenTarget<Quaternion> target, Quaternion from, Quaternion to, float duration )
		{
			initialize( target, to, duration );
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
		public static ColorTween create()
		{
			return ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
		}


		public ColorTween()
		{}


		public ColorTween( ITweenTarget<Color> target, Color from, Color to, float duration )
		{
			initialize( target, to, duration );
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
		public static Color32Tween create()
		{
			return ZestKit.cacheColor32Tweens ? QuickCache<Color32Tween>.pop() : new Color32Tween();
		}


		public Color32Tween()
		{}


		public Color32Tween( ITweenTarget<Color32> target, Color32 from, Color32 to, float duration )
		{
			initialize( target, to, duration );
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


	public class RectTween : Tween<Rect>
	{
		public static RectTween create()
		{
			return ZestKit.cacheRectTweens ? QuickCache<RectTween>.pop() : new RectTween();
		}


		public RectTween()
		{}


		public RectTween( ITweenTarget<Rect> target, Rect from, Rect to, float duration )
		{
			initialize( target, to, duration );
		}


		public override ITween<Rect> setIsRelative()
		{
			_isRelative = true;
			_toValue = new Rect
			(
				_toValue.x + _fromValue.x,
				_toValue.y + _fromValue.y,
				_toValue.width + _fromValue.width,
				_toValue.height + _fromValue.height
			);

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

			if( _shouldRecycleTween && ZestKit.cacheRectTweens )
				QuickCache<RectTween>.push( this );
		}
	}

}
