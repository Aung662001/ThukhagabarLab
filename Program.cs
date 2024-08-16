using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapiTools.Base.Net;
using System.Globalization;
using ThukhagabarLab.Factory;

namespace ThukhagabarLab
{
	public class Program
	{
		static void Main(string[] args)
		{
			try
			{
				IMessage adtMessage;
				bool loop = true;
				// create the HL7 message
				while (loop)
				{
					string operation = AskOperationToMake();
					if (operation == "A28") { 
						 adtMessage = MessageFactory.CreateMessage("A28");
						MakeOperation(adtMessage);
						loop = AskMoreOperationNeed();
					}else if(operation == "OUL_R21")
					{
						adtMessage = MessageFactory.CreateMessage("OUL_R21");
						MakeOperation(adtMessage);
						loop = AskMoreOperationNeed();
					}
					else
					{
						Console.WriteLine("Operation Not Found.Please Restarting Again.");
					}
				}

				
			}
			catch (Exception e)
			{
				LogToDebugConsole($"Error occured while creating HL7 message {e.Message}");
			}
		}

		private static void WriteMessageFile(ParserBase parser, IMessage hl7Message, string outputDirectory, string outputFileName)
		{
			if (!Directory.Exists(outputDirectory))
				Directory.CreateDirectory(outputDirectory);

			var fileName = Path.Combine(outputDirectory, outputFileName);

			LogToDebugConsole("Writing data to file...");

			if (File.Exists(fileName))
				File.Delete(fileName);
			File.WriteAllText(fileName, parser.Encode(hl7Message));
			LogToDebugConsole($"Wrote data to file {fileName} successfully...");
		}

		private static void LogToDebugConsole(string informationToLog)
		{
			Debug.WriteLine(informationToLog);
		}

		private static void SendMessage(IMessage hl7Message, string host, int port)
		{
			var connection = new SimpleMLLPClient(host, port);
			var parser = new PipeParser();
			try
			{
				LogToDebugConsole("Sending message:\n" + parser.Encode(hl7Message));
				IMessage response = connection.SendHL7Message(hl7Message);
				LogToDebugConsole("Received Response: \n" + parser.Encode(response));
			}
			catch (Exception e)
			{
				LogToDebugConsole(e.Message);
			}
			finally
			{
			}
		}
	
		private static string AskOperationToMake()
		{
			string operation = "";
            Console.WriteLine("1. Add Patient");
            Console.WriteLine("2. Order A Test");
            Console.Write("Please select one number of operations Above: ");
			int op_no =Convert.ToInt32(Console.ReadLine());
			if(op_no == 1)
			{
				operation = "A28";
			}else if(op_no == 2)
			{
				operation = "OUL_R21";
			}
			return operation;
		}
	
		private static void MakeOperation (IMessage adtMessage)
		{
            Console.WriteLine("Sending to LIS. Please Wait....");
            // create these parsers for the file encoding operations
            var pipeParser = new PipeParser();
			var xmlParser = new DefaultXMLParser();

			// print out the message that we constructed
			LogToDebugConsole("Message was constructed successfully..." + "\n");

			// serialize the message to pipe delimited output file C:\Users\setan\Desktop
			WriteMessageFile(pipeParser, adtMessage, "C:\\Users\\setan\\Desktop\\HL7TestOutputs", "testPipeDelimitedOutputFile.txt");

			// serialize the message to XML format output file
			WriteMessageFile(xmlParser, adtMessage, "C:\\Users\\setan\\Desktop\\HL7TestOutputs", "testXmlOutputFile.xml");

			//send to LIS
			SendMessage(adtMessage, "195.0.0.239", 20860);
		}
		private static bool AskMoreOperationNeed()
		{
			bool more = true;
            Console.Write("More Operation? Y for Yes , N for No :");
			string answer = Console.ReadLine().ToUpper();
			
			if(answer == "N")
			{
				more = false;
			}
			return more;
        }
	}
}