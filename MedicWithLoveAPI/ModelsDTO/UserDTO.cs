using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class UserDTO
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronym { get; set; }

    public DateOnly Birthday { get; set; }

    public string? Passport { get; set; }

    public string? Phone { get; set; }

    public bool? IsBlocked { get; set; }

    public short? Post { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Image { get; set; }

	public static implicit operator User(UserDTO dto) => new()
	{
		Id = dto.Id,
		Surname = dto.Surname,
		Name = dto.Name,
		Patronym = dto.Patronym,
		Birthday = dto.Birthday,
		Passport = dto.Passport,
		Phone = dto.Phone,
		IsBlocked = dto.IsBlocked,
		Post = dto.Post,
		Login = dto.Login,
		Password = dto.Password,
		Image = dto.Image
	};

	public static implicit operator UserDTO(User user) => new()
	{
		Id = user.Id,
		Surname = user.Surname,
		Name = user.Name,
		Patronym = user.Patronym,
		Birthday = user.Birthday,
		Passport = user.Passport,
		Phone = user.Phone,
		IsBlocked = user.IsBlocked,
		Post = user.Post,
		Login = user.Login,
		Password = user.Password,
		Image = user.Image
	};
}
