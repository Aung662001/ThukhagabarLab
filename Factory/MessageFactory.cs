using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHapi.Base.Model;
using ThukhagabarLab.Builder;
using ThukhagabarLab.Models;

namespace ThukhagabarLab.Factory
{
	public class MessageFactory
	{
		public static IMessage CreateMessage(string messageType)
		{
			//This patterns enables you to build other message types 
			if (messageType.Equals("A28"))
			{
				return new A28Builder().Build();
			}
			else if (messageType.Equals("OUL_R21"))
			{
				return new OML_O21_Test_Builder().Build();
			}

			//if other types of ADT messages are needed, then implement your builders here
			throw new ArgumentException($"'{messageType}' is not supported yet. Extend this if you need to");
		}

	}
}
