namespace dotnet_rpg.Models
{
    public class CharacterSkill
    {
		//additionally we need a primary key for this ENTITY
		//it will be a composite key of skill and character
		//class+id convention
		public int CharacterId { get; set; }
		public int SkillId { get; set; }
		
        public Character Character { get; set; }
		public Skill Skill { get; set; }
		//next FluentApi DataContext
    }
}