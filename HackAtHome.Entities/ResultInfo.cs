using System;
namespace HackAtHome.Entities
{
	public class ResultInfo
	{
		public Status Status { get; set; }

		public string Token { get; set; } // El Token expiera depsues de 10min del ultimo acceso al servicio REST

		public string FullName { get; set; }
	}
}
