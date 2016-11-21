using MediaLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartView2.Native.MediaLibrary
{
	public class TrackList : FileList
	{
		public TrackList()
		{
		}

		public TrackList(IEnumerable<ItemBase> tracks) : base(tracks)
		{
		}

		public override void FillRandomPriority()
		{
			List<int> list = Enumerable.Range(0, this.files.Count).ToList<int>();
			List<int> nums = new List<int>();
			Random random = new Random();
			while (list.Count > 0)
			{
				int num = random.Next(0, list.Count);
				nums.Add(list[num]);
				list.RemoveAt(num);
			}
			for (int i = 0; i < this.files.Count; i++)
			{
				if (this.files[i] is Track)
				{
					((Track)this.files[i]).Priority = nums[i];
				}
			}
		}

		public override ItemBase GetNextFile(ItemBase file, bool? repeat, bool random)
		{
			if (!random)
			{
				return base.GetNextFile(file, repeat, random);
			}
			int priority = ((Track)file).Priority;
			if (priority >= this.files.Count - 1)
			{
				this.FillRandomPriority();
				ItemBase itemBase = this.files.FirstOrDefault<ItemBase>((ItemBase f) => ((Track)f).Priority == 0);
				if (itemBase != null)
				{
					return itemBase;
				}
				return base.GetNextFile(file, repeat, random);
			}
			if (repeat.HasValue && !repeat.Value)
			{
				return file;
			}
			ItemBase itemBase1 = this.files.FirstOrDefault<ItemBase>((ItemBase f) => ((Track)f).Priority == priority + 1);
			if (itemBase1 != null)
			{
				return itemBase1;
			}
			return base.GetNextFile(file, repeat, random);
		}

		public override ItemBase GetPreviousFile(ItemBase file, bool? repeat, bool random)
		{
			if (!random)
			{
				return base.GetPreviousFile(file, repeat, random);
			}
			int priority = ((Track)file).Priority;
			if (priority <= 0)
			{
				this.FillRandomPriority();
				ItemBase itemBase = this.files.FirstOrDefault<ItemBase>((ItemBase f) => ((Track)f).Priority == this.files.Count - 1);
				if (itemBase != null)
				{
					return itemBase;
				}
				return base.GetPreviousFile(file, repeat, random);
			}
			if (repeat.HasValue && !repeat.Value)
			{
				return file;
			}
			ItemBase itemBase1 = this.files.FirstOrDefault<ItemBase>((ItemBase f) => ((Track)f).Priority == priority - 1);
			if (itemBase1 != null)
			{
				return itemBase1;
			}
			return base.GetPreviousFile(file, repeat, random);
		}
	}
}