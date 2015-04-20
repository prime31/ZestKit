using UnityEngine;
using System;


namespace ZestKit
{
	/// <summary>
	/// a series of strongly typed, chainable methods to setup various tween properties
	/// </summary>
	public interface ITween<T> : ITweenControl where T : struct
	{
		ITween<T> setEaseType( EaseType easeType );
		ITween<T> setAnimationCurve( AnimationCurve animationCurve );
		ITween<T> setDelay( float delay );
		ITween<T> setIsTimeScaleIndependant();
		ITween<T> setCompletionHandler( Action<ITween<T>> completionHandler );
		ITween<T> setLoops( LoopType loopType, int loops = 1, float delayBetweenLoops = 0f );
		ITween<T> setLoopCompletionHandler( Action<ITween<T>> loopCompleteHandler );
		ITween<T> setFrom( T from );
		ITween<T> prepareForReuse( T from, T to, float duration );
		ITween<T> setRecycleTween( bool shouldRecycleTween );
	}


	/// <summary>
	/// standard tween playback controls
	/// </summary>
	public interface ITweenControl
	{
		bool isRunning();
		void start();
		void pause();
		void resume();
		void stop( bool bringToCompletion );
	}
	

	/// <summary>
	/// all of the methods needed to control a tween. Any non-tweens that want to take part in ZestKit can implement this.
	/// The TweenChain is a good example of how it can be used.
	/// </summary>
	public interface ITweenable : ITweenControl
	{
		bool tick();
		void recycleSelf();
	}


	/// <summary>
	/// any object that wants to be tweened needs to implement this. ZestKit internally likes to make a simple object
	/// that implements this interface and stores a reference to the object being tweened. That makes for tiny, simple,
	/// lightweight implementations that can be handed off to any TweenT
	/// </summary>
	public interface ITweenTarget<T> where T : struct
	{
		void setTweenedValue( T value );
	}
}
