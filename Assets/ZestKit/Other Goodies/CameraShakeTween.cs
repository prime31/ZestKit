using UnityEngine;
using System.Collections;


namespace Prime31.ZestKit
{
	public class CameraShakeTween : AbstractTweenable
	{
		private Transform _cameraTransform;
		private Vector3 _shakeDirection = Vector3.zero;
		private Vector3 _shakeOffset = Vector3.zero;
		private float _shakeIntensity = 0.3f;
		private float _shakeDegredation = 0.95f;


		/// <summary>
		/// adds an offset to the cameras position each frame. shakeIntensity is degraded by shadeDegradation each frame until it nears 0.
		/// Once it reaches 0 the tween is removed from ZestKit. You can reuse it at anytime by just calling the shake method.
		/// 
		/// </summary>
		/// <param name="camera">the Camera to shake</param>
		/// <param name="shakeIntensity">how much should we shake it</param>
		/// <param name="shakeDegredation">higher values cause faster degradation</param>
		/// <param name="shakeDirection">Vector3.zero will result in a shake on just the x/y axis. any other values will result in the passed
		/// in shakeDirection * intensity being the offset the camera is moved</param>
		public CameraShakeTween( Camera camera, float shakeIntensity = 0.3f, float shakeDegredation = 0.95f, Vector3 shakeDirection = default( Vector3 ) )
		{
			_cameraTransform = camera.transform;
			_shakeIntensity = shakeIntensity;
			_shakeDegredation = shakeDegredation;
			_shakeDirection = shakeDirection.normalized;
		}


		/// <summary>
		/// if the shake is already running this will overwrite the current values only if shakeIntensity > the current shakeIntensity.
		/// if the shake is not currently active it will be started.
		/// </summary>
		/// <param name="shakeIntensity">how much should we shake it</param>
		/// <param name="shakeDegredation">higher values cause faster degradation</param>
		/// <param name="shakeDirection">Vector3.zero will result in a shake on just the x/y axis. any other values will result in the passed
		/// in shakeDirection * intensity being the offset the camera is moved</param>
		public void shake( float shakeIntensity = 0.3f, float shakeDegredation = 0.95f, Vector3 shakeDirection = default( Vector3 ) )
		{
			// guard against adding a weaker shake to an already running shake
			if( !_isCurrentlyManagedByZestKit || _shakeIntensity < shakeIntensity )
			{
				_shakeDirection = shakeDirection.normalized;
				_shakeIntensity = shakeIntensity;
				if( shakeDegredation < 0f || shakeDegredation >= 1f )
					shakeDegredation = 0.98f;

				_shakeDegredation = shakeDegredation;
			}

			if( !_isCurrentlyManagedByZestKit )
				start();
		}


		#region AbstractTweenable

		public override bool tick()
		{
			if( _isPaused )
				return false;

			if( Mathf.Abs( _shakeIntensity ) > 0f )
			{
				_shakeOffset = _shakeDirection;
				if( _shakeOffset != Vector3.zero )
				{
					_shakeOffset.Normalize();
				}
				else
				{
					_shakeOffset.x += Random.Range( 0f, 1f ) - 0.5f;
					_shakeOffset.y += Random.Range( 0f, 1f ) - 0.5f;
				}

				_shakeOffset *= _shakeIntensity;
				_shakeIntensity *= -_shakeDegredation;
				if( Mathf.Abs( _shakeIntensity ) <= 0.01f )
					_shakeIntensity = 0f;

				_cameraTransform.position += _shakeOffset;

				return false;
			}

			_isCurrentlyManagedByZestKit = false;
			return true;
		}

		#endregion
		
	}
}
