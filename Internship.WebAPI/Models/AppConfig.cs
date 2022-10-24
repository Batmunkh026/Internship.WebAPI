using System;
namespace Internship.WebAPI.Models
{
	public class AppConfig
	{
        public virtual string? dbhost { get; set; }
        public virtual string? schema { get; set; }
        public virtual string? rowNumber { get; set; }
    }
}

