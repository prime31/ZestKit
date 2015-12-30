using UnityEngine;
using System.Collections;


namespace Prime31.ZestKit
{
	/// <summary>
	/// helper base class to make creating custom ITweenTargets as trivial as possible
	/// </summary>
	public abstract class AbstractTweenTarget<U,T> : ITweenTarget<T> where T : struct
	{
		protected U _target;

		abstract public void setTweenedValue( T value );
		abstract public T getTweenedValue();


		public void setTarget( U target )
		{
			_target = target;
		}


		public bool validateTarget()
		{
			return !_target.Equals( null );
		}


		public object getTargetObject()
		{
			return _target;
		}
	}
}