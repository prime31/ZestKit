using UnityEngine;
using System;
using System.Collections;


namespace ZestKit
{
	/// <summary>
	/// a series of strongly typed, chainable methods to setup various tween properties
	/// </summary>
	public interface ITween<T> : ITweenControl where T : struct
	{
		/// <summary>
		/// sets the ease type used for this tween
		/// </summary>
		/// <returns>The ease type.</returns>
		/// <param name="easeType">Ease type.</param>
		ITween<T> setEaseType( EaseType easeType );

		/// <summary>
		/// if an AnimationCurve is set it will be used instead of the EaseType
		/// </summary>
		/// <returns>The animation curve.</returns>
		/// <param name="animationCurve">Animation curve.</param>
		ITween<T> setAnimationCurve( AnimationCurve animationCurve );

		/// <summary>
		/// sets the delay before starting the tween
		/// </summary>
		/// <returns>The delay.</returns>
		/// <param name="delay">Delay.</param>
		ITween<T> setDelay( float delay );

		/// <summary>
		/// sets the tween duration
		/// </summary>
		/// <returns>The duration.</returns>
		/// <param name="duration">Duration.</param>
		ITween<T> setDuration( float duration );

		/// <summary>
		/// sets the tween to use Time.unscaledDeltaTime instead of Time.deltaTime
		/// </summary>
		/// <returns>The is time scale independant.</returns>
		ITween<T> setIsTimeScaleIndependant();

		/// <summary>
		/// chainable. sets the action that should be called when the tween is complete.
		/// </summary>
		ITween<T> setCompletionHandler( Action<ITween<T>> completionHandler );

		/// <summary>
		/// chainable. set the loop type for the tween. a single pingpong loop means going from start-finish-start.
		/// </summary>
		ITween<T> setLoops( LoopType loopType, int loops = 1, float delayBetweenLoops = 0f );

		/// <summary>
		/// chainable. sets the action that should be called when a loop is complete. A loop is either when the first part of
		/// a ping-pong animation completes or when starting over when using a restart-from-beginning loop type. Note that ping-pong
		/// loops (which are really two part tweens) will not fire the loop completion handler on the last iteration. The normal
		/// tween completion handler will fire though
		/// </summary>
		ITween<T> setLoopCompletionHandler( Action<ITween<T>> loopCompleteHandler );

		/// <summary>
		/// sets the start position for the tween
		/// </summary>
		/// <returns>The from.</returns>
		/// <param name="from">From.</param>
		ITween<T> setFrom( T from );

		/// <summary>
		/// prepares a tween for reuse by resetting its from/to values and duration
		/// </summary>
		/// <returns>The for reuse.</returns>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		ITween<T> prepareForReuse( T from, T to, float duration );

		/// <summary>
		/// if true (the default) the tween will be recycled after use. Currently, only the most commonly used
		/// tweens are recycled.
		/// </summary>
		/// <returns>The recycle tween.</returns>
		/// <param name="shouldRecycleTween">If set to <c>true</c> should recycle tween.</param>
		ITween<T> setRecycleTween( bool shouldRecycleTween );

		/// <summary>
		/// helper that just sets the to value of the tween to be to + from making the tween relative
		/// to it's current value.
		/// </summary>
		/// <returns>The is relative tween.</returns>
		ITween<T> setIsRelative();
	}


	/// <summary>
	/// standard tween playback controls
	/// </summary>
	public interface ITweenControl
	{
		/// <summary>
		/// checks to see if a tween is running
		/// </summary>
		/// <returns><c>true</c>, if running was ised, <c>false</c> otherwise.</returns>
		bool isRunning();

		/// <summary>
		/// starts the tween
		/// </summary>
		void start();

		/// <summary>
		/// pauses the tween
		/// </summary>
		void pause();

		/// <summary>
		/// resumes the tween after a pause
		/// </summary>
		void resume();

		/// <summary>
		/// stops the tween optionally bringing it to completion
		/// </summary>
		/// <param name="bringToCompletion">If set to <c>true</c> bring to completion.</param>
		void stop( bool bringToCompletion = false );

		/// <summary>
		/// warps the tween to elapsedTime clamping it between 0 and duration. this will immediately update the tweened
		/// object whether it is paused, completed or running.
		/// </summary>
		/// <param name="elapsedTime">Elapsed time.</param>
		void jumpToElapsedTime( float elapsedTime );

		/// <summary>
		/// when called from StartCoroutine it will yield until the tween is complete
		/// </summary>
		/// <returns>The for completion.</returns>
		IEnumerator waitForCompletion();
	}
	

	/// <summary>
	/// all of the methods needed to control a tween. Any non-tweens that want to take part in ZestKit can implement this.
	/// The TweenChain is a good example of how it can be used.
	/// </summary>
	public interface ITweenable : ITweenControl
	{
		/// <summary>
		/// called by ZestKit each frame like an internal Update
		/// </summary>
		bool tick();

		/// <summary>
		/// called by ZestKit when a tween is removed. Subclasses can optionally recycle themself. Subclasses
		/// should first check the _shouldRecycleTween bool in their implementation!
		/// </summary>
		void recycleSelf();
	}


	/// <summary>
	/// any object that wants to be tweened needs to implement this. ZestKit internally likes to make a simple object
	/// that implements this interface and stores a reference to the object being tweened. That makes for tiny, simple,
	/// lightweight implementations that can be handed off to any TweenT
	/// </summary>
	public interface ITweenTarget<T> where T : struct
	{
		/// <summary>
		/// sets the final, tweened value on the object of your choosing.
		/// </summary>
		/// <param name="value">Value.</param>
		void setTweenedValue( T value );
	}
}
