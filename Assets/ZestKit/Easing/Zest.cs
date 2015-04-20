using EaseFunction = System.Func<float, float, float>;
using UnityEngine;


namespace ZestKit
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


		public static Vector2 unclampedLerp( Vector2 from, Vector2 to, float t )
		{
			return new Vector2( from.x + ( to.x - from.x ) * t, from.y + ( to.y - from.y ) * t );
		}


		public static Vector3 unclampedLerp( Vector3 from, Vector3 to, float t )
		{
			return new Vector3( from.x + ( to.x - from.x ) * t, from.y + ( to.y - from.y ) * t, from.z + ( to.z - from.z ) * t );
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


		public static Color unclampedLerp( Color a, Color b, float t )
		{
			return new Color( a.r + ( b.r - a.r ) * t, a.g + ( b.g - a.g ) * t, a.b + ( b.b - a.b ) * t, a.a + ( b.a - a.a ) * t );
		}


		public static Color32 unclampedLerp( Color32 a, Color32 b, float t )
		{
			return new Color32( (byte)( (float)a.r + (float)( b.r - a.r ) * t ), (byte)( (float)a.g + (float)( b.g - a.g ) * t ), (byte)( (float)a.b + (float)( b.b - a.b ) * t ), (byte)( (float)a.a + (float)( b.a - a.a ) * t ) );
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

		#endregion

	}
}
