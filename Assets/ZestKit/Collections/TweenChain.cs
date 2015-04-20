using UnityEngine;
using System;
using System.Collections.Generic;


namespace ZestKit
{
	public class TweenChain : ITweenable
	{
		private List<ITweenable> _tweenList = new List<ITweenable>();
		private int _currentTween = 0;
		private bool _isPaused;
		private Action<TweenChain> _completionHandler;


		#region ITweenInternal

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


		#region TweenChain control

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


		public void stop( bool bringToCompletion )
		{
			stop();
		}


		public void stop()
		{
			_currentTween = _tweenList.Count;
		}

		#endregion
	}
}