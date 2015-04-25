using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


namespace Prime31.ZestKit
{
	public class TweenChain : ITweenable
	{
		public object context { get; set; }


		private List<ITweenable> _tweenList = new List<ITweenable>();
		private int _currentTween = 0;
		private bool _isPaused;
		private Action<TweenChain> _completionHandler;


		#region ITweenable

		public bool tick()
		{
			if( _isPaused )
				return false;

			// if currentTween is greater than we we've got end this chain
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
					return true;
				}
			}

			return false;
		}


		public void recycleSelf()
		{
			for( var i = 0; i < _tweenList.Count; i++ )
				_tweenList[i].recycleSelf();
			_tweenList.Clear();
		}

		#endregion


		#region TweenChain management

		public TweenChain appendTween( ITweenControl tween )
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


		#region ITweenControl

		public bool isRunning()
		{
			return !_isPaused;
		}


		public void start()
		{
			ZestKit.instance.addTween( this );
		}


		public void pause()
		{
			_isPaused = true;
		}


		public void resume()
		{
			_isPaused = false;
		}


		/// <summary>
		/// bringToCompletion is ignored for chains due to it not having a solid, specific meaning for a chain
		/// </summary>
		/// <param name="bringToCompletion">If set to <c>true</c> bring to completion.</param>
		public void stop( bool bringToCompletion = false )
		{
			_currentTween = _tweenList.Count;
		}


		/// <summary>
		/// this is ignored for TweenChains since it doesn't have a clear definition
		/// </summary>
		/// <param name="elapsedTime">Elapsed time.</param>
		public void jumpToElapsedTime( float elapsedTime )
		{}


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
	
	}
}