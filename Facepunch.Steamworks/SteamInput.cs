﻿using System;
using Steamworks.Data;
using System.Collections.Generic;

namespace Steamworks
{
	/// <summary>
	/// Class for utilizing Steam Input.
	/// </summary>
	public class SteamInput : SteamClientClass<SteamInput>
	{
		internal static ISteamInput Internal => Interface as ISteamInput;

		internal override bool InitializeInterface( bool server )
		{
			SetInterface( server, new ISteamInput( server ) );
			if ( Interface.Self == IntPtr.Zero ) return false;
			
			Internal.Init(true);

			return true;
		}

		internal const int STEAM_CONTROLLER_MAX_COUNT = 16;


		/// <summary>
		/// You shouldn't really need to call this because it gets called by <see cref="SteamClient.RunCallbacks"/>
		/// but Valve think it might be a nice idea if you call it right before you get input info -
		/// just to make sure the info you're getting is 100% up to date.
		/// </summary>
		public static void RunFrame()
		{
			Internal.RunFrame( false );
		}

		static readonly InputHandle_t[] queryArray = new InputHandle_t[STEAM_CONTROLLER_MAX_COUNT];

		/// <summary>
		/// Gets a list of connected controllers.
		/// </summary>
		public static IEnumerable<Controller> Controllers
		{
			get
			{
				var num = Internal.GetConnectedControllers( queryArray );

				for ( int i = 0; i < num; i++ )
				{
					yield return new Controller( queryArray[i] );
				}
			}
		}


        /// <summary>
        /// Return an absolute path to the PNG image glyph for the provided digital action name. The current
        /// action set in use for the controller will be used for the lookup. You should cache the result and
        /// maintain your own list of loaded PNG assets.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetDigitalActionGlyph( Controller controller, string action )
        {
            InputActionOrigin origin = InputActionOrigin.None;

            Internal.GetDigitalActionOrigins(
                controller.Handle,
                Internal.GetCurrentActionSet(controller.Handle),
                GetDigitalActionHandle(action),
                ref origin
            );

            return Internal.GetGlyphForActionOrigin_Legacy(origin);
        }


		/// <summary>
		/// Return an absolute path to the PNG image glyph for the provided digital action name. The current
		/// action set in use for the controller will be used for the lookup. You should cache the result and
		/// maintain your own list of loaded PNG assets.
		/// </summary>
		public static string GetPngActionGlyph( Controller controller, string action, GlyphSize size )
		{
			InputActionOrigin origin = InputActionOrigin.None;

			Internal.GetDigitalActionOrigins( controller.Handle, Internal.GetCurrentActionSet( controller.Handle ), GetDigitalActionHandle( action ), ref origin );

			return Internal.GetGlyphPNGForActionOrigin( origin, size, 0 );
		}

		/// <summary>
		/// Return an absolute path to the SVF image glyph for the provided digital action name. The current
		/// action set in use for the controller will be used for the lookup. You should cache the result and
		/// maintain your own list of loaded PNG assets.
		/// </summary>
		public static string GetSvgActionGlyph( Controller controller, string action )
		{
			InputActionOrigin origin = InputActionOrigin.None;

			Internal.GetDigitalActionOrigins( controller.Handle, Internal.GetCurrentActionSet( controller.Handle ), GetDigitalActionHandle( action ), ref origin );

			return Internal.GetGlyphSVGForActionOrigin( origin, 0 );
		}
        
		/// <summary>
		/// The typical max value of an unsigned short is 65535, which means the longest haptic pulse you can trigger with this method has a duration of 0.065535 seconds (ie, less than 1/10th of a second).
		/// This function should be thought of as a low-level primitive meant to be repeatedly used in higher-level user functions to generate more sophisticated behavior.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="targetPad"></param>
		/// <param name="durationMicroSec"></param>
		public static void TriggerHapticPulse(Controller controller, SteamControllerPad targetPad, ushort durationMicroSec)
		{
			Internal.Legacy_TriggerHapticPulse( controller.Handle, targetPad, durationMicroSec );
		}
		
		/// <summary>
		/// Trigger a vibration event on supported controllers.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="leftMotor">The typical max value of an unsigned short is 65535</param>
		/// <param name="rightMotor">The typical max value of an unsigned short is 65535</param>
		public static void TriggerVibration(Controller controller, ushort leftMotor, ushort rightMotor)
		{
			Internal.TriggerVibration(controller.Handle, leftMotor, rightMotor );
		}
		
		/// <summary>
		/// Trigger an extended vibration event on supported controllers.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="leftMotor"></param>
		/// <param name="rightMotor"></param>
		/// <param name="leftTrigger"></param>
		/// <param name="rightTrigger"></param>
		public static void TriggerVibrationExtended(Controller controller, ushort leftMotor, ushort rightMotor, ushort leftTrigger, ushort rightTrigger)
		{
			Internal.TriggerVibrationExtended(controller.Handle, leftMotor, rightMotor, leftTrigger, rightTrigger );
		}

		internal static Dictionary<string, InputDigitalActionHandle_t> DigitalHandles = new Dictionary<string, InputDigitalActionHandle_t>();
		internal static InputDigitalActionHandle_t GetDigitalActionHandle( string name )
		{
			if ( DigitalHandles.TryGetValue( name, out var val ) )
				return val;

			val = Internal.GetDigitalActionHandle( name );
			DigitalHandles.Add( name, val );
			return val;
		}

		internal static Dictionary<string, InputAnalogActionHandle_t> AnalogHandles = new Dictionary<string, InputAnalogActionHandle_t>();
		internal static InputAnalogActionHandle_t GetAnalogActionHandle( string name )
		{
			if ( AnalogHandles.TryGetValue( name, out var val ) )
				return val;

			val = Internal.GetAnalogActionHandle( name );
			AnalogHandles.Add( name, val );
			return val;
		}

		internal static Dictionary<string, InputActionSetHandle_t> ActionSets = new Dictionary<string, InputActionSetHandle_t>();
		internal static InputActionSetHandle_t GetActionSetHandle( string name )
		{
			if ( ActionSets.TryGetValue( name, out var val ) )
				return val;

			val = Internal.GetActionSetHandle( name );
			ActionSets.Add( name, val );
			return val;
		}

		public static int GetConnectedControllersCount()
		{
			return SteamInput.Internal.GetConnectedControllers( SteamInput.queryArray );
		}
	}
}
