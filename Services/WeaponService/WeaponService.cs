using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService {
	public class WeaponService : IWeaponService {
		
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;

		// parameter var click "initialize field from parameter ... "
		public WeaponService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) {
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_context = dataContext;
		}

		public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon) {
			
			ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
			
			try {
				Character character = await _context.Characters
				.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == 
				int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
				
				if (character == null) {
					response.Message = "Character not found";
					response.Success = false;
					return response;
				}

				Weapon weapon = new Weapon {
					Name = newWeapon.Name,
					Damage = newWeapon.Damage,
					Character = character
				};

				await _context.Weapons.AddAsync(weapon);
				await _context.SaveChangesAsync();
				response.Data = _mapper.Map<GetCharacterDto>(character);

			} catch(Exception ex) {
				if ( ex is DbUpdateException )
				{
					response.Message = "duplicat ključeva samo jedno oružje je dozvoljeno jednom Charu";
				}else{
					response.Message = ex.Message;
				}
				response.Success = false;
			}
			return response;
		}
	}
}