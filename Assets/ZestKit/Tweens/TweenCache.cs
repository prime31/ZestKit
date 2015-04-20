using UnityEngine;
using System;
using System.Collections.Generic;


namespace ZestKit
{
	public class TweenCache
	{
		private static Dictionary<Type,Stack<object>> _cache = new Dictionary<Type,Stack<object>>();


		public static T get<T>() where T : new()
		{
			var type = typeof( T );

			if( !_cache.ContainsKey( type ) )
				_cache[type] = new Stack<object>( 3 );
			
			if( _cache[type].Count == 0 )
				return new T();

			return (T)_cache[type].Pop();
		}


		public static void put( object item )
		{
			var type = item.GetType();
			if( !_cache.ContainsKey( type ) )
				_cache[type] = new Stack<object>( 3 );

			_cache[type].Push( item );
		}
	}
}