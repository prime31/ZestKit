using UnityEngine;
using System;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	/// <summary>
	/// simple static class that can be used to pool any object. this is meant for use with non-Unity classes such as tweens.
	/// </summary>
	public static class TweenCache<T> where T : new()
	{
		private static Stack<T> _objectStack = new Stack<T>( 10 );


		public static T pop()
		{
			if( _objectStack.Count > 0 )
				return _objectStack.Pop();

			return new T();
		}


		public static void push( T obj )
		{
			_objectStack.Push( obj );
		}
	}
}