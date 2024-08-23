using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapiTools.Base;
using NHapiTools.Base.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThukhagabarLab
{
	internal class Connection
	{
		private SimpleMLLPClient _connection;
		private readonly string _host;
		private readonly int _port;
		
		public Connection (string host,int port)
		{
			_host = host;
			_port = port;
		}

		public SimpleMLLPClient Connect ()
		{
            Console.WriteLine("connection:"+ _connection);
            if (_connection == null)
			{
				_connection = new SimpleMLLPClient(_host, _port);
			}
			return _connection;
		}
	}
}
