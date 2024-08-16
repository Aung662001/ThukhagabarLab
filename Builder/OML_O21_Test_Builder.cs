using NHapi.Base.Model;
using NHapi.Base.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThukhagabarLab.Models;

namespace ThukhagabarLab.Builder
{
	public class OML_O21_Test_Builder
	{

		public IMessage Build()
		{
			OrderInfo orderinfo = AskForReq();
			var currenttimestamp = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			var message = $@"MSH|^~\&|||||{currenttimestamp}||OML^O21|{currenttimestamp}|T|2.5|||AL|ER
				PID|||{orderinfo.Pid}|| ^{orderinfo.Name}^^^^^^{orderinfo.Dob}|||{orderinfo.Gender}
				NTE|1||{orderinfo.PatientComment}| 
				ORC|NW|{currenttimestamp}|||||||{currenttimestamp}|||00003322^Dr. ArkarMoe / Cardiologic
				OBR|1|||{orderinfo.OrderCode}|{orderinfo.TestName}||||||||||Blood";

			PipeParser parser = new();
			IMessage hl7Message = parser.Parse(message);
			//string encodedMessage = parser.Encode(hl7Message);

			return hl7Message;

		}
		private static OrderInfo AskForReq()
		{
			string testName = AskTestType();
			int orderCode=0;
			string gender = "M";
			if(testName == "Glucose")
			{
				orderCode = 1017;
			}else if(testName == "Blood Group")
			{
				orderCode = 6204;
			}
			Console.Write("Patient Id: ");
			int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Patient Name: ");
			string name = Console.ReadLine();
            Console.Write("Date of birth with yyyymmdd format: ");
			string dob = Console.ReadLine();
            Console.Write("Gender 1.Male 2:Female: ");
			int gen = Convert.ToInt32(Console.ReadLine());
			if(gen == 2 ) { gender = "F"; }
            OrderInfo orderinfo = new OrderInfo(
				pid: id,
				name: name,
				dob: dob,
				gender: gender,
				test:testName,
				orderCode:orderCode
				);
			return orderinfo;
        }
		private static string AskTestType()
		{
			string testType = "";
			Console.WriteLine("1. Glucose");
			Console.WriteLine("2. Blood Group");
			Console.Write("Please One Of Test Tpye Above:");
			int type = Convert.ToInt32(Console.ReadLine());
			if (type == 1)
			{
				testType = "Glucose";
			}
			else if (type == 2)
			{
				testType = "Blood Group";
			}
			else
			{
                Console.WriteLine("Test Not Found. Select Again.");
				AskTestType();
            }
			return testType;
		}

	}
}
