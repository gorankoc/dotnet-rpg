using System.Linq;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;

namespace dotnet_rpg{
    public class AutoMapperProfile : Profile{
		public AutoMapperProfile(){
        	/*CreateMap<Character, GetCharacterDto>()
			.ForMember(dto => dto.Skills, mapper => mapper.MapFrom(c => c.CharacterSkills
			.Select(cs => cs.Skill)));*/

			CreateMap<Character, GetCharacterDto>()
			.ForMember(dto => dto.Skills, mapper => mapper.MapFrom(c => c.CharacterSkills
			.Select(cs => cs.Skill)));


			// od određenog Charactera MapFrom ( trazi vezan)
			CreateMap<AddCharacterDto, Character>();
			CreateMap<Weapon, GetWeaponDto>();
			CreateMap<Skill, GetSkillDto>();
		}
    }
}