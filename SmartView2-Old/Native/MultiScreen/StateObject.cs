using System;
using System.Net.Sockets;
using System.Text;

namespace SmartView2.Native.MultiScreen
{
	internal class StateObject
	{
		public const int BufferSize = 1024;

		public Socket workSocket;

		public byte[] buffer = new byte[1024];

		public StringBuilder sb = new StringBuilder();

		public StateObject()
		{
		}
	}
}