using UnityEngine;
using System.Collections;


namespace ZestKit
{
	public class TransformRotationTarget : ITweenTarget<Quaternion>
	{
		public enum TransformRotationType
		{
			Rotation,
			LocalRotation
		}

		Transform _transform;
		TransformRotationType _targetType;


		public void setTweenedValue( Quaternion value )
		{
			switch( _targetType )
			{
				case TransformRotationType.Rotation:
					_transform.rotation = value;
					break;
				case TransformRotationType.LocalRotation:
					_transform.localRotation = value;
					break;
			}
		}
	

		public TransformRotationTarget( Transform transform, TransformRotationType targetType )
		{
			_transform = transform;
			_targetType = targetType;
		}
	}


	#region Material targets

	public abstract class AbstractMaterialTarget
	{
		protected Material _material;
		protected string _propertyName;


		public void prepareForUse( Material material, string propertyName )
		{
			_material = material;
			_propertyName = propertyName;
		}
	}


	public class MaterialColorTarget : AbstractMaterialTarget, ITweenTarget<Color>
	{
		public void setTweenedValue( Color value )
		{
			_material.SetColor( _propertyName, value );
		}
	}


	public class MaterialAlphaTarget : AbstractMaterialTarget, ITweenTarget<float>
	{
		public void setTweenedValue( float value )
		{
			var color = _material.GetColor( _propertyName );
			color.a = value;
			_material.SetColor( _propertyName, color );
		}
	}


	public class MaterialFloatTarget : AbstractMaterialTarget, ITweenTarget<float>
	{
		public void setTweenedValue( float value )
		{
			_material.SetFloat( _propertyName, value );
		}
	}


	public class MaterialVector4Target : AbstractMaterialTarget, ITweenTarget<Vector4>
	{
		public void setTweenedValue( Vector4 value )
		{
			_material.SetVector( _propertyName, value );
		}
	}

	#endregion

}
