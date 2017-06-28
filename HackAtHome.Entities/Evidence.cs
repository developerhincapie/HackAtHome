using System;
namespace HackAtHome.Entities
{
	public class Evidence
	{
		// Identificador de la evidencia
		public int EvidenceID { get; set; }

		// Titulo de la evidencia
		public string Title { get; set; }

		// Estatus de la evidencia
		public string Status { get; set; }
	}
}
