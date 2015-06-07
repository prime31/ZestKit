using EaseFunction = System.Func<float, float, float>;
using UnityEngine;


namespace Prime31.ZestKit
{
	/// <summary>
	/// series of static methods to handle all common tween type structs along with unclamped lerps for them.
	/// unclamped lerps are required for bounce, elastic or other tweens that exceed the 0 - 1 range. all easable
	/// structs can use either a standard ease equation (EaseType enum) or an AnimationCurve.
	/// </summary>
	public static class Zest
	{
		#region Lerps

		public static float unclampedLerp( float from, float to, float t )
		{
			return from + ( to - from ) * t;
		}
		
		
		// remainingFactorPerSecond is the percentage of the distance it covers every second. should be between 0 and 1.
		// if it's 0.25 it means it covers 75% of the remaining distance every second independent of the framerate
		public static float lerpTowards( float from, float to, float remainingFactorPerSecond, float deltaTime )
		{
			return unclampedLerp( from, to, 1f - Mathf.Pow( remainingFactorPerSecond, deltaTime ) );
		}


		public static Vector2 unclampedLerp( Vector2 from, Vector2 to, float t )
		{
			return new Vector2( from.x + ( to.x - from.x ) * t, from.y + ( to.y - from.y ) * t );
		}
		
		
		// remainingFactorPerSecond is the percentage of the distance it covers every second. should be between 0 and 1.
		// if it's 0.25 it means it covers 75% of the remaining distance every second independent of the framerate
		public static Vector2 lerpTowards( Vector2 from, Vector2 to, float remainingFactorPerSecond, float deltaTime )
		{
			return unclampedLerp( from, to, 1f - Mathf.Pow( remainingFactorPerSecond, deltaTime ) );
		}


		public static Vector3 unclampedLerp( Vector3 from, Vector3 to, float t )
		{
			return new Vector3( from.x + ( to.x - from.x ) * t, from.y + ( to.y - from.y ) * t, from.z + ( to.z - from.z ) * t );
		}
		
		
		// remainingFactorPerSecond is the percentage of the distance it covers every second. should be between 0 and 1.
		// if it's 0.25 it means it covers 75% of the remaining distance every second independent of the framerate
		public static Vector3 lerpTowards( Vector3 from, Vector3 to, float remainingFactorPerSecond, float deltaTime )
		{
			return unclampedLerp( from, to, 1f - Mathf.Pow( remainingFactorPerSecond, deltaTime ) );
		}
		
		
		// a different variant that requires the target details to calculate the lerp
		public static Vector3 lerpTowards( Vector3 followerCurrentPosition, Vector3 targetPreviousPosition, Vector3 targetCurrentPosition, float smoothFactor, float deltaTime )
	    {
			var targetDiff = targetCurrentPosition - targetPreviousPosition;
	        var temp = followerCurrentPosition - targetPreviousPosition + targetDiff / ( smoothFactor * deltaTime );
	        return targetCurrentPosition - targetDiff / ( smoothFactor * deltaTime ) + temp * Mathf.Exp( -smoothFactor * deltaTime );
	    }


		public static Vector3 unclampedAngledLerp( Vector3 from, Vector3 to, float t )
		{
			// we calculate the shortest difference between the angles for this lerp
			var toMinusFrom = new Vector3( Mathf.DeltaAngle( from.x, to.x ), Mathf.DeltaAngle( from.y, to.y ), Mathf.DeltaAngle( from.z, to.z ) );
			return new Vector3( from.x + toMinusFrom.x * t, from.y + toMinusFrom.y * t, from.z + toMinusFrom.z * t );
		}


		public static Vector4 unclampedLerp( Vector4 from, Vector4 to, float t )
		{
			return new Vector4( from.x + ( to.x - from.x ) * t, from.y + ( to.y - from.y ) * t, from.z + ( to.z - from.z ) * t, from.w + ( to.w - from.w ) * t );
		}


		public static Color unclampedLerp( Color from, Color to, float t )
		{
			return new Color( from.r + ( to.r - from.r ) * t, from.g + ( to.g - from.g ) * t, from.b + ( to.b - from.b ) * t, from.a + ( to.a - from.a ) * t );
		}


		public static Color32 unclampedLerp( Color32 from, Color32 to, float t )
		{
			return new Color32( (byte)( (float)from.r + (float)( to.r - from.r ) * t ), (byte)( (float)from.g + (float)( to.g - from.g ) * t ), (byte)( (float)from.b + (float)( to.b - from.b ) * t ), (byte)( (float)from.a + (float)( to.a - from.a ) * t ) );
		}


		public static Rect unclampedLerp( Rect from, Rect to, float t )
		{
			return new Rect
			(
				from.x + ( to.x - from.x ) * t,
				from.y + ( to.y - from.y ) * t,
				from.width + ( to.width - from.width ) * t,
				from.height + ( to.height - from.height ) * t
			);
		}

		#endregion


		#region Easers

		public static float ease( EaseType easeType, float from, float to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static float ease( AnimationCurve curve, float from, float to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Vector2 ease( EaseType easeType, Vector2 from, Vector2 to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Vector2 ease( AnimationCurve curve, Vector2 from, Vector2 to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Vector3 ease( EaseType easeType, Vector3 from, Vector3 to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Vector3 ease( AnimationCurve curve, Vector3 from, Vector3 to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Vector3 easeAngle( EaseType easeType, Vector3 from, Vector3 to, float t, float duration )
		{
			return unclampedAngledLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Vector3 easeAngle( AnimationCurve curve, Vector3 from, Vector3 to, float t, float duration )
		{
			return unclampedAngledLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Vector4 ease( EaseType easeType, Vector4 from, Vector4 to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Vector4 ease( AnimationCurve curve, Vector4 from, Vector4 to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Quaternion ease( EaseType easeType, Quaternion from, Quaternion to, float t, float duration )
		{
			return Quaternion.Lerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Quaternion ease( AnimationCurve curve, Quaternion from, Quaternion to, float t, float duration )
		{
			return Quaternion.Lerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Color ease( EaseType easeType, Color from, Color to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Color ease( AnimationCurve curve, Color from, Color to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Color32 ease( EaseType easeType, Color32 from, Color32 to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Color32 ease( AnimationCurve curve, Color32 from, Color32 to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}


		public static Rect ease( EaseType easeType, Rect from, Rect to, float t, float duration )
		{
			return unclampedLerp( from, to, EaseHelper.ease( easeType, t, duration ) );
		}


		public static Rect ease( AnimationCurve curve, Rect from, Rect to, float t, float duration )
		{
			return unclampedLerp( from, to, curve.Evaluate( t / duration ) );
		}

		#endregion


		#region Springs

		/// <summary>
		/// uses the semi-implicit euler method. faster, but not always stable.
		/// see http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
		/// </summary>
		/// <returns>The spring.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="targetValue">Target value.</param>
		/// <param name="velocity">Velocity by reference. Be sure to reset it to 0 if changing the targetValue between calls</param>
		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.</param>
		/// <param name="angularFrequency">An angular frequency of 2pi (radians per second) means the oscillation completes one
		/// full period over one second, i.e. 1Hz. should be less than 35 or so to remain stable</param>
		public static float fastSpring( float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency )
		{
			velocity += -2.0f * Time.deltaTime * dampingRatio * angularFrequency * velocity + Time.deltaTime * angularFrequency * angularFrequency * ( targetValue - currentValue );
			currentValue += Time.deltaTime * velocity;

			return currentValue;
		}


		/// <summary>
		/// uses the implicit euler method. slower, but always stable.
		/// see http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
		/// </summary>
		/// <returns>The spring.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="targetValue">Target value.</param>
		/// <param name="velocity">Velocity by reference. Be sure to reset it to 0 if changing the targetValue between calls</param>
		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.</param>
		/// <param name="angularFrequency">An angular frequency of 2pi (radians per second) means the oscillation completes one
		/// full period over one second, i.e. 1Hz. should be less than 35 or so to remain stable</param>
		public static float stableSpring( float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency )
		{
			var f = 1f + 2f * Time.deltaTime * dampingRatio * angularFrequency;
			var oo = angularFrequency * angularFrequency;
			var hoo = Time.deltaTime * oo;
			var hhoo = Time.deltaTime * hoo;
			var detInv = 1.0f / ( f + hhoo );
			var detX = f * currentValue + Time.deltaTime * velocity + hhoo * targetValue;
			var detV = velocity + hoo * ( targetValue - currentValue );

			currentValue = detX * detInv;
			velocity = detV * detInv;

			return currentValue;
		}


		/// <summary>
		/// uses the semi-implicit euler method. slower, but always stable.
		/// see http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
		/// </summary>
		/// <returns>The spring.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="targetValue">Target value.</param>
		/// <param name="velocity">Velocity by reference. Be sure to reset it to 0 if changing the targetValue between calls</param>
		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.</param>
		/// <param name="angularFrequency">An angular frequency of 2pi (radians per second) means the oscillation completes one
		/// full period over one second, i.e. 1Hz. should be less than 35 or so to remain stable</param>
		public static Vector3 fastSpring( Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float dampingRatio, float angularFrequency )
		{
			velocity += -2.0f * Time.deltaTime * dampingRatio * angularFrequency * velocity + Time.deltaTime * angularFrequency * angularFrequency * ( targetValue - currentValue );
			currentValue += Time.deltaTime * velocity;

			return currentValue;
		}


		/// <summary>
		/// uses the implicit euler method. faster, but not always stable.
		/// see http://allenchou.net/2015/04/game-math-more-on-numeric-springing/
		/// </summary>
		/// <returns>The spring.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="targetValue">Target value.</param>
		/// <param name="velocity">Velocity by reference. Be sure to reset it to 0 if changing the targetValue between calls</param>
		/// <param name="dampingRatio">lower values are less damped and higher values are more damped resulting in less springiness.
		/// should be between 0.01f, 1f to avoid unstable systems.</param>
		/// <param name="angularFrequency">An angular frequency of 2pi (radians per second) means the oscillation completes one
		/// full period over one second, i.e. 1Hz. should be less than 35 or so to remain stable</param>
		public static Vector3 stableSpring( Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float dampingRatio, float angularFrequency )
		{
			var f = 1f + 2f * Time.deltaTime * dampingRatio * angularFrequency;
			var oo = angularFrequency * angularFrequency;
			var hoo = Time.deltaTime * oo;
			var hhoo = Time.deltaTime * hoo;
			var detInv = 1.0f / ( f + hhoo );
			var detX = f * currentValue + Time.deltaTime * velocity + hhoo * targetValue;
			var detV = velocity + hoo * ( targetValue - currentValue );

			currentValue = detX * detInv;
			velocity = detV * detInv;

			return currentValue;
		}

		#endregion

	}
}
