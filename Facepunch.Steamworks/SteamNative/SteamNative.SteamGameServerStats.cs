using System;
using System.Runtime.InteropServices;

namespace SteamNative
{
	public unsafe class SteamGameServerStats : IDisposable
	{
		//
		// Holds a platform specific implentation
		//
		internal Platform.Interface _pi;
		
		//
		// Constructor decides which implementation to use based on current platform
		//
		public SteamGameServerStats( IntPtr pointer )
		{
			if ( Platform.IsWindows64 ) _pi = new Platform.Win64( pointer );
			else if ( Platform.IsWindows32 ) _pi = new Platform.Win32( pointer );
			else if ( Platform.IsLinux32 ) _pi = new Platform.Linux32( pointer );
			else if ( Platform.IsLinux64 ) _pi = new Platform.Linux64( pointer );
			else if ( Platform.IsOsx ) _pi = new Platform.Mac( pointer );
		}
		
		//
		// Class is invalid if we don't have a valid implementation
		//
		public bool IsValid{ get{ return _pi != null && _pi.IsValid; } }
		
		//
		// When shutting down clear all the internals to avoid accidental use
		//
		public virtual void Dispose()
		{
			 if ( _pi != null )
			{
				_pi.Dispose();
				_pi = null;
			}
		}
		
		// bool
		public bool ClearUserAchievement( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/ )
		{
			return _pi.ISteamGameServerStats_ClearUserAchievement( steamIDUser.Value /*C*/, pchName /*C*/ );
		}
		
		// bool
		public bool GetUserAchievement( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, ref bool pbAchieved /*bool **/ )
		{
			return _pi.ISteamGameServerStats_GetUserAchievement( steamIDUser.Value /*C*/, pchName /*C*/, ref pbAchieved /*A*/ );
		}
		
		// bool
		public bool GetUserStat( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, out int pData /*int32 **/ )
		{
			return _pi.ISteamGameServerStats_GetUserStat( steamIDUser.Value /*C*/, pchName /*C*/, out pData /*B*/ );
		}
		
		// bool
		public bool GetUserStat0( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, out float pData /*float **/ )
		{
			return _pi.ISteamGameServerStats_GetUserStat0( steamIDUser.Value /*C*/, pchName /*C*/, out pData /*B*/ );
		}
		
		// SteamAPICall_t
		public SteamAPICall_t RequestUserStats( CSteamID steamIDUser /*class CSteamID*/ )
		{
			return _pi.ISteamGameServerStats_RequestUserStats( steamIDUser.Value /*C*/ );
		}
		
		// bool
		public bool SetUserAchievement( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/ )
		{
			return _pi.ISteamGameServerStats_SetUserAchievement( steamIDUser.Value /*C*/, pchName /*C*/ );
		}
		
		// bool
		public bool SetUserStat( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, int nData /*int32*/ )
		{
			return _pi.ISteamGameServerStats_SetUserStat( steamIDUser.Value /*C*/, pchName /*C*/, nData /*C*/ );
		}
		
		// bool
		public bool SetUserStat0( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, float fData /*float*/ )
		{
			return _pi.ISteamGameServerStats_SetUserStat0( steamIDUser.Value /*C*/, pchName /*C*/, fData /*C*/ );
		}
		
		// SteamAPICall_t
		public SteamAPICall_t StoreUserStats( CSteamID steamIDUser /*class CSteamID*/ )
		{
			return _pi.ISteamGameServerStats_StoreUserStats( steamIDUser.Value /*C*/ );
		}
		
		// bool
		public bool UpdateUserAvgRateStat( CSteamID steamIDUser /*class CSteamID*/, string pchName /*const char **/, float flCountThisSession /*float*/, double dSessionLength /*double*/ )
		{
			return _pi.ISteamGameServerStats_UpdateUserAvgRateStat( steamIDUser.Value /*C*/, pchName /*C*/, flCountThisSession /*C*/, dSessionLength /*C*/ );
		}
		
	}
}
