using UnityEngine;
using System.Collections;


namespace Prime31.ZestKit
{
	/// <summary>
	/// HSVColor uses a hue between 0 - 360 and saturation/value between 0 - 100%
	/// </summary>
	public struct HSVColor
	{
		public float hue, saturation, value;


		public HSVColor( float hue, float saturation, float value )
		{
			this.hue = hue;
			this.saturation = saturation;
			this.value = value;
		}


		public HSVColor( Color color )
		{
			this = colorToHSV( color );
		}


		public void makeComplimentary()
		{
			if( hue < 180f )
				hue += 180;
			else
				hue -= 180f;
		}


		/// <summary>
		/// shifts the color (value and saturation) making it brighter and less saturated (or vice versa for negative values)
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void shift( float amount )
		{
			value += amount;
			saturation -= amount;
		}


		#region Convertors to/from Color

		public static Color colorFromHSV( float hue, float saturation, float value, float alpha = 1f )
		{
			// no saturation, we can return the value across the board (grayscale)
			if( saturation == 0 )
				return new Color( value, value, value, alpha );

			// which chunk of the rainbow are we in?
			var sector = hue / 60f;

			// split across the decimal (ie 3.87 into 3 and 0.87)
			int i = (int)sector;
			float f = sector - i;

			var p = value * ( 1 - saturation );
			var q = value * ( 1 - saturation * f );
			var t = value * ( 1 - saturation * ( 1 - f ) );

			// build our rgb color
			var color = new Color( 0, 0, 0, alpha );

			switch( i )
			{
				case 0:
					color.r = value;
					color.g = t;
					color.b = p;
					break;

				case 1:
					color.r = q;
					color.g = value;
					color.b = p;
					break;

				case 2:
					color.r  = p;
					color.g  = value;
					color.b  = t;
					break;

				case 3:
					color.r  = p;
					color.g  = q;
					color.b  = value;
					break;

				case 4:
					color.r  = t;
					color.g  = p;
					color.b  = value;
					break;

				default:
					color.r  = value;
					color.g  = p;
					color.b  = q;
					break;
			}

			return color;
		}


		public static HSVColor colorToHSV( Color color )
		{
			var hsv = new HSVColor();

			float min = Mathf.Min( Mathf.Min( color.r, color.g ), color.b );
			float max = Mathf.Max( Mathf.Max( color.r, color.g ), color.b );
			float delta = max - min;

			// value is our max color
			hsv.value = max;

			// saturation is percent of max
			if( !Mathf.Approximately( max, 0f ) )
			{
				hsv.saturation = delta / max;
			}
			else
			{
				// all colors are zero, no saturation and hue is undefined
				hsv.saturation = 0f;
				hsv.hue = -1f;

				return hsv;
			}

			// grayscale image if min and max are the same
			if( Mathf.Approximately( min, max ) )
			{
				hsv.value = max;
				hsv.saturation = 0f;
				hsv.hue = -1f;

				return hsv;
			}

			// hue depends which color is max (this creates a rainbow effect)
			if( color.r == max )
				hsv.hue = ( color.g - color.b ) / delta; // between yellow & magenta
			else if( color.g == max )
				hsv.hue = 2f + ( color.b - color.r ) / delta; // between cyan & yellow
			else
				hsv.hue = 4f + ( color.r - color.g ) / delta; // between magenta & cyan

			// turn hue into 0-360 degrees
			hsv.hue *= 60;
			if( hsv.hue < 0 )
				hsv.hue += 360;

			return hsv;
		}

		#endregion


		#region Implicit operators

		public static HSVColor operator +( HSVColor a, HSVColor b )
		{
			return new HSVColor
			(
				Mathf.Clamp( a.hue + b.hue, 0f, 360f ),
				Mathf.Clamp( a.saturation + b.saturation, 0f, 100f ),
				Mathf.Clamp( a.value + b.value, 0f, 100f )
			);
		}


		public static HSVColor operator -( HSVColor a, HSVColor b )
		{
			return new HSVColor
			(
				Mathf.Clamp( a.hue - b.hue, 0f, 360f ),
				Mathf.Clamp( a.saturation - b.saturation, 0f, 100f ),
				Mathf.Clamp( a.value - b.value, 0f, 100f )
			);
		}


		public static implicit operator Color( HSVColor hsvColor )
		{
			return HSVColor.colorFromHSV( hsvColor.hue, hsvColor.saturation, hsvColor.value );
		}

		#endregion
	
	}
}
