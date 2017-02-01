using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


namespace Prime31.ZestKit
{
	/// <summary>
	/// provides a container that allows you to chain together 2 or more ITweenables. They will run one after the other until
	/// all of them are complete.
	/// </summary>
	public class TweenChain : AbstractTweenable
	{
		List<ITweenable> _tweenList = new List<ITweenable>();
		int _currentTween = 0;
		Action<TweenChain> _completionHandler;

		public int totalTweens { get { return _tweenList.Count; } }


		public override void start()
		{
			// prep our first tween
			if( _tweenList.Count > 0 )
				_tweenList[0].start();
			
			base.start();
		}

		#region ITweenable

		public override bool tick()
		{
			if( _isPaused )
				return false;

			// if currentTween is greater than we've got in the tweenList end this chain
			if( _currentTween >= _tweenList.Count )
				return true;
			
			var tween = _tweenList[_currentTween];
			if( tween.tick() )
			{
				_currentTween++;
				if( _currentTween == _tweenList.Count )
				{
					if( _completionHandler != null )
						_completionHandler( this );

					_isCurrentlyManagedByZestKit = false;
					return true;
				}
				else
				{
					// we have a new tween so start it
					_tweenList[_currentTween].start();
				}
			}

			return false;
		}


		public override void recycleSelf()
		{
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].recycleSelf();
			_tweenList.Clear();
		}


		/// <summary>
		/// bringToCompletion is ignored for chains due to it not having a solid, specific meaning for a chain
		/// </summary>
		/// <param name="bringToCompletion">If set to <c>true</c> bring to completion.</param>
		public override void stop( bool bringToCompletion = false, bool bringToCompletionImmediately = false )
		{
			_currentTween = _tweenList.Count;
		}

		#endregion


		#region ITweenControl

		/// <summary>
		/// when called via StartCoroutine this will continue until the TweenChain completes
		/// </summary>
		/// <returns>The for completion.</returns>
		public IEnumerator waitForCompletion()
		{
			while( _currentTween < _tweenList.Count )
				yield return null;
		}

		#endregion


		#region TweenChain management

		public TweenChain appendTween( ITweenable tween )
		{
			// make sure we have a legit ITweenable
			if( tween is ITweenable )
			{
				tween.resume();
				_tweenList.Add( tween as ITweenable );
			}
			else
			{
				Debug.LogError( "attempted to add a tween that does not implement ITweenable to a TweenChain!" );
			}

			return this;
		}


		/// <summary>
		/// chainable. sets the action that should be called when the tween is complete.
		/// </summary>
		public TweenChain setCompletionHandler( Action<TweenChain> completionHandler )
		{
			_completionHandler = completionHandler;
			return this;
		}

		#endregion
	
	}
}