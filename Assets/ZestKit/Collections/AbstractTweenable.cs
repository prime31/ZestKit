using UnityEngine;
using System.Collections;


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