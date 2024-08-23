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
		//var message = $@"MSH|^~\&|||||{currenttimestamp}||OML^O21|{currenttimestamp}|T|2.5|||AL|ER
		//		PID|||{orderinfo.Pid}|| ^{orderinfo.Name}^^^^^^{orderinfo.Dob}|||{orderinfo.Gender}
		//		NTE|1||{orderinfo.PatientComment}| 
		//		ORC|NW|{currenttimestamp}|||||||{currenttimestamp}|||00003322^Dr. ArkarMoe / Cardiologic
		//		OBR|1|||{orderinfo.OrderCode}|{orderinfo.TestName}||||||||||Blood";
		public IMessage Build()
		{
			OrderInfo orderinfo = AskTestTypeAndGetOrderInfo();
			var currenttimestamp = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			var message = $@"MSH|^~\&|||||{currenttimestamp}||OML^O21|{currenttimestamp}|T|2.5|||AL|ER
				PID|||{orderinfo.Pid}|| ^{orderinfo.Name}^^^^^^{orderinfo.Dob}|||{orderinfo.Gender}
				ORC|NW|CSH123456789|||||||{currenttimestamp}|||
				OBR|1|||{orderinfo.OrderCode}|{orderinfo.TestName}||||||||||Blood";

			PipeParser parser = new();
			IMessage hl7Message = parser.Parse(message);
			//string encodedMessage = parser.Encode(hl7Message);

			return hl7Message;

		}
		
		private static OrderInfo AskTestTypeAndGetOrderInfo()
		{
			int testCode = 0;
			string testName = "";
			string gender = "M";

			Console.WriteLine("1. Glucose");
			Console.WriteLine("2. ABO Grouping");
			Console.WriteLine("3. Rh Grouping");
			Console.WriteLine("4. Sodium (N+)");
			Console.WriteLine("5. Potassium (K+)"); 
			Console.WriteLine("6. Chloride (CL)"); 
			Console.WriteLine("7. Bicarbonate(HCO3)"); 
			Console.Write("Please One Of Test Tpye Above:");
			int type = Convert.ToInt32(Console.ReadLine());
			switch (type){
				case 1:
					testCode = 1017;
					testName = "Glucose";
					break;
				case 2:
					testCode = 6204;
					testName = "ABO Grouping";
					break;
				case 3:
					testCode = 6211;
					testName = "Rh Grouping";
					break;
				case 4:
					testCode = 1000;
					testName = "Sodium (N+)";
					break;
				case 5:
					testCode = 1001;
					testName = "Potassium(K +)";
					break;
				case 6:
					testCode = 1002;
					testName = "Chloride (CL)";
					break;
				case 7:
					testCode = 1002;
					testName = "Bicarbonate (HCO3)";
					break;
				default:
					Console.WriteLine("Test Not Found. Select Again.");
					AskTestTypeAndGetOrderInfo();
					break;
			}

			Console.Write("Patient Id: ");
			int id = Convert.ToInt32(Console.ReadLine());
			Console.Write("Patient Name: ");
			string name = Console.ReadLine();
			Console.Write("Date of birth with yyyymmdd format: ");
			string dob = Console.ReadLine();
			Console.Write("Gender 1.Male 2:Female: ");
			int gen = Convert.ToInt32(Console.ReadLine());
			if (gen == 2) { gender = "F"; }
			OrderInfo orderinfo = new OrderInfo(
				pid: id,
				name: name,
				dob: dob,
				gender: gender,
				test: testName,
				orderCode: testCode
				);
			return orderinfo;
		}

	}
}
