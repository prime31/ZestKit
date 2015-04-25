using UnityEngine;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	/// <summary>
	/// this is a special case since Transforms are by far the most tweened object. we encapsulate the Tween and the ITweenTarget
	/// in a single, cacheable class
	/// </summary>
	public class TransformVector3Tween : Vector3Tween, ITweenTarget<Vector3>
	{
		#region Static caching

		private static Stack<TransformVector3Tween> _vectorTransformTweenStack = new Stack<TransformVector3Tween>( 10 );


		public static TransformVector3Tween nextAvailableTween()
		{
			if( _vectorTransformTweenStack.Count > 0 )
				return _vectorTransformTweenStack.Pop();

			return new TransformVector3Tween();
		}

		#endregion


		public enum TransformTargetType
		{
			Position,
			LocalPosition,
			LocalScale,
			EulerAngles,
			LocalEulerAngles
		}

		Transform _transform;
		TransformTargetType _targetType;


		public void setTweenedValue( Vector3 value )
		{
			// if the babysitter is enabled and we dont validate just silently do nothing
			if( ZestKit.enableBabysitter && !_transform )
				return;
			
			switch( _targetType )
			{
				case TransformTargetType.Position:
					_transform.position = value;
					break;
				case TransformTargetType.LocalPosition:
					_transform.localPosition = value;
					break;
				case TransformTargetType.LocalScale:
					_transform.localScale = value;
					break;
				case TransformTargetType.EulerAngles:
					_transform.eulerAngles = value;
					break;
				case TransformTargetType.LocalEulerAngles:
					_transform.localEulerAngles = value;
					break;
				default:
					throw new System.ArgumentOutOfRangeException();
			}
		}


		public void setTargetAndType( Transform transform, TransformTargetType targetType )
		{
			_transform = transform;
			_targetType = targetType;
		}


		protected override void updateValue()
		{
			// special case for non-relative angle lerps so that they take the shortest possible rotation
			if( ( _targetType == TransformTargetType.EulerAngles || _targetType == TransformTargetType.LocalEulerAngles ) && !_isRelative )
			{
				if( _animationCurve != null )
					setTweenedValue( Zest.easeAngle( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
				else
					setTweenedValue( Zest.easeAngle( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
			}
			else
			{
				if( _animationCurve != null )
					setTweenedValue( Zest.ease( _animationCurve, _fromValue, _toValue, _elapsedTime, _duration ) );
				else
					setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
			}
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween )
			{
				_transform = null;
				_vectorTransformTweenStack.Push( this );
			}
		}
	
	}
}