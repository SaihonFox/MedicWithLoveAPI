using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class PatientDTO
{
	public int Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronym { get; set; }

	public DateOnly Birthday { get; set; }

	public string? Passport { get; set; }

	public string? Phone { get; set; }

	public string? Email { get; set; }

	public string Sex { get; set; } = null!;

	public string Login { get; set; } = null!;

	public string Password { get; set; } = null!;

	public byte[]? Image { get; set; }

	public string? Address { get; set; }

	public static implicit operator Patient(PatientDTO dto) => new()
	{
		Id = dto.Id,
		Surname = dto.Surname,
		Name = dto.Name,
		Patronym = dto.Patronym,
		Birthday = dto.Birthday,
		Passport = dto.Passport,
		Phone = dto.Phone,
		Email = dto.Email,
		Sex = dto.Sex,
		Login = dto.Login,
		Password = dto.Password,
		Image = dto.Image,
		Address = dto.Address,
	};

	public static implicit operator PatientDTO(Patient patient) => new()
	{
		Id = patient.Id,
		Surname = patient.Surname,
		Name = patient.Name,
		Patronym = patient.Patronym,
		Birthday = patient.Birthday,
		Passport = patient.Passport,
		Phone = patient.Phone,
		Email = patient.Email,
		Sex = patient.Sex,
		Login = patient.Login,
		Password = patient.Password,
		Image = patient.Image,
		Address = patient.Address,
	};
}