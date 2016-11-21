using SmartView2.Controls;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SmartView2.Native.Player
{
	public class Caption
	{
		private char[,] text;

		private int cursorRow;

		private int cursorCol;

		public int ID
		{
			get;
			set;
		}

		public Point Position
		{
			get;
			internal set;
		}

		public string Text
		{
			get
			{
				return this.GetString();
			}
		}

		public bool Visible
		{
			get;
			internal set;
		}

		public int WindowCols
		{
			get;
			internal set;
		}

		public CaptionWindow WindowRef
		{
			get;
			set;
		}

		public int WindowRows
		{
			get;
			internal set;
		}

		public Caption(int id, int x, int y, int rows, int cols, bool visibile)
		{
			this.text = new char[rows, cols];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					this.text[i, j] = ' ';
				}
			}
			this.cursorRow = 0;
			this.cursorCol = 0;
			this.ID = id;
			this.Position = new Point((double)x, (double)y);
			this.WindowRows = rows;
			this.WindowCols = cols;
			this.Visible = visibile;
		}

		private string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.WindowRows; i++)
			{
				char[] chrArray = new char[this.WindowCols];
				Buffer.BlockCopy(this.text, i * this.WindowCols * 2, chrArray, 0, this.WindowCols * 2);
				stringBuilder.Append((new string(chrArray)).Trim());
				if (i != this.WindowRows - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		internal void PutChar(char ch)
		{
			if (ch == '\n')
			{
				Caption caption = this;
				caption.cursorRow = caption.cursorRow + 1;
				if (this.cursorRow >= this.WindowRows)
				{
					this.ScrollText(this.cursorRow - this.WindowRows + 1);
					Caption windowRows = this;
					windowRows.cursorRow = windowRows.cursorRow - (this.cursorRow - this.WindowRows + 1);
				}
				return;
			}
			if (this.cursorCol >= this.WindowCols)
			{
				this.cursorCol = 0;
				Caption caption1 = this;
				caption1.cursorRow = caption1.cursorRow + 1;
			}
			if (this.cursorRow >= this.WindowRows)
			{
				this.ScrollText(this.cursorRow - this.WindowRows + 1);
				Caption windowRows1 = this;
				windowRows1.cursorRow = windowRows1.cursorRow - (this.cursorRow - this.WindowRows + 1);
			}
			this.text[this.cursorRow, this.cursorCol] = ch;
			Caption caption2 = this;
			caption2.cursorCol = caption2.cursorCol + 1;
			if (this.WindowRef != null)
			{
				this.WindowRef.Dispatcher.Invoke<string>(() => {
					CaptionWindow windowRef = this.WindowRef;
					string text = this.Text;
					string str = text;
					windowRef.Text = text;
					return str;
				});
			}
		}

		internal void ResizeWindow(int rows, int cols)
		{
			if (this.text.Length < rows * cols)
			{
				char[,] chrArray = new char[rows, cols];
				Array.Copy(this.text, chrArray, this.text.Length);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < cols; j++)
					{
						if (chrArray[i, j] == 0)
						{
							chrArray[i, j] = ' ';
						}
					}
				}
				this.text = chrArray;
			}
			this.WindowRows = rows;
			this.WindowCols = cols;
		}

		private void ScrollText(int rows)
		{
			if (rows > this.WindowRows)
			{
				rows = this.WindowRows;
			}
			for (int i = 0; i < this.WindowRows - rows; i++)
			{
				for (int j = 0; j < this.WindowCols; j++)
				{
					this.text[i, j] = this.text[i + rows, j];
				}
			}
			for (int k = this.WindowRows - rows; k < this.WindowRows; k++)
			{
				for (int l = 0; l < this.WindowCols; l++)
				{
					this.text[k, l] = ' ';
				}
			}
		}

		internal void SetCursorPos(int row, int col)
		{
			this.cursorRow = row;
			this.cursorCol = col;
		}
	}
}