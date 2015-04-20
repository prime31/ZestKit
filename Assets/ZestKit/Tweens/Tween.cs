using UnityEngine;
using System;
using System.Collections.Generic;


namespace ZestKit
{
	public enum LoopType
	{
		None,
		RestartFromBeginning,
		PingPong
	}


	public abstract class Tween<T> : ITweenable, ITween<T> where T : struct
	{
		enum TweenState
		{
			Running,
			Paused,
			Complete
		}


		protected ITweenTarget<T> _target;
		protected T _fromValue;
		protected T _toValue;
		protected EaseType _easeType;
		protected AnimationCurve _animationCurve;
		protected bool _shouldRecycleTween = true;
		protected Action<ITween<T>> _completionHandler;
		protected Action<ITween<T>> _loopCompleteHandler;

		// tween state
		TweenState _tweenState = TweenState.Complete;
		bool _isTimeScaleIndependent;
		protected float _delay;
		protected float _duration;
		protected float _elapsedTime;
		
		// loop state
		protected LoopType _loopType;
		protected int _loops;
		protected float _delayBetweenLoops;
		bool _isRunningInReverse;


		#region ITweenT implementation

		public ITween<T> setEaseType( EaseType easeType )
		{
			_easeType = easeType;
			return this;
		}


		public ITween<T> setAnimationCurve( AnimationCurve animationCurve )
		{
			_animationCurve = animationCurve;
			return this;
		}


		public ITween<T> setDelay( float delay )
		{
			_delay = delay;
			_elapsedTime = -_delay;
			return this;
		}


		public ITween<T> setIsTimeScaleIndependant()
		{
			_isTimeScaleIndependent = true;
			return this;
		}
		
		
		/// <summary>
		/// chainable. sets the action that should be called when the tween is complete.
		/// </summary>
		public ITween<T> setCompletionHandler( Action<ITween<T>> completionHandler )
		{
			_completionHandler = completionHandler;
			return this;
		}
		
		
		/// <summary>
		/// chainable. set the loop type for the tween. a single pingpong loop means going from start-finish-start.
		/// </summary>
		public ITween<T> setLoops( LoopType loopType, int loops = 1, float delayBetweenLoops = 0f )
		{
			_loopType = loopType;
			_delayBetweenLoops = delayBetweenLoops;

			// double the loop count for ping-pong
			if( loopType == LoopType.PingPong )
				loops = loops * 2;
			_loops = loops;

			return this;
		}
		
		
		/// <summary>
		/// chainable. sets the action that should be called when a loop is complete. A loop is either when the first part of
		/// a ping-pong animation completes or when starting over when using a restart-from-beginning loop type. Note that ping-pong
		/// loops (which are really two part tweens) will not fire the loop completion handler on the last iteration. The normal
		/// tween completion handler will fire though
		/// </summary>
		public ITween<T> setLoopCompletionHandler( Action<ITween<T>> loopCompleteHandler )
		{
			_loopCompleteHandler = loopCompleteHandler;
			return this;
		}


		public ITween<T> setFrom( T from )
		{
			_fromValue = from;
			return this;
		}


		public ITween<T> prepareForReuse( T from, T to, float duration )
		{
			initialize( _target, from, to, duration );
			return this;
		}


		public ITween<T> setRecycleTween( bool shouldRecycleTween )
		{
			_shouldRecycleTween = shouldRecycleTween;
			return this;
		}


		public bool isRunning()
		{
			return _tweenState == TweenState.Running;
		}


		public void start()
		{
			if( _tweenState == TweenState.Complete )
			{
				_tweenState = TweenState.Running;
				ZestKit.instance.addTween( this );
			}
		}


		public void pause()
		{
			_tweenState = TweenState.Paused;
		}


		public void resume()
		{
			_tweenState = TweenState.Running;
		}


		public void stop( bool bringToCompletion )
		{
			_tweenState = TweenState.Complete;
			_elapsedTime = _duration;
			tick();
		}
		
		
		/// <summary>
		/// reverses the current tween. if it was going forward it will be going backwards and vice versa.
		/// </summary>
		public void reverseTween()
		{
			_isRunningInReverse = !_isRunningInReverse;
		}

		#endregion


		void resetState()
		{
			_completionHandler = _loopCompleteHandler = null;
			_isTimeScaleIndependent = false;
			_tweenState = TweenState.Complete;
			_shouldRecycleTween = true;
			_easeType = ZestKit.defaultEaseType;
			_animationCurve = null;
			
			_delay = 0f;
			_duration = 0f;
			_elapsedTime = 0f;
			_loopType = LoopType.None;
			_delayBetweenLoops = 0f;
			_loops = 0;
			_isRunningInReverse = false;
		}


		public void initialize( ITweenTarget<T> target, T from, T to, float duration )
		{
			resetState();

			_target = target;
			_fromValue = from;
			_toValue = to;
			_duration = duration;
		}


		#region ITweenInternal

		public bool tick()
		{
			if( _tweenState == TweenState.Paused )
				return false;
			
			if( !_isRunningInReverse && _elapsedTime >= _duration )
			{
				_elapsedTime = _duration;
				_tweenState = TweenState.Complete;
			}
			else if( _isRunningInReverse && _elapsedTime <= 0 )
			{
				_elapsedTime = 0f;
				_tweenState = TweenState.Complete;
			}

			// elapsed time will be negative while we are delaying the start of the tween so dont update the value
			if( _elapsedTime >= 0 && _elapsedTime <= _duration )
				updateValue();

			var deltaTime = _isTimeScaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;

			// running in reverse? then we need to subtract deltaTime
			if( _isRunningInReverse )
				_elapsedTime -= deltaTime;
			else
				_elapsedTime += deltaTime;
			
			// if we have a loopType and we are done do the loop
			if( _loopType != LoopType.None && _tweenState == TweenState.Complete )
				handleLooping();

			if( _tweenState == TweenState.Complete )
			{
				if( _completionHandler != null )
					_completionHandler( this );

				return true;
			}

			return false;
		}


		public virtual void recycleSelf()
		{
			if( _shouldRecycleTween )
				_target = null;
		}
			
		#endregion

		
		/// <summary>
		/// handles loop logic
		/// </summary>
		void handleLooping()
		{
			_loops--;
			if( _loopType == LoopType.PingPong )
			{
				reverseTween();
			}

			if( _loopType == LoopType.RestartFromBeginning || _loops % 2 == 1 )
			{
				if( _loopCompleteHandler != null )
					_loopCompleteHandler( this );
			}

			// kill our loop if we have no loops left and zero out the delay then prepare for use
			if( _loops == 0 )
				_loopType = LoopType.None;
			else
				_tweenState = TweenState.Running;

			_delay = _delayBetweenLoops;

			if( _loopType == LoopType.RestartFromBeginning )
			{
				_elapsedTime = -_delay;
			}
			else
			{
				if( _isRunningInReverse )
					_elapsedTime += _delay;
				else
					_elapsedTime = -_delay;
			}
		}
			

		abstract protected void updateValue();

	}
}
