using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TagLib;

namespace SmartView2.Native.MediaLibrary
{
	internal class MidiFile : File
	{
		private TagLib.Tag tag;

		private TagLib.Properties properties;

		public override TagLib.Properties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public override TagLib.Tag Tag
		{
			get
			{
				return this.tag;
			}
		}

		public MidiFile(string name) : base("")
		{
			this.tag = new MidiTag();
			TagLib.Tag tag = this.tag;
			char[] chrArray = new char[] { '\\' };
			tag.Title = name.Split(chrArray).Last<string>();
			MediaPlayer mediaPlayer = new MediaPlayer();
			mediaPlayer.Open(new Uri(name));
			while (!mediaPlayer.NaturalDuration.HasTimeSpan)
			{
			}
			Duration naturalDuration = mediaPlayer.NaturalDuration;
			this.properties = new TagLib.Properties(naturalDuration.TimeSpan, new ICodec[0]);
		}

		public override TagLib.Tag GetTag(TagLib.TagTypes type, bool create)
		{
			return this.tag;
		}

		public override void RemoveTags(TagLib.TagTypes types)
		{
			throw new NotImplementedException();
		}

		public override void Save()
		{
			throw new NotImplementedException();
		}
	}
}