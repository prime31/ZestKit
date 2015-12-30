using UnityEngine;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	public partial class ZestKit : MonoBehaviour
	{
		public static EaseType defaultEaseType = EaseType.QuartIn;

		/// <summary>
		/// if enabled, does a null check on the object being tweened. If null, the tweened value will not be set.
		/// Only AbstractTweenTarget subclasses and Transform tweens will do validation (that includes all the built in tweens).
		/// It is up to any ITweenTarget custom implementations to add validation themselves if they want to take part in the babysitter.
		/// </summary>
		public static bool enableBabysitter = false;

		/// <summary>
		/// if true, the active tween list will be cleared when a new level loads
		/// </summary>
		public static bool removeAllTweensOnLevelLoad = false;


		#region Caching rules

		/// <summary>
		/// automatic caching of various types is supported here. Note that caching will only work when using extension methods to start
		/// the tweens or if you fetch a tween from the cache when doing custom tweens. See the extension method implementations for
		/// how to fetch a cached tween.
		/// </summary>
		public static bool cacheIntTweens = false;
		public static bool cacheFloatTweens = false;
		public static bool cacheVector2Tweens = false;
		public static bool cacheVector3Tweens = false;
		public static bool cacheVector4Tweens = false;
		public static bool cacheQuaternionTweens = false;
		public static bool cacheColorTweens = false;
		public static bool cacheColor32Tweens = false;
		public static bool cacheRectTweens = false;

		#endregion


		/// <summary>
		/// internal list of all the currently active tweens
		/// </summary>
		List<ITweenable> _activeTweens = new List<ITweenable>();

		/// <summary>
		/// guard to stop instances being created while the application is quitting
		/// </summary>
		static bool _applicationIsQuitting;


		/// <summary>
		/// holds the singleton instance. creates one on demand if none exists.
		/// </summary>
		private static ZestKit _instance;
		public static ZestKit instance
		{
			get
			{
				if( !_instance && !_applicationIsQuitting )
				{
					// check if there is a GoKitLite instance already available in the scene graph before creating one
					_instance = FindObjectOfType( typeof( ZestKit ) ) as ZestKit;

					if( !_instance )
					{
						var obj = new GameObject( "ZestKit" );
						_instance = obj.AddComponent<ZestKit>();
						DontDestroyOnLoad( obj );
					}
				}

				return _instance;
			}
		}
			

		#region MonoBehaviour

		void Awake()
		{
			if( _instance == null )
				_instance = this;
		}


		void OnApplicationQuit()
		{
			_instance = null;
			Destroy( gameObject );
			_applicationIsQuitting = true;
		}


		void OnLevelWasLoaded( int level )
		{
			if( removeAllTweensOnLevelLoad )
				_activeTweens.Clear();
		}


		void Update()
		{
			// loop backwards so we can remove completed tweens
			for( var i = _activeTweens.Count - 1; i >= 0; --i )
			{
				var tween = _activeTweens[i];
				if( tween.tick() )
					removeTween( tween, i );
			}
		}

		#endregion


		#region Tween management

		/// <summary>
		/// adds a tween to the active tweens list
		/// </summary>
		/// <param name="tween">Tween.</param>
		public void addTween( ITweenable tween )
		{
			_activeTweens.Add( tween );
		}


		/// <summary>
		/// removes the tween at index from the active tweens list.
		/// </summary>
		/// <param name="tween">Tween.</param>
		/// <param name="index">Index.</param>
		public void removeTween( ITweenable tween, int index )
		{
			_activeTweens.RemoveAt( index );
			tween.recycleSelf();
		}


		/// <summary>
		/// removes a tween from the active tweens list. List.Remove can be quite slow so it is preferable to sue the other
		/// removeTween variant.
		/// </summary>
		/// <param name="tween">Tween.</param>
		public void removeTween( ITweenable tween )
		{
			_activeTweens.Remove( tween );
			tween.recycleSelf();
		}


		/// <summary>
		/// stops all tweens optionlly bringing them all to completion
		/// </summary>
		/// <param name="bringToCompletion">If set to <c>true</c> bring to completion.</param>
		public void stopAllTweens( bool bringToCompletion = false )
		{
			for( var i = _activeTweens.Count - 1; i >= 0; --i )
				_activeTweens[i].stop( bringToCompletion );
		}


		/// <summary>
		/// returns all the tweens that have a specific context. Tweens are returned as ITweenable since that is all
		/// that ZestKit knows about.
		/// </summary>
		/// <returns>The tweens with context.</returns>
		/// <param name="context">Context.</param>
		public List<ITweenable> allTweensWithContext( object context )
		{
			var foundTweens = new List<ITweenable>();

			for( var i = 0; i < _activeTweens.Count; i++ )
			{
				if( _activeTweens[i] is ITweenable && ( _activeTweens[i] as ITweenControl ).context == context )
					foundTweens.Add( _activeTweens[i] );
			}

			return foundTweens;
		}


		/// <summary>
		/// stops all the tweens with a given context
		/// </summary>
		/// <returns>The tweens with context.</returns>
		/// <param name="context">Context.</param>
		public void stopAllTweensWithContext( object context, bool bringToCompletion = false )
		{
			for( var i = _activeTweens.Count - 1; i >= 0; --i )
			{
				if( _activeTweens[i] is ITweenable && ( _activeTweens[i] as ITweenControl ).context == context )
					_activeTweens[i].stop( bringToCompletion );
			}
		}


		/// <summary>
		/// returns all the tweens that have a specific target. Tweens are returned as ITweenControl since that is all
		/// that ZestKit knows about.
		/// </summary>
		/// <returns>The tweens with target.</returns>
		/// <param name="target">target.</param>
		public List<ITweenable> allTweensWithTarget( object target )
		{
			var foundTweens = new List<ITweenable>();

			for( var i = 0; i < _activeTweens.Count; i++ )
			{
				if( _activeTweens[i] is ITweenControl )
				{
					var tweenControl = _activeTweens[i] as ITweenControl;
					if( tweenControl.getTargetObject() == target )
						foundTweens.Add( _activeTweens[i] as ITweenable );
				}
			}

			return foundTweens;
		}


		/// <summary>
		/// stops all the tweens that have a specific target
		/// that ZestKit knows about.
		/// </summary>
		/// <param name="target">target.</param>
		public void stopAllTweensWithTarget( object target, bool bringToCompletion = false )
		{
			for( var i = _activeTweens.Count - 1; i >= 0; --i )
			{
				if( _activeTweens[i] is ITweenControl )
				{
					var tweenControl = _activeTweens[i] as ITweenControl;
					if( tweenControl.getTargetObject() == target )
						tweenControl.stop( bringToCompletion );
				}
			}
		}

		#endregion

	}
}
