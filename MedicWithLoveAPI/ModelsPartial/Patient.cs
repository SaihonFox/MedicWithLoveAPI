using Newtonsoft.Json;

namespace MedicWithLoveAPI.Models;

public partial class Patient
{
	[JsonIgnore]
	public string FullName => $"{Surname} {Name}{(Patronym != null ? " " + Patronym : "")}";
}