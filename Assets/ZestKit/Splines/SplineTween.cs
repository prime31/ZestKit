using UnityEngine;
using System.Collections;


namespace Prime31.ZestKit
{
	public class SplineTween : Tween<Vector3>, ITweenTarget<Vector3>
	{
		Transform _transform;
		Spline _spline;
		bool _isRelativeTween;


		public SplineTween( Transform transform, Spline spline, float duration )
		{
			_transform = transform;
			_spline = spline;
			_spline.buildPath();

			initialize( this, Vector3.zero, duration );
		}


		public void setTweenedValue( Vector3 value )
		{
            // if the babysitter is enabled and we dont validate just silently do nothing
            if( ZestKit.enableBabysitter && !_transform )
                return;
            
			_transform.position = value;
		}


		public Vector3 getTweenedValue()
		{
			return _transform.position;
		}


		public override ITween<Vector3> setIsRelative()
		{
			_isRelativeTween = true;
			return this;
		}


		protected override void updateValue()
		{
			var easedTime = EaseHelper.ease( _easeType, _elapsedTime, _duration );
			var position = _spline.getPointOnPath( easedTime );

			// if this is a relative tween we use the fromValue (initial position) as a base and add the spline to it
			if( _isRelativeTween )
				position += _fromValue;

			setTweenedValue( position );
		}


		public new object getTargetObject()
		{
			return _transform;
		}
	}
}