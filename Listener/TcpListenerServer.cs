using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;

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
                Console.WriteLine("\nStarting server at {0}:{1}...",IPAddress.Loopback,6666);
                tcpListener.Start();
                Console.WriteLine("Server is ready to receive message.");
            }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
			var receivedByteBuffer = new byte[1024];
			//200 bytes is allocated to hold the incoming data from a client connection

			for (; ; )
			{
				TcpClient acceptTcpClient = null;
				NetworkStream netStream = null;
				try
				{
					acceptTcpClient = tcpListener.AcceptTcpClient(); 
					netStream = acceptTcpClient.GetStream();


					int bytesReceived;
					var hl7Data = string.Empty;
					while ((bytesReceived = netStream.Read(receivedByteBuffer, 0, receivedByteBuffer.Length)) > 0)
					{
						hl7Data += Encoding.UTF8.GetString(receivedByteBuffer, 0, bytesReceived);

						PipeParser parser = new();
						IMessage hl7Message = parser.Parse(hl7Data);
						var mshSegment = (MSH)hl7Message.GetStructure("MSH");
						ORU_R01 oruMessage = (ORU_R01)hl7Message;
                        Console.WriteLine("___________________");
                        if (oruMessage != null)
						{
							// Access MSH Segment
							MSH msh = oruMessage.MSH;
							Console.WriteLine($"Message Type: {msh.MessageType.MessageCode.Value}^{msh.MessageType.TriggerEvent.Value}");

							// Access PID Segment
							var patientResult = oruMessage.GetPATIENT_RESULT();
							PID pid = patientResult.PATIENT.PID;
							Console.WriteLine($"Patient ID: {pid.GetPatientIdentifierList(0).IDNumber.Value}");
							Console.WriteLine($"Patient Name: {pid.GetPatientName(0).FamilyName.Surname.Value}, {pid.GetPatientName(0).GivenName.Value}");

							for (int i = 0; i < patientResult.ORDER_OBSERVATIONRepetitionsUsed; i++)
							{
								var orderObservation = patientResult.GetORDER_OBSERVATION(i);
								ORC orc = orderObservation.ORC;
								OBR obr = orderObservation.OBR;


								//Console.WriteLine($"Order Control: {orc.GetField(1).GetValue(0)}");
								Console.WriteLine($"Order Control: {orc.OrderControl.Value}");
								Console.WriteLine($"Placer order Id: {orc.PlacerOrderNumber.EntityIdentifier.Value}");
								Console.WriteLine($"Observation Request: {obr.UniversalServiceIdentifier.Identifier.Value}");

								for (int j = 0; j < orderObservation.OBSERVATIONRepetitionsUsed; j++)
								{
									var observation = orderObservation.GetOBSERVATION(j);
									OBX obx = observation.OBX;

									Console.WriteLine($"Observation ID: {obx.ObservationIdentifier.Identifier.Value}");
									Console.WriteLine($"Observation Value: {obx.GetObservationValue(0).Data.ToString()}");
									Console.WriteLine($"Units: {obx.Units.Identifier.Value}");
									Console.WriteLine($"Reference Range: {obx.ReferencesRange.Value}");
									Console.WriteLine($"Result Status: {obx.ObservationResultStatus.Value}");

									// Access NTE segments if present
									for (int k = 0; k < observation.NTERepetitionsUsed; k++)
									{
										NTE nte = observation.GetNTE(k);
										Console.WriteLine($"Note: {nte.GetComment(0).Value}");
									}
								}
							}
						}
						else
						{
							Console.WriteLine("Received message is not an ORU_R01 message.");
						}
						var messageType = mshSegment.MessageType.MessageCode.Value;
						var messageTriggerEvent = mshSegment.MessageType.TriggerEvent.Value;
                        Console.WriteLine("Message type: "+ messageType+"^"+messageTriggerEvent);
						//Console.WriteLine("hl7Message"+ hl7Message);
						Console.WriteLine("___________________");
					}
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
		
	}
}
