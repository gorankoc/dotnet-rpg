using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private static readonly List<Character> characters = new List<Character>(){
			new Character(),
			new Character { Id = 1, Name = "Sam"}
		};
		
		public CharacterService(IMapper mapper){
			_mapper = mapper;
		}
		public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

			Character character = _mapper.Map<Character>(newCharacter);
			character.Id = characters.Max(c => c.Id) + 1;
			characters.Add(character);

			serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();

			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
		{
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			try{
				Character character = characters.First(c => c.Id == id);
				characters.Remove(character);

				serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
			}
			catch (Exception ex){
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
 
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByID(int id)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
			serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.Id == id));

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

			Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
			try{
				character.Name = updatedCharacter.Name;
				character.Class = updatedCharacter.Class;
				character.Defence = updatedCharacter.Defence;
				character.Intelligence = updatedCharacter.Intelligence;
				character.HitPoints = updatedCharacter.HitPoints;
				character.Strenght = updatedCharacter.Strenght;

				serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
			}
			catch (Exception ex){
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;

		}
	}
}