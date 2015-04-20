/*

v2: Matt Rix pointed out there's an undocumented ONGeneratedCSProjectFiles() callback
https://gist.github.com/MattRix/0bf8de88e16e8b494dbb

v1: Still available in the gist history if you want a FileSystemWatcher solution!

THE PROBLEM:

- Unity constantly rewrites its .sln files whenever you rename/add/remove scripts
- Their solution files includes a bunch of formatting/tab/etc settings
- Unfortunately, MonoDevelop / Xamarin Studio give these settings priority over your other defaults

(At least sometimes--over here I only get issues maybe once or twice a day)

THE SOLUTION:

- Strip out this entire block of settings whenever any .sln file changes

TO USE:

- Toss this script anywhere in an Editor/ folder
- Enjoy your tabs not getting constantly mangled!

Enjoy,
Matthew / Team Colorblind

*/


using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class StripGeneratedSolutionSettings : AssetPostprocessor
{
	/// <summary>
	/// Undocumented callback, thanks Matt Rix!
	/// </summary>
	private static void OnGeneratedCSProjectFiles()
	{
		string currentDir = Directory.GetCurrentDirectory();
		var slnFiles = Directory.GetFiles( currentDir, "*.sln" );
		var csprojFiles = Directory.GetFiles( currentDir, "*.csproj" );

		foreach( var filePath in slnFiles )
		{
			FixSolution( filePath );
		}

		foreach( var filePath in csprojFiles )
		{
			FixProject( filePath );
		}
	}


	/// <summary>
	/// Put any project changes here
	/// </summary>
	/// <returns><c>true</c>, if project was fixed, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	static bool FixProject( string filePath )
	{
		string content = File.ReadAllText( filePath );

		string searchString = "<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>";
		string replaceString = "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>";

		if( content.IndexOf( searchString ) != -1 )
		{
			content = Regex.Replace( content, searchString, replaceString );
			File.WriteAllText( filePath, content );
			return true;
		}
		else
		{
			return false;
		}
	}


	/// <summary>
	/// Remove settings and .unityproj files
	/// </summary>
	/// <returns><c>true</c>, if solution was fixed, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	static bool FixSolution( string filePath )
	{
		string content = File.ReadAllText( filePath );
		string newContent = content;

		// xamarin studio/monodevelop barf on .unityproj files
		string stripUnityProject = @"Project.*?EndProject";
		newContent = Regex.Replace( newContent, stripUnityProject, new MatchEvaluator( CleanProjects ), RegexOptions.Singleline );

		// strip out all solution preferences
		string stripSettings = @"GlobalSection\(MonoDevelopProperties\)\s=\spreSolution.*EndGlobalSection";
		newContent = Regex.Replace( newContent, stripSettings, "", RegexOptions.Singleline | RegexOptions.RightToLeft );

		// save back if it was changed
		if( content != newContent )
		{
			File.WriteAllText( filePath, newContent );
			return true;
		}
		else
		{
			return false;
		}
	}


	/// <summary>
	/// If our project match is a .unityproj file, strip it
	/// </summary>
	/// <returns>The projects.</returns>
	/// <param name="m">M.</param>
	static string CleanProjects( Match m )
	{
		string text = m.ToString();

		if( text.Contains( ".unityproj" ) )
			return "";
		else
			return text;
	}
}
