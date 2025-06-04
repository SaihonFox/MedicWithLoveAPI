using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;

namespace MedicWithLoveAPI.Hubs;

public interface IUserHub
{
	Task UserUpdated(User user);
}