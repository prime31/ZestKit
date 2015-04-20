using UnityEngine;
using System.Collections.Generic;


namespace ZestKit
{
	public partial class ZestKit : MonoBehaviour
	{
		public static EaseType defaultEaseType = EaseType.QuartIn;

		private List<ITweenable> _activeTweens = new List<ITweenable>( 5 );


		/// <summary>
		/// holds the singleton instance. creates one on demand if none exists.
		/// </summary>
		private static ZestKit _instance;
		public static ZestKit instance
		{
			get
			{
				if( !_instance )
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


		private void OnApplicationQuit()
		{
			_instance = null;
			Destroy( gameObject );
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

		public void addTween( ITweenable tween )
		{
			_activeTweens.Add( tween );
		}


		void removeTween( ITweenable tween, int index )
		{
			_activeTweens.RemoveAt( index );
			tween.recycleSelf();
		}


		void removeTween( ITweenable tween )
		{
			_activeTweens.Remove( tween );
			tween.recycleSelf();
		}

		#endregion

	}
}
