using UnityEngine;
using System;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	/// <summary>
	/// helper class for managing a series of simultaneous tweens. An important item to note here is that the delay,
	/// loop values and ease/animation curve should be set on the TweenParty and that the sub-tweens must have the
	/// same duration as the TweenParty. TweenParty will force reset delay, loops, duration and ease type of all subtweens.
	/// 
	/// We piggyback on a FloatTween here and use the float value to tween all of our sub-tweens.
	/// </summary>
	public class TweenParty : FloatTween, ITweenTarget<float>
	{
		public int totalTweens { get { return _tweenList.Count; } }
		public float currentElapsedTime { get; private set; }

		List<ITweenControl> _tweenList = new List<ITweenControl>();


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
			currentElapsedTime = value;
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].jumpToElapsedTime( value );
		}


		public float getTweenedValue()
		{
			return currentElapsedTime;
		}


		public new object getTargetObject()
		{
			return null;
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
						((ITween<int>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<float> )
						((ITween<float>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Vector2> )
						((ITween<Vector2>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Vector3> )
						((ITween<Vector3>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Vector4> )
						((ITween<Vector4>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Quaternion> )
						((ITween<Quaternion>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Color> )
						((ITween<Color>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );
					else if( _tweenList[i] is ITween<Color32> )
						((ITween<Color32>)_tweenList[i]).setDelay( 0 ).setLoops( LoopType.None ).setDuration( _duration ).setEaseType( _easeType );

					_tweenList[i].start();
				}

				ZestKit.instance.addTween( this );
			}
		}


		public override void recycleSelf()
		{
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].recycleSelf();
			_tweenList.Clear();
		}

		#endregion


		#region TweenParty management

		public TweenParty addTween( ITweenControl tween )
		{
			tween.resume();
			_tweenList.Add( tween );

			return this;
		}


		/// <summary>
		/// Prepare TweenParty for reuse. This recycles sub-tweens so use setRecycleTween(false) on any sub-tweens you want to reuse.
		/// </summary>
		/// <param name="duration">Duration.</param>
		public TweenParty prepareForReuse( float duration )
		{
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].recycleSelf();
			_tweenList.Clear();

			return (TweenParty)prepareForReuse( 0f, duration, duration ); 
		}

		#endregion

	}
}