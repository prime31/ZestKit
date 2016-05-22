# ZestKit

The 3rd tween library for Unity. Yes, you read that number correctly. First there was [GoKit](https://github.com/prime31/GoKit). Then there was [GoKitLite](https://github.com/prime31/GoKitLite). They each had their purpose. GoKit was insanely customizable and GoKitLite was insanely fast. ZestKit was designed to take the best of both of them and make it even better. For a brief overview on the design and structure checkout [this blog post](http://blog.prime31.com/anatomy-of-a-tween-library/). Be sure to checkout [the wiki](https://github.com/prime31/ZestKit/wiki) as well for usage examples.


### Quick Start or I Just Wanna Tween

Import the files in the ZestKit folder into your project, add ```using Prime31.ZestKit``` to your file and you are ready to start tweening. Using the [extension methods](http://csharp.net-tutorials.com/csharp-3.0/extension-methods/) (currently covering 26+ methods for the Transform, Material, AudioSource, Camera, CanvasGroup, Image, RectTransform and ScrollRect classes) will get you up and running in no time. Note that all extension methods have the "ZK" prefix to avoid conflicts. Examples:

```csharp
// tween position to Vector3.one over 0.3 seconds
transform.ZKpositionTo( Vector3.one, 0.3f ).start();

// tween eulerAngles to Vector3.zero over 0.3 seconds then ping-pong back to the original value
transform.ZKpositionTo( Vector3.zero, 0.3f )
    .setLoops( LoopType.PingPong )
    .start();

// tween the Material _Color property from black to yellow over 0.5 seconds
material.ZKcolorTo( Color.yellow, 0.5f )
    .setFrom( Color.black )
    .start();

// tween localScale independant of Time.timeScale with a 2 second delay before starting the tween
// and get notified when the tween has finished specifying the easing equation to use
transform.ZKlocalScaleTo( new Vector3( 10f, 10f, 10f ), 0.5f )
    .setDelay( 2f )
    .setIsTimeScaleIndependent()
    .setCompletionHandler( myCompletionHandlerFunction )
    .setEaseType( EaseType.ElasticOut )
    .start();
```


### What Can I Tween?

Out of the box, ZestKit can tween any int, float, Vector2, Vector3, Vector4, Quaternion, Rect, Color, Color32 and it has a built in spline editor ("borrowed" from GoKit and expanded upon ;). ZestKit offers both strongly targeted and weakly targeted tweens. What's the difference between strongly and weakly targeted tweens? A strongly targeted tween is something ZestKit knows about out of the box. The most commonly used would be transform.position/rotation/scale, material.color, etc. A weakly targeted tween means ZestKit doesn't know about the object or property being tweened. For example, if you have a custom class (SomeCustomClass) that has a Vector3 property (myVector3) you can still tween this value with ZestKit by using a property tween. The following would do the trick:

```csharp
PropertyTweens.vector3PropertyTo( someCustomClassInstance, "myVector3", Vector3.zero, Vector3.one, 0.4f )
```


### What About Those Fancy Easing Equations?

ZestKit offers a bunch of built-in easing equations and it also lets you specify an AnimationCurve to handle easing for maximum flexibility. The included easing types are: Linear, SineIn, SineOut, SineInOut, QuadIn, QuadOut, QuadInOut, CubicIn, CubicOut, CubicInOut, QuartIn, QuartOut, QuartInOut, QuintIn, QuintOut, QuintInOut, ExpoIn, ExpoOut, ExpoInOut, CircIn, CircOut, CircInOut, ElasticIn, ElasticOut, ElasticInOut, Punch, BackIn, BackOut, BackInOut, BounceIn, BounceOut and BounceInOut.



#### Extending ZestKit or Using the Tween Engine Without ZestKit

Special care was taken when making ZestKit so that it can be extended with great ease. Let's take the PropertyTween example from above and make it a proper ZestKit tween so that we don't have to use the PropertyTween class (it allocates a bit of memory to locate the property). All we have to do is implement the ```ITweenTarget<T>``` interface which contains a single method:

```csharp
public class SomeCustomClass : ITweenTarget<Vector3>
{
  public void setTweenedValue( Vector3 value )
  {
      myVector3 = value;
  }
}
```

That's it. SomeCustomClass is now a valid ```ITweenTarget<T>``` and it can tweened with a ```Vector3Tween``` (just pass it as the first parameter to the constructor or the initialize method of the Vector3Tween).

It was mentioned above that you can use the ZestKit tween engine without ZestKit. The ```Zest``` class contains the core tween engine and all of its easing powers ready to use. If you are going that route, it is expected that you are a big girl/boy and can read the code and figure out how to use it so you're on your own from here.



#### License

[Attribution-NonCommercial-ShareAlike 3.0 Unported](http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode) with [simple explanation](http://creativecommons.org/licenses/by-nc-sa/3.0/deed.en_US) with the attribution clause waived. You are free to use ZestKit in any and all games that you make. You cannot sell ZestKit directly or as part of a larger game asset.
