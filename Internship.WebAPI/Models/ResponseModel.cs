using System;
namespace Internship.WebAPI.Models
{
	public class ResponseModel
	{
		public virtual bool isSuccess { get; set; }
		public virtual string? errorCode { get; set; } = "200";
		public virtual string? resultMessage { get; set; } = "success";
		public virtual List<SmartCard>? cards { get; set; }
	}
	public class SmartCard
	{
		public virtual string? cardNo { get; set; }
	}
}

