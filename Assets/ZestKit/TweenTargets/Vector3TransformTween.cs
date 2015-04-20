using UnityEngine;
using System.Collections.Generic;


namespace ZestKit
{
	/// <summary>
	/// this is a special case since Transforms are by far the most tweened object. we encapsulate the Tween and the ITweenTarget
	/// in a single, cacheable object
	/// </summary>
	public class Vector3TransformTween : Tween<Vector3>, ITweenTarget<Vector3>
	{
		#region Static caching

		private static Stack<Vector3TransformTween> _vectorTransformTweenStack = new Stack<Vector3TransformTween>( 5 );


		public static Vector3TransformTween nextAvailableTween()
		{
			if( _vectorTransformTweenStack.Count > 0 )
				return _vectorTransformTweenStack.Pop();

			return new Vector3TransformTween();
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
			// special case for angle lerps so that they take the shortest possible rotation
			if( _targetType == TransformTargetType.EulerAngles || _targetType == TransformTargetType.LocalEulerAngles )
				setTweenedValue( Zest.easeAngle( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
			else
				setTweenedValue( Zest.ease( _easeType, _fromValue, _toValue, _elapsedTime, _duration ) );
		}


		public override void recycleSelf()
		{
			base.recycleSelf();

			if( _shouldRecycleTween )
				_vectorTransformTweenStack.Push( this );
		}
	
	}
}