/*
 * Creato da SharpDevelop.
 * Utente: Family Rose
 * Data: 26/12/2008
 * Ora: 11.48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;


namespace mkdb
{
	/// <summary>
	/// Description of Win32Utils.
	/// </summary>
	public class Win32Utils
	{
		[DllImport("user32.dll", SetLastError = true)] 
		public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);		
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		
		// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
		// Also consider whether you're being lazy or not.
		[DllImport("user32.dll", EntryPoint="FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);		
				
		public Win32Utils()
		{
		}
	}
}
