using System;
using System.Runtime.CompilerServices;
using TagLib;

namespace SmartView2.Native.MediaLibrary
{
	internal class MidiTag : Tag
	{
		public override TagLib.TagTypes TagTypes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override string Title
		{
			get;
			set;
		}

		public MidiTag()
		{
		}

		public override void Clear()
		{
			throw new NotImplementedException();
		}
	}
}