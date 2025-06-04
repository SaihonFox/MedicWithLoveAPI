using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.Hubs;

public interface IRequestHub
{
	Task RequestAdded(Request analysis);

	Task RequestUpdated(Request analysis);
}