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
		
		[DllImport("user32.dll", EntryPoint="FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);		
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool InvalidateRect(IntPtr hWnd, ref System.Drawing.Rectangle lpRect, bool bErase); 		
		
    	[DllImport("user32.dll",EntryPoint="GetDC")]
    	public static extern IntPtr GetDC(IntPtr ptr);		
				
		public Win32Utils()
		{
		}
	}
}
