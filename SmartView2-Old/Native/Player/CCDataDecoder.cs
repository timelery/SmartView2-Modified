using SmartView2.Controls;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SmartView2.Native.Player
{
	public class CCDataDecoder
	{
		private Caption current;

		private List<Caption> captions;

		public CCDataDecoder()
		{
			this.captions = new List<Caption>();
		}

		private void OnHideCaption(object sender, CaptionEventArgs e)
		{
			if (this.HideCaption != null)
			{
				this.HideCaption(sender, e);
			}
		}

		private void OnShowCaption(object sender, CaptionEventArgs e)
		{
			if (this.ShowCaption != null)
			{
				this.ShowCaption(sender, e);
			}
		}

		public void ParseData(byte[] data)
		{
			switch (data[1])
			{
				case 0:
				case 1:
				{
					if (this.current == null)
					{
						break;
					}
					this.current.PutChar(CCDataCodeTable.CodeTable[data[2]]);
					return;
				}
				case 2:
				case 3:
				case 4:
				{
					if (data[1] < 152 || data[1] > 159)
					{
						break;
					}
					Caption caption1 = this.captions.Find((Caption o) => o.ID == data[1] - 151);
					if (caption1 != null)
					{
						this.current = caption1;
						this.current.ResizeWindow((data[5] & 15) + 1, (data[6] & 63) + 1);
						this.current.Position = new Point((double)data[4], (double)(data[3] & 127));
					}
					else
					{
						this.current = new Caption(data[1] - 151, (int)data[4], data[3] & 127, (data[5] & 15) + 1, (data[6] & 63) + 1, (data[2] & 32) > 0);
						this.captions.Add(this.current);
					}
					if (!this.current.Visible || this.current.WindowRef != null)
					{
						break;
					}
					Application.Current.Dispatcher.Invoke(() => {
						this.current.WindowRef = new CaptionWindow()
						{
							Text = this.current.Text
						};
						this.OnShowCaption(this, new CaptionEventArgs()
						{
							WindowRef = this.current.WindowRef,
							WindowPos = this.current.Position
						});
					});
					break;
				}
				case 5:
				{
					if (data[2] != 13)
					{
						return;
					}
					if (this.current == null)
					{
						break;
					}
					this.current.PutChar('\n');
					return;
				}
				case 6:
				{
					CCDataCodeTable.ControlCode controlCode = (CCDataCodeTable.ControlCode)data[2];
					switch (controlCode)
					{
						case CCDataCodeTable.ControlCode.TGW:
						{
							Application.Current.Dispatcher.Invoke(() => {
								foreach (Caption captionWindow in this.captions.FindAll((Caption o) => (o.ID & data[3]) > 0))
								{
									if (!captionWindow.Visible)
									{
										captionWindow.WindowRef = new CaptionWindow()
										{
											Text = captionWindow.Text
										};
										captionWindow.Visible = true;
										this.OnShowCaption(this, new CaptionEventArgs()
										{
											WindowRef = captionWindow.WindowRef,
											WindowPos = captionWindow.Position
										});
									}
									else
									{
										this.OnHideCaption(this, new CaptionEventArgs()
										{
											WindowRef = captionWindow.WindowRef,
											WindowPos = captionWindow.Position
										});
									}
								}
							});
							return;
						}
						case CCDataCodeTable.ControlCode.DLW:
						{
							Application.Current.Dispatcher.Invoke(() => {
								foreach (Caption caption in this.captions.FindAll((Caption o) => (o.ID & data[3]) > 0))
								{
									this.OnHideCaption(this, new CaptionEventArgs()
									{
										WindowRef = caption.WindowRef,
										WindowPos = caption.Position
									});
									this.captions.Remove(caption);
								}
							});
							return;
						}
						default:
						{
							if (controlCode != CCDataCodeTable.ControlCode.SPL)
							{
								return;
							}
							if (this.current == null)
							{
								return;
							}
							this.current.SetCursorPos(data[3] & 15, data[4] & 63);
							return;
						}
					}
					break;
				}
				default:
				{
					goto case 4;
				}
			}
		}

		public event EventHandler<CaptionEventArgs> HideCaption;

		public event EventHandler<CaptionEventArgs> ShowCaption;
	}
}