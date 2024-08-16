using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThukhagabarLab.Models
{
	public class OrderInfo
	{
		public int Pid { get; set; }
		public string Name { get; set; }
		public string Dob { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Religion { get; set; }
		public string PatientComment { get; set; }
		public string TestName { get; set; }
		public int OrderCode { get; set; }
		public string Gender { get; set; }

		public OrderInfo(int pid, string name, string dob,string gender,int orderCode, string address = "", string city = "", string province = "", string zipCode = "", string country = "", string phone = "", string religion = "", string patientComment = "", string test = "")
		{
			Pid = pid;
			Name = name;
			Dob = dob;
			Gender = gender;
			Address = address;
			City = city;
			Province = province;
			ZipCode = zipCode;
			Country = country;
			Phone = phone;
			Religion = religion;
			PatientComment = patientComment;
			TestName = test;
			OrderCode = orderCode;
		}
	}
}
