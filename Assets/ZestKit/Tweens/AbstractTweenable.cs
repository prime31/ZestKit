using UnityEngine;
using System.Collections;
using System;


namespace Prime31.ZestKit
{
	/// <summary>
	/// AbstractTweenable serves as a base for any custom classes you might want to make that can be ticked. These differ from
	/// ITweens in that they dont implement the ITweenT interface. What does that mean? It just says they an AbstractTweenable
	/// is not just moving a value from start to finish. It can do anything at all that requires a tick each frame.
	/// 
	/// The TweenChain is one example of AbstractTweenable for reference.
	/// </summary>
	public abstract class AbstractTweenable : ITweenable
	{
		protected bool _isPaused;

		/// <summary>
		/// AbstractTweenable are often kept around after they complete. This flag lets them know internally if they are currently
		/// being tweened by ZestKit so that they can readd themselves if necessary.
		/// </summary>
		protected bool _isActiveTween;


		#region ITweenable

		public abstract bool tick();


		public virtual void recycleSelf()
		{}

		#endregion


		#region ITweenControl

		public object context { get; set; }


		public bool isRunning()
		{
			return _isActiveTween && !_isPaused;
		}


		public void start()
		{
			// dont add ourself twice!
			if( _isActiveTween )
				return;
			
			ZestKit.instance.addTween( this );
			_isActiveTween = true;
			_isPaused = false;
		}


		public void pause()
		{
			_isPaused = true;
		}


		public void resume()
		{
			_isPaused = false;
		}


		public virtual void stop( bool bringToCompletion = false )
		{
			ZestKit.instance.removeTween( this );
			_isActiveTween = false;
			_isPaused = true;
		}


		public virtual void jumpToElapsedTime( float elapsedTime )
		{
			throw new System.NotImplementedException();
		}


		public virtual IEnumerator waitForCompletion()
		{
			throw new System.NotImplementedException();
		}

		#endregion

	}
}