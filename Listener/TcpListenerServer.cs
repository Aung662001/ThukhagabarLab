using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ThukhagabarLab.Listener
{
	internal class TcpListenerServer
	{
		private static char END_OF_BLOCK = '\u001c';
		private static char START_OF_BLOCK = '\u000b';
		private static char CARRIAGE_RETURN = (char)13;

		public static void OpenListener()
		{
			TcpListener tcpListener;
			try
			{
				tcpListener = new TcpListener(IPAddress.Loopback,6666);
                Console.WriteLine("\nStarting server...");
                tcpListener.Start();
                Console.WriteLine("Server is ready to receive message.");
            }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
			var receivedByteBuffer = new byte[200];
			//200 bytes is allocated to hold the incoming data from a client connection

			for (; ; )
			{
				TcpClient acceptTcpClient = null;
				NetworkStream netStream = null;
				try
				{
					acceptTcpClient = tcpListener.AcceptTcpClient(); 
					netStream = acceptTcpClient.GetStream();


					var totalBytesReceivedFromClient = 0;
					int bytesReceived; 
					while ((bytesReceived = netStream.Read(receivedByteBuffer, 0, receivedByteBuffer.Length)) > 0)
					{
						totalBytesReceivedFromClient += bytesReceived;
					}

					Console.WriteLine("Echoed {0} bytes back to the client.", totalBytesReceivedFromClient);
				}
				catch (Exception e)
				{
					//print any exceptions during the communications to the console
					string directoryPath = "C:\\Users\\setan\\Desktop\\HL7TestOutputs\\";
					string filePath = Path.Combine(directoryPath, "error_log.txt");
					if (!Directory.Exists(directoryPath))
					{
						Directory.CreateDirectory(directoryPath);
					}
					File.AppendAllText(filePath, $"{DateTime.Now}: {e.Message}{Environment.NewLine}{e.StackTrace}{Environment.NewLine}");
					Console.WriteLine(e.Message);
				}
				finally
				{
					netStream?.Close();
					netStream?.Dispose();
					acceptTcpClient?.Close();
				}
			}
		}
		private string GetMessageControlID(string incomingHl7Message)
		{

			var fieldCount = 0;
			//parse the message into segments using the end of segment separter
			var hl7MessageSegments = incomingHl7Message.Split(CARRIAGE_RETURN);

			//tokenize the MSH segment into fields using the field separator
			var hl7FieldsInMshSegment = hl7MessageSegments[0].Split("|");

			//retrieve the message control ID in order to reply back with the message ack
			foreach (var field in hl7FieldsInMshSegment)
			{
				if (fieldCount == 4) //MESSAGE_CONTROL_ID_LOCATION
				{
					return field;
				}
				fieldCount++;
			}

			return string.Empty; //you can also throw an exception here if you wish
		}
	}
}
