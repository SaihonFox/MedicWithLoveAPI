using System;
using System.Collections.Generic;

using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class RequestStateDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

	public static implicit operator RequestState(RequestStateDTO dto) => new()
	{
		Id = dto.Id,
		Name = dto.Name
	};

	public static implicit operator RequestStateDTO(RequestState requestState) => new()
	{
		Id = requestState.Id,
		Name = requestState.Name
	};
}
