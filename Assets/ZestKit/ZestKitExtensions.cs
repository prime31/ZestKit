using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Prime31.ZestKit
{
	/// <summary>
	/// this class adds extension methods for the most commonly used tweens
	/// </summary>
	public static class ZestKitExtensions
	{
		#region Transform tweens

		/// <summary>
		/// transform.position tween
		/// </summary>
		/// <returns>The kposition to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKpositionTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = QuickCache<TransformVector3Tween>.pop();
			tween.setTargetAndType( self, TransformTargetType.Position );
			tween.initialize( tween, self.position, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.localPosition tween
		/// </summary>
		/// <returns>The klocal position to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKlocalPositionTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = QuickCache<TransformVector3Tween>.pop();
			tween.setTargetAndType( self, TransformTargetType.LocalPosition );
			tween.initialize( tween, self.localPosition, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.localScale tween
		/// </summary>
		/// <returns>The klocal scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKlocalScaleTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = QuickCache<TransformVector3Tween>.pop();
			tween.setTargetAndType( self, TransformTargetType.LocalScale );
			tween.initialize( tween, self.localScale, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.eulers tween
		/// </summary>
		/// <returns>The keulers to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKeulersTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = QuickCache<TransformVector3Tween>.pop();
			tween.setTargetAndType( self, TransformTargetType.EulerAngles );
			tween.initialize( tween, self.eulerAngles, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.localEulers tween
		/// </summary>
		/// <returns>The klocal eulers to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKlocalEulersTo( this Transform self, Vector3 to, float duration = 0.3f )
		{
			var tween = QuickCache<TransformVector3Tween>.pop();
			tween.setTargetAndType( self, TransformTargetType.LocalEulerAngles );
			tween.initialize( tween, self.localEulerAngles, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.rotation tween
		/// </summary>
		/// <returns>The krotation to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Quaternion> ZKrotationTo( this Transform self, Quaternion to, float duration = 0.3f )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.Rotation );
			var tween = new QuaternionTween( tweenTarget, self.rotation, to, duration );

			return tween;
		}


		/// <summary>
		/// transform.localRotation tween
		/// </summary>
		/// <returns>The klocal rotation to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Quaternion> ZKlocalRotationTo( this Transform self, Quaternion to, float duration = 0.3f )
		{
			var tweenTarget = new TransformRotationTarget( self, TransformRotationTarget.TransformRotationType.LocalRotation );
			var tween = new QuaternionTween( tweenTarget, self.localRotation, to, duration );

			return tween;
		}

		#endregion


		#region Material tweens

		/// <summary>
		/// tweens any Material Color property
		/// </summary>
		/// <returns>The kcolor to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<Color> ZKcolorTo( this Material self, Color to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialColorTarget( self, propertyName );
			var tween = ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
			tween.initialize( tweenTarget, self.GetColor( propertyName ), to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the alpha value of any Material Color property
		/// </summary>
		/// <returns>The kalpha to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<float> ZKalphaTo( this Material self, float to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialAlphaTarget( self, propertyName );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.GetColor( propertyName ).a, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens any Material float property
		/// </summary>
		/// <returns>The kfloat to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<float> ZKfloatTo( this Material self, float to, float duration = 0.3f, string propertyName = "_Color" )
		{
			var tweenTarget = new MaterialFloatTarget( self, propertyName );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.GetFloat( propertyName ), to, duration );

			return tween;
		}


		/// <summary>
		/// tweens any Material Vector4 property
		/// </summary>
		/// <returns>The vector4 to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<Vector4> ZKVector4To( this Material self, Vector4 to, float duration, string propertyName )
		{
			var tweenTarget = new MaterialVector4Target( self, propertyName );
			var tween = ZestKit.cacheVector4Tweens ? QuickCache<Vector4Tween>.pop() : new Vector4Tween();
			tween.initialize( tweenTarget, self.GetVector( propertyName ), to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the Materials texture offset
		/// </summary>
		/// <returns>The ktexture offset to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<Vector2> ZKtextureOffsetTo( this Material self, Vector2 to, float duration, string propertyName = "_MainTex" )
		{
			var tweenTarget = new MaterialTextureOffsetTarget( self, propertyName );
			var tween = ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
			tween.initialize( tweenTarget, self.GetTextureOffset( propertyName ), to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the Materials texture scale
		/// </summary>
		/// <returns>The ktexture scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="propertyName">Property name.</param>
		public static ITween<Vector2> ZKtextureScaleTo( this Material self, Vector2 to, float duration, string propertyName = "_MainTex" )
		{
			var tweenTarget = new MaterialTextureScaleTarget( self, propertyName );
			var tween = ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
			tween.initialize( tweenTarget, self.GetTextureScale( propertyName ), to, duration );

			return tween;
		}

		#endregion


		#region AudioSource tweens

		/// <summary>
		/// tweens an AudioSource volume property
		/// </summary>
		/// <returns>The kvolume to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKvolumeTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.Volume );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.volume, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens an AudioSource pitch property
		/// </summary>
		/// <returns>The kpitch to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKpitchTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.Pitch );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.pitch, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens an AudioSource panStereo property
		/// </summary>
		/// <returns>The kpan stereo to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKpanStereoTo( this AudioSource self, float to, float duration = 0.3f )
		{
			var tweenTarget = new AudioSourceFloatTarget( self, AudioSourceFloatTarget.AudioSourceFloatType.PanStereo );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.panStereo, to, duration );

			return tween;
		}

		#endregion


		#region Camera tweens

		/// <summary>
		/// tweens the Cameras fieldOfView
		/// </summary>
		/// <returns>The kfield of view to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKfieldOfViewTo( this Camera self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self, CameraTarget.CameraTargetType.FieldOfView );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.fieldOfView, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the Cameras orthographicSize
		/// </summary>
		/// <returns>The korthographic size to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKorthographicSizeTo( this Camera self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self, CameraTarget.CameraTargetType.OrthographicSize );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.orthographicSize, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the Cameras backgroundColor property
		/// </summary>
		/// <returns>The kbackground color to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Color> ZKbackgroundColorTo( this Camera self, Color to, float duration = 0.3f )
		{
			var tweenTarget = new CameraTarget( self );
			var tween = ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
			tween.initialize( tweenTarget, self.backgroundColor, to, duration );

			return tween;
		}

		#endregion


		#region CanvasGroup tweens

		/// <summary>
		/// tweens the CanvasGroup alpha property
		/// </summary>
		/// <returns>The kalpha to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKalphaTo( this CanvasGroup self, float to, float duration = 0.3f )
		{
			var tweenTarget = new CanvasGroupTarget( self );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.alpha, to, duration );

			return tween;
		}

		#endregion


		#region Image tweens

		/// <summary>
		/// tweens an Images alpha property
		/// </summary>
		/// <returns>The kalpha to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKalphaTo( this Image self, float to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.Alpha );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.color.a, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens an Images fillAmount property
		/// </summary>
		/// <returns>The kfill amount to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKfillAmountTo( this Image self, float to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.FillAmount );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.fillAmount, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens an Images color property
		/// </summary>
		/// <returns>The kcolor to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Color> ZKcolorTo( this Image self, Color to, float duration = 0.3f )
		{
			var tweenTarget = new ImageTarget( self, ImageTarget.ImageTargetType.Alpha );
			var tween = ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
			tween.initialize( tweenTarget, self.color, to, duration );

			return tween;
		}

		#endregion


		#region RectTransform tweens

		/// <summary>
		/// tweens the RectTransforms anchoredPosition property
		/// </summary>
		/// <returns>The kanchored position to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> ZKanchoredPositionTo( this RectTransform self, Vector2 to, float duration = 0.3f )
		{
			var tweenTarget = new RectTransformTarget( self );
			var tween = ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
			tween.initialize( tweenTarget, self.anchoredPosition, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens the RectTransforms anchoredPosition3D property
		/// </summary>
		/// <returns>The kanchored position3 D to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector3> ZKanchoredPosition3DTo( this RectTransform self, Vector3 to, float duration = 0.3f )
		{
			var tweenTarget = new RectTransformTarget( self );
			var tween = ZestKit.cacheVector3Tweens ? QuickCache<Vector3Tween>.pop() : new Vector3Tween();
			tween.initialize( tweenTarget, self.anchoredPosition3D, to, duration );

			return tween;
		}

		#endregion


		#region ScrollRect tweens

		/// <summary>
		/// tweens the ScrollRects normalizedPosition (scroll position)
		/// </summary>
		/// <returns>The knormalized position to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> ZKnormalizedPositionTo( this ScrollRect self, Vector2 to, float duration = 0.3f )
		{
			var tweenTarget = new ScrollRectTarget( self );
			var tween = ZestKit.cacheVector2Tweens ? QuickCache<Vector2Tween>.pop() : new Vector2Tween();
			tween.initialize( tweenTarget, self.normalizedPosition, to, duration );

			return tween;
		}

		#endregion


		#region Light tweens

		/// <summary>
		/// tweens a Lights intensity property
		/// </summary>
		/// <returns>The kintensity to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKintensityTo( this Light self, float to, float duration = 0.3f )
		{
			var tweenTarget = new LightTarget( self, LightTarget.LightTargetType.Intensity );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.intensity, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens a Lights range property
		/// </summary>
		/// <returns>The krange to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKrangeTo( this Light self, float to, float duration = 0.3f )
		{
			var tweenTarget = new LightTarget( self, LightTarget.LightTargetType.Range );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.range, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens a Lights spotAngle property
		/// </summary>
		/// <returns>The kspot angle to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<float> ZKspotAngleTo( this Light self, float to, float duration = 0.3f )
		{
			var tweenTarget = new LightTarget( self, LightTarget.LightTargetType.SpotAngle );
			var tween = ZestKit.cacheFloatTweens ? QuickCache<FloatTween>.pop() : new FloatTween();
			tween.initialize( tweenTarget, self.spotAngle, to, duration );

			return tween;
		}


		/// <summary>
		/// tweens a Lights color property
		/// </summary>
		/// <returns>The kcolor to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Color> ZKcolorTo( this Light self, Color to, float duration = 0.3f )
		{
			var tweenTarget = new LightTarget( self );
			var tween = ZestKit.cacheColorTweens ? QuickCache<ColorTween>.pop() : new ColorTween();
			tween.initialize( tweenTarget, self.color, to, duration );

			return tween;
		}

		#endregion

	}
}