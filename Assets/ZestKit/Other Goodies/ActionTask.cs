using UnityEngine;
using System;



namespace Prime31.ZestKit
{
	/// <summary>
	/// ActionTasks let you pass in an Action that will be called at different intervals depending on how you set it up.
	/// Note that all ActionTask static constructor methods will automatically cache the ActionTasks for easy reuse. Also note
	/// that the real trick to this class is to pass in a context object that you use in the Action when it is called. That is how
	/// you avoid allocations when using anonymous Actions.
	/// 
	/// All of the ITweenable methods apply here so you can pause/resume/stop the ActionTask at any time.
	/// </summary>
	public class ActionTask : AbstractTweenable
	{
		/// <summary>
		/// provides access to the context for this task
		/// </summary>
		/// <value>The context.</value>
		public object context { get; private set; }

		/// <summary>
		/// provides the elapsed time not included the initial delay that this task has been running
		/// </summary>
		/// <value>The elapsed time.</value>
		public float elapsedTime { get { return _unfilteredElapsedTime; } }

		Action<ActionTask> _action;
		float _unfilteredElapsedTime;
		float _elapsedTime;
		float _initialDelay = 0f;
		float _repeatDelay = 0f;
		bool _repeats = false;
		bool _isTimeScaleIndependent = false;


		#region static convenience constructors

		/// <summary>
		/// creates an ActionTask but does not start it!
		/// </summary>
		/// <param name="action">Action.</param>
		public static ActionTask create( Action<ActionTask> action )
		{
			return QuickCache<ActionTask>.pop()
				.setAction( action );
		}


		/// <summary>
		/// creates an ActionTask with a context but does not start it!
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="context">Context.</param>
		public static ActionTask create( Action<ActionTask> action, object context )
		{
			return QuickCache<ActionTask>.pop()
				.setAction( action )
				.setContext( context );
		}


		/// <summary>
		/// calls the Action every repeatsDelay seconds. The ActionTask is automatically started for you.
		/// </summary>
		/// <param name="initialDelay">Initial delay.</param>
		/// <param name="repeatDelay">Repeat delay.</param>
		/// <param name="context">Context.</param>
		/// <param name="action">Action.</param>
		public static ActionTask every( float repeatDelay, object context, Action<ActionTask> action )
		{
			var task = QuickCache<ActionTask>.pop()
				.setAction( action )
				.setRepeats( repeatDelay )
				.setContext( context );
			task.start();

			return task;
		}


		/// <summary>
		/// calls the Action every repeatsDelay seconds after the initial delay. The ActionTask is automatically started for you.
		/// </summary>
		/// <param name="initialDelay">Initial delay.</param>
		/// <param name="repeatDelay">Repeat delay.</param>
		/// <param name="context">Context.</param>
		/// <param name="action">Action.</param>
		public static ActionTask every( float initialDelay, float repeatDelay, object context, Action<ActionTask> action )
		{
			var task = QuickCache<ActionTask>.pop()
				.setAction( action )
				.setRepeats( repeatDelay )
				.setContext( context )
				.setDelay( initialDelay );
			task.start();

			return task;
		}


		/// <summary>
		/// calls the action after an initial delay. The ActionTask is automatically started for you.
		/// </summary>
		/// <param name="initialDelay">Initial delay.</param>
		/// <param name="context">Context.</param>
		/// <param name="action">Action.</param>
		public static ActionTask afterDelay( float initialDelay, object context, Action<ActionTask> action )
		{
			var task = QuickCache<ActionTask>.pop()
				.setAction( action )
				.setDelay( initialDelay )
				.setContext( context );
			task.start();

			return task;
		}

		#endregion


		// paramaterless constructor for use with QuickCache
		public ActionTask()
		{}


		/// <summary>
		/// sets the Action to be called
		/// </summary>
		/// <param name="action">Action.</param>
		public ActionTask setAction( Action<ActionTask> action )
		{
			_action = action;
			return this;
		}


		/// <summary>
		/// Sets the delay before the Action is called
		/// </summary>
		/// <returns>The delay.</returns>
		/// <param name="delay">Delay.</param>
		public ActionTask setDelay( float delay )
		{
			_initialDelay = delay;
			return this;
		}


		/// <summary>
		/// tells this action to repeat. It will repeat every frame unless a repeatDelay is provided
		/// </summary>
		/// <returns>The repeats.</returns>
		/// <param name="repeatDelay">Repeat delay.</param>
		public ActionTask setRepeats( float repeatDelay = 0f )
		{
			_repeats = true;
			_repeatDelay = repeatDelay;
			return this;
		}


		/// <summary>
		/// allows you to set any object reference retrievable via tween.context. This is handy for avoiding
		/// closure allocations for completion handler Actions. You can also search ZestKit for all tweens with a specific
		/// context.
		/// </summary>
		/// <returns>The context.</returns>
		/// <param name="context">Context.</param>
		public ActionTask setContext( object context )
		{
			this.context = context;
			return this;
		}


		/// <summary>
		/// sets the task to use Time.unscaledDeltaTime instead of Time.deltaTime
		/// </summary>
		/// <returns>ActionTask</returns>
		public ActionTask setIsTimeScaleIndependant()
		{
			_isTimeScaleIndependent = true;
			return this;
		}


		#region AbstractTweenable

		public override bool tick()
		{
			if( _isPaused )
				return false;


			var deltaTime = _isTimeScaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;

			// handle our initial delay first
			if( _initialDelay > 0f )
			{
				_initialDelay -= deltaTime;

				// catch the overflow if we have any. if we end up less than 0 while decrementing our initial delay we make that our elapsedTime
				// so that the Action gets called and so that we keep time accurately.
				if( _initialDelay < 0f )
				{
					_elapsedTime = -_initialDelay;
					_action( this );

					// if we repeat continue on. if not, then we end things here
					if( _repeats )
					{
						return false;
					}
					else
					{
						_isCurrentlyManagedByZestKit = false;
						return true;
					}
				}
				else
				{
					return false;
				}
			}


			// done with initial delay. now we either tick the Action every frame or use the repeatDelay to delay calls to the Action
			if( _repeatDelay > 0f )
			{
				if( _elapsedTime > _repeatDelay )
				{
					_elapsedTime -= _repeatDelay;
					_action( this );
				}
			}
			else
			{
				_action( this );
			}

			_unfilteredElapsedTime += deltaTime;
			_elapsedTime += deltaTime;

			return false;
		}


		public override void recycleSelf()
		{
			_unfilteredElapsedTime = _elapsedTime = _initialDelay = _repeatDelay = 0f;
			_isPaused = _isCurrentlyManagedByZestKit = _repeats = _isTimeScaleIndependent = false;
			context = null;
			_action = null;

			QuickCache<ActionTask>.push( this );
		}

		#endregion
	}
}
