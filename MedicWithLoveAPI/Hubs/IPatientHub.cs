using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.Hubs;

public interface IPatientHub
{
	Task PatientAdded(Patient analysis);

	Task PatientUpdated(Patient analysis);

	Task PatientDeleted(Patient analysis);
}