using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace ZestKit
{
	/// <summary>
	/// this class adds extension methods for the most commonly used tweens
	/// </summary>
	public static class ZestKitExtensions
	{
		#region Transform tweens

		public static ITween<Vector3> ZKpositionTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = TransformVector3Tween.nextAvailableTween();
			tween.setTargetAndType( self, TransformVector3Tween.TransformTargetType.Position );
			tween.initialize( tween, self.position, to, duration );

			return tween;
		}


		public static ITween<Vector3> ZKlocalPositionTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = TransformVector3Tween.nextAvailableTween();
			tween.setTargetAndType( self, TransformVector3Tween.TransformTargetType.LocalPosition );
			tween.initialize( tween, self.localPosition, to, duration );

			return tween;
		}


		public static ITween<Vector3> ZKlocalScaleTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = TransformVector3Tween.nextAvailableTween();
			tween.setTargetAndType( self, TransformVector3Tween.TransformTargetType.LocalScale );
			tween.initialize( tween, self.localScale, to, duration );

			return tween;
		}


		public static ITween<Vector3> ZKeulersTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = TransformVector3Tween.nextAvailableTween();
			tween.setTargetAndType( self, TransformVector3Tween.TransformTargetType.EulerAngles );
			tween.initialize( tween, self.eulerAngles, to, duration );

			return tween;
		}


		public static ITween<Vector3> ZKlocalEulersTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = TransformVector3Tween.nextAvailableTween();
			tween.setTargetAndType( self, TransformVector3Tween.TransformTargetType.LocalEulerAngles );
			tween.initialize( tween, self.localEulerAngles, to, duration );

			return tween;
		}


		public static ITween<Quaternion> ZKrotationTo( this Transform self, Quaternion to, float duration = 0.3f )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.Rotation );
			var tween = new QuaternionTween( tweenTarget, self.rotation, to, duration );

			return tween;
		}


		public static ITween<Quaternion> ZKlocalRotationTo( this Transform self, Quaternion to, float duration = 0.3f )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.LocalRotation );
			var tween = new QuaternionTween( tweenTarget, self.localRotation, to, duration );

			return tween;
		}

		#endregion


		#region Material tweens

		public static ITween<Color> ZKcolorTo( this Material self, Color to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialColorTarget( self, propertyName );
			var tween = new ColorTween( tweenTarget, self.GetColor( propertyName ), to, duration );

			return tween;
		}


		public static ITween<float> ZKalphaTo( this Material self, float to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialAlphaTarget( self, propertyName );
			var tween = new FloatTween( tweenTarget, self.GetColor( propertyName ).a, to, duration );

			return tween;
		}


		public static ITween<float> ZKfloatTo( this Material self, float to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialFloatTarget( self, propertyName );
			var tween = new FloatTween( tweenTarget, self.GetFloat( propertyName ), to, duration );

			return tween;
		}


		public static ITween<Vector4> ZKVector4To( this Material self, Vector4 to, float duration, string propertyName )
		{
			var tweenTarget = new MaterialVector4Target( self, propertyName );
			var tween = new Vector4Tween( tweenTarget, self.GetVector( propertyName ), to, duration );

			return tween;
		}


		public static ITween<Vector2> ZKtextureOffsetTo( this Material self, Vector2 to, float duration, string propertyName )
		{
			var tweenTarget = new MaterialTextureOffsetTarget( self, propertyName );
			var tween = new Vector2Tween( tweenTarget, self.GetTextureOffset( propertyName ), to, duration );

			return tween;
		}


		public static ITween<Vector2> ZKtextureScaleTo( this Material self, Vector2 to, float duration, string propertyName )
		{
			var tweenTarget = new MaterialTextureScaleTarget( self, propertyName );
			var tween = new Vector2Tween( tweenTarget, self.GetTextureScale( propertyName ), to, duration );

			return tween;
		}

		#endregion


		#region AudioSource tweens

		public static ITween<float> ZKvolumeTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.Volume );
			var tween = new FloatTween( tweenTarget, self.volume, to, duration );

			return tween;
		}


		public static ITween<float> ZKpitchTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.Pitch );
			var tween = new FloatTween( tweenTarget, self.pitch, to, duration );

			return tween;
		}


		public static ITween<float> ZKpanStereoTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.PanStereo );
			var tween = new FloatTween( tweenTarget, self.panStereo, to, duration );

			return tween;
		}

		#endregion


		#region Camera tweens

		public static ITween<float> ZKfieldOfViewTo( this Camera self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self, CameraTarget.CameraTargetType.FieldOfView );
			var tween = new FloatTween( tweenTarget, self.fieldOfView, to, duration );

			return tween;
		}


		public static ITween<float> ZKorthographicSizeTo( this Camera self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self, CameraTarget.CameraTargetType.OrthographicSize );
			var tween = new FloatTween( tweenTarget, self.orthographicSize, to, duration );

			return tween;
		}


		public static ITween<Color> ZKbackgroundColorTo( this Camera self, Color to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self );
			var tween = new ColorTween( tweenTarget, self.backgroundColor, to, duration );

			return tween;
		}

		#endregion


		#region CanvasGroup tweens

		public static ITween<float> ZKalphaTo( this CanvasGroup self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CanvasGroupTarget( self );
			var tween = new FloatTween( tweenTarget, self.alpha, to, duration );

			return tween;
		}

		#endregion


		#region Image tweens

		public static ITween<float> ZKalphaTo( this Image self, float to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.Alpha );
			var tween = new FloatTween( tweenTarget, self.color.a, to, duration );

			return tween;
		}


		public static ITween<float> ZKfillAmountTo( this Image self, float to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.FillAmount );
			var tween = new FloatTween( tweenTarget, self.fillAmount, to, duration );

			return tween;
		}


		public static ITween<Color> ZKcolorTo( this Image self, Color to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.Alpha );
			var tween = new ColorTween( tweenTarget, self.color, to, duration );

			return tween;
		}

		#endregion


		#region RectTransform tweens

		public static ITween<Vector2> ZKanchoredPositionTo( this RectTransform self, Vector2 to, float duration = 0.3f )
		{
			var tweenTarget = new RectTransformTarget( self );
			var tween = new Vector2Tween( tweenTarget, self.anchoredPosition, to, duration );

			return tween;
		}


		public static ITween<Vector3> ZKanchoredPosition3DTo( this RectTransform self, Vector3 to, float duration = 0.3f )
		{
			var tweenTarget = new RectTransformTarget( self );
			var tween = new Vector3Tween( tweenTarget, self.anchoredPosition3D, to, duration );

			return tween;
		}

		#endregion


		#region ScrollRect tweens

		public static ITween<Vector2> ZKnormalizedPositionTo( this ScrollRect self, Vector2 to, float duration = 0.3f )
		{
			var tweenTarget = new ScrollRectTarget( self );
			var tween = new Vector2Tween( tweenTarget, self.normalizedPosition, to, duration );

			return tween;
		}

		#endregion

	}
}