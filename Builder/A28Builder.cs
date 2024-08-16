using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V23.Segment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThukhagabarLab.Models;

namespace ThukhagabarLab.Builder
{
	public class A28Builder
	{
		public IMessage Build()
		{
			OrderInfo orderinfo = AskForReq();
			var currenttime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			string message = $@"MSH|^~\&|Monicare||LIS||{currenttime}||ADT^A28|{currenttime}|P|2.5
							EVN|A28|{currenttime}
							PID|{orderinfo.Pid}|0000299|||^{orderinfo.Name}||{orderinfo.Dob}|M";

			PipeParser parser = new();
			IMessage hl7Message = parser.Parse(message);
			return hl7Message;
		}
		private static OrderInfo AskForReq()
		{
			Console.Write("Patient Id: ");
			int id = Convert.ToInt32(Console.ReadLine());
			Console.Write("Patient Name: ");
			string name = Console.ReadLine();
			Console.WriteLine("Date of birth with yyyymmdd format: ");
			string dob = Console.ReadLine();
			OrderInfo orderinfo = new OrderInfo(
				pid: id,
				name: name,
				dob: dob
				);
			return orderinfo;
		}
	}
}
