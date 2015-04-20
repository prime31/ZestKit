using UnityEngine;
using System.Collections;


namespace ZestKit
{
	/// <summary>
	/// this class adds extension methods for the most commonly used tweens
	/// </summary>
	public static class ZestKitExtensions
	{
		#region Transform tweens

		public static ITween<Vector3> positionTo( this Transform self, Vector3 to, float duration )
		{
			var tween = Vector3TransformTween.nextAvailableTween();
			tween.setTargetAndType( self, Vector3TransformTween.TransformTargetType.Position );
			tween.initialize( tween, self.position, to, duration );

			return tween;
		}


		public static ITween<Vector3> localPositionTo( this Transform self, Vector3 to, float duration )
		{
			var tween = Vector3TransformTween.nextAvailableTween();
			tween.setTargetAndType( self, Vector3TransformTween.TransformTargetType.LocalPosition );
			tween.initialize( tween, self.localPosition, to, duration );

			return tween;
		}


		public static ITween<Vector3> localScaleTo( this Transform self, Vector3 to, float duration )
		{
			var tween = Vector3TransformTween.nextAvailableTween();
			tween.setTargetAndType( self, Vector3TransformTween.TransformTargetType.LocalScale );
			tween.initialize( tween, self.localScale, to, duration );

			return tween;
		}


		public static ITween<Vector3> eulersTo( this Transform self, Vector3 to, float duration )
		{
			var tween = Vector3TransformTween.nextAvailableTween();
			tween.setTargetAndType( self, Vector3TransformTween.TransformTargetType.EulerAngles );
			tween.initialize( tween, self.eulerAngles, to, duration );

			return tween;
		}


		public static ITween<Vector3> localEulersTo( this Transform self, Vector3 to, float duration )
		{
			var tween = Vector3TransformTween.nextAvailableTween();
			tween.setTargetAndType( self, Vector3TransformTween.TransformTargetType.LocalEulerAngles );
			tween.initialize( tween, self.localEulerAngles, to, duration );

			return tween;
		}


		public static ITween<Quaternion> rotationTo( this Transform self, Quaternion to, float duration )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.Rotation );
			var tween = new QuaternionTween();
			tween.initialize( tweenTarget, self.rotation, to, duration );

			return tween;
		}


		public static ITween<Quaternion> localRotationTo( this Transform self, Quaternion to, float duration )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.LocalRotation );
			var tween = new QuaternionTween();
			tween.initialize( tweenTarget, self.localRotation, to, duration );

			return tween;
		}

		#endregion


		#region Material tweens

		public static ITween<Color> colorTo( this Material self, Color to, float duration, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialColorTarget();
			tweenTarget.prepareForUse( self, propertyName );

			var tween = new ColorTween();
			tween.initialize( tweenTarget, self.GetColor( propertyName ), to, duration );

			return tween;
		}


		public static ITween<float> alphaTo( this Material self, float to, float duration, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialAlphaTarget();
			tweenTarget.prepareForUse( self, propertyName );

			var tween = new FloatTween();
			tween.initialize( tweenTarget, self.GetColor( propertyName ).a, to, duration );

			return tween;
		}


		public static ITween<float> floatTo( this Material self, float to, float duration, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialFloatTarget();
			tweenTarget.prepareForUse( self, propertyName );

			var tween = new FloatTween();
			tween.initialize( tweenTarget, self.GetFloat( propertyName ), to, duration );

			return tween;
		}


		public static ITween<Vector4> floatTo( this Material self, Vector4 to, float duration, string propertyName )
		{
			var tweenTarget = new MaterialVector4Target();
			tweenTarget.prepareForUse( self, propertyName );

			var tween = new Vector4Tween();
			tween.initialize( tweenTarget, self.GetVector( propertyName ), to, duration );

			return tween;
		}

		#endregion

	}
}