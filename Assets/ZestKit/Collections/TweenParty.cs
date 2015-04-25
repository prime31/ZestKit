using UnityEngine;
using System;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	/// <summary>
	/// helper class for managing a series of simultaneous tweens. An important item to note here is that the delay,
	/// loop values and ease/animation curve should be set on the TweenParty and that the sub-tweens must have the
	/// same duration as the TweenParty.
	/// 
	/// We piggyback on a FloatTween here and use the float value to tween all of our sub-tweens.
	/// </summary>
	public class TweenParty : FloatTween, ITweenTarget<float>
	{
		private List<ITweenable> _tweenList = new List<ITweenable>();


		public TweenParty( float duration )
		{
			_target = this;
			_duration = duration;
			_toValue = duration;
		}


		#region ITweenTarget

		/// <summary>
		/// value will be an already eased float between 0 and duration. We can just manually apply this to all our
		/// sub-tweens
		/// </summary>
		/// <param name="value">Value.</param>
		public void setTweenedValue( float value )
		{
			for( var i = 0; i < _tweenList.Count; i++ )
			{
				_tweenList[i].jumpToElapsedTime( value );
			}
		}

		#endregion


		#region ITweenControl

		public override void start()
		{
			if( _tweenState == TweenState.Complete )
			{
				_tweenState = TweenState.Running;

				// normalize all of our subtweens. this is gross but it helps alleviate user error
				for( var i = 0; i < _tweenList.Count; i++ )
				{
					if( _tweenList[i] is ITween<int> )
						((ITween<int>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<float> )
						((ITween<float>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Vector2> )
						((ITween<Vector2>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Vector3> )
						((ITween<Vector3>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Vector4> )
						((ITween<Vector4>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Quaternion> )
						((ITween<Quaternion>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Color> )
						((ITween<Color>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
					else if( _tweenList[i] is ITween<Color32> )
						((ITween<Color32>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration );
				}

				ZestKit.instance.addTween( this );
			}
		}

		#endregion


		#region ITweenable

		public override void recycleSelf()
		{
			if( !_shouldRecycleTween )
				return;
			
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].recycleSelf();
			_tweenList.Clear();
		}

		#endregion


		#region TweenParty management

		public TweenParty addTween( ITweenControl tween )
		{
			// make sure we have a legit ITweenable
			if( tween is ITweenable )
			{
				// resume gets the tween into Playing mode so we can tick it
				tween.resume();
				_tweenList.Add( tween as ITweenable );
			}
			else
			{
				Debug.LogError( "attempted to add a tween that does not implement ITweenable to a TweenParty!" );
			}

			return this;
		}

		#endregion

	}
}