using SmartView2.Controls;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SmartView2.Native.Player
{
	public class CaptionEventArgs
	{
		public Point WindowPos
		{
			get;
			internal set;
		}

		public CaptionWindow WindowRef
		{
			get;
			internal set;
		}

		public CaptionEventArgs()
		{
		}
	}
}