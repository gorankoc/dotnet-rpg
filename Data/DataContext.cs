using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace dotnet_rpg.Data {
	public class DataContext : DbContext {
		public DbSet<Character> Characters { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Weapon> Weapons { get; set; }
		public DbSet<Skill> Skills { get; set; }
		public DbSet<CharacterSkill> CharacterSkills { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// configures one-to-many relationship
			/*modelBuilder.Entity<Grades>()
						.HasMany(g => g.Students)
						.WithOne(s => s.Grade)
						.HasForeignKey(s => s.GradeId);*/

			//defines the shape of your entity, relationships between them
			//and how they map to DB
			modelBuilder.Entity<CharacterSkill>()
				.HasKey(cs => new { cs.CharacterId, cs.SkillId });
		}
	}
}