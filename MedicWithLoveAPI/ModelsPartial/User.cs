using Newtonsoft.Json;

namespace MedicWithLoveAPI.Models;

public partial class User
{
	[JsonIgnore]
	public string FullName => $"{Surname} {Name}{(Patronym != null ? " " + Patronym : "")}";
}