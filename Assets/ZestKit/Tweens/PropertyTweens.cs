using UnityEngine;
using System;
using System.Collections;
using System.Reflection;


namespace Prime31.ZestKit
{
	/// <summary>
	/// helper class to fetch property delegates
	/// </summary>
	class PropertyTweenUtils
	{
		/// <summary>
		/// either returns a super fast Delegate to set the given property or null if it couldn't be found
		/// via reflection
		/// </summary>
		public static T setterForProperty<T>( System.Object targetObject, string propertyName )
		{
			// first get the property
#if NETFX_CORE
			var propInfo = targetObject.GetType().GetRuntimeProperty( propertyName );
#else
			var propInfo = targetObject.GetType().GetProperty( propertyName );
#endif

			if( propInfo == null )
			{
				Debug.Log( "could not find property with name: " + propertyName );
				return default( T );
			}

#if NETFX_CORE
			// Windows Phone/Store new API
			return (T)(object)propInfo.SetMethod.CreateDelegate( typeof( T ), targetObject );
#else
			return (T)(object)Delegate.CreateDelegate( typeof( T ), targetObject, propInfo.GetSetMethod() );
#endif
		}


		/// <summary>
		/// either returns a super fast Delegate to get the given property or null if it couldn't be found
		/// via reflection
		/// </summary>
		public static T getterForProperty<T>( System.Object targetObject, string propertyName )
		{
			// first get the property
#if NETFX_CORE
			var propInfo = targetObject.GetType().GetRuntimeProperty( propertyName );
#else
			var propInfo = targetObject.GetType().GetProperty( propertyName );
#endif

			if( propInfo == null )
			{
				Debug.Log( "could not find property with name: " + propertyName );
				return default( T );
			}

#if NETFX_CORE
			// Windows Phone/Store new API
			return (T)(object)propInfo.GetMethod.CreateDelegate( typeof( T ), targetObject );
#else
			return (T)(object)Delegate.CreateDelegate( typeof( T ), targetObject, propInfo.GetGetMethod() );
#endif
		}

	}


	public class PropertyTarget<T> : ITweenTarget<T> where T : struct
	{
		protected object _target;
		protected Action<T> _setter;
		protected Func<T> _getter;


		public void setTweenedValue( T value )
		{
			_setter( value );
		}


		public T getTweenedValue()
		{
			return _getter();
		}


		public PropertyTarget( object target, string propertyName )
		{
			_target = target;
			_setter = PropertyTweenUtils.setterForProperty<Action<T>>( target, propertyName );
			_getter = PropertyTweenUtils.getterForProperty<Func<T>>( target, propertyName );

			if( _setter == null )
				Debug.LogError( "either the property (" + propertyName + ") setter or getter could not be found on the object " + target );
		}


		public object getTargetObject()
		{
			return _target;
		}
	}


	public static class PropertyTweens
	{
		public static ITween<int> intPropertyTo( object self, string propertyName, int to, float duration )
		{
			var tweenTarget = new PropertyTarget<int>( self, propertyName );
			var tween = ZestKit.cacheIntTweens ? QuickCache<IntTween>.pop() : new IntTween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<float> floatPropertyTo( object self, string propertyName, float to, float duration )
		{
			var tweenTarget = new PropertyTarget<float>( self, propertyName );

			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Vector2> vector2PropertyTo( object self, string propertyName, Vector2 to, float duration )
		{
			var tweenTarget = new PropertyTarget<Vector2>( self, propertyName );
			var tween = ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Vector3> vector3PropertyTo( object self, string propertyName, Vector3 to, float duration )
		{
			var tweenTarget = new PropertyTarget<Vector3>( self, propertyName );
			var tween = ZestKit.cacheVector3Tweens ? QuickCache<Vector3Tween>.pop() : new Vector3Tween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Vector4> vector4PropertyTo( object self, string propertyName, Vector4 to, float duration )
		{
			var tweenTarget = new PropertyTarget<Vector4>( self, propertyName );
			var tween = ZestKit.cacheVector4Tweens ? QuickCache<Vector4Tween>.pop() : new Vector4Tween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Quaternion> quaternionPropertyTo( object self, string propertyName, Quaternion to, float duration )
		{
			var tweenTarget = new PropertyTarget<Quaternion>( self, propertyName );
			var tween = ZestKit.cacheQuaternionTweens ? QuickCache<QuaternionTween>.pop() : new QuaternionTween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Color> colorPropertyTo( object self, string propertyName, Color to, float duration )
		{
			var tweenTarget = new PropertyTarget<Color>( self, propertyName );
			var tween = ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}


		public static ITween<Color32> color32PropertyTo( object self, string propertyName, Color32 to, float duration )
		{
			var tweenTarget = new PropertyTarget<Color32>( self, propertyName );
			var tween = ZestKit.cacheColor32Tweens ? QuickCache<Color32Tween>.pop() : new Color32Tween();
			tween.initialize( tweenTarget, to, duration );

			return tween;
		}
	}
		
}
