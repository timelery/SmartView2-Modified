using MediaLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UIFoundation;

namespace SmartView2.Native.MediaLibrary
{
	public class FileList : BindableBase
	{
		protected List<ItemBase> files;

		public int Count
		{
			get
			{
				return this.files.Count;
			}
		}

		public List<ItemBase> Files
		{
			get
			{
				return this.files;
			}
		}

		public ItemBase this[int index]
		{
			get
			{
				return this.files[index];
			}
		}

		public FileList()
		{
			this.files = new List<ItemBase>();
		}

		public FileList(IEnumerable<ItemBase> files)
		{
			this.files = files.ToList<ItemBase>();
		}

		public virtual void FillRandomPriority()
		{
		}

		public IEnumerable<ItemBase> GetFilesOrderByAscending()
		{
			this.files = new List<ItemBase>((
				from f in this.files
				orderby f.Name
				select f).ToList<ItemBase>());
			base.RisePropertyChanged("Files");
			return this.files;
		}

		public IEnumerable<ItemBase> GetFilesOrderByDescending()
		{
			this.files = new List<ItemBase>((
				from f in this.files
				orderby f.Name descending
				select f).ToList<ItemBase>());
			base.RisePropertyChanged("Files");
			return this.files;
		}

		public virtual ItemBase GetNextFile(ItemBase file, bool? repeat, bool random)
		{
			if (repeat.HasValue && !repeat.Value)
			{
				return file;
			}
			int num = this.files.IndexOf(file);
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("File is not from this list.");
			}
			if (!this.isLast(file))
			{
				return this.files[num + 1];
			}
			if (!repeat.HasValue || !repeat.Value)
			{
				throw new IndexOutOfRangeException("Current index is the last.");
			}
			return this.files[0];
		}

		public virtual ItemBase GetPreviousFile(ItemBase file, bool? repeat, bool random)
		{
			if (repeat.HasValue && !repeat.Value)
			{
				return file;
			}
			int num = this.files.IndexOf(file);
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("File is not from this list.");
			}
			if (!this.isFirst(file))
			{
				return this.files[num - 1];
			}
			if (!repeat.HasValue || !repeat.Value)
			{
				throw new IndexOutOfRangeException("Current index is the first.");
			}
			return this.files[this.files.Count - 1];
		}

		public int IndexOf(ItemBase file)
		{
			return this.files.IndexOf(file);
		}

		public bool isFirst(ItemBase file)
		{
			return this.files.IndexOf(file) == 0;
		}

		public bool isLast(ItemBase file)
		{
			int num = this.files.IndexOf(file);
			return num == this.files.Count - 1;
		}
	}
}