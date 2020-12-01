using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
         Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
		 Task<ServiceResponse<GetCharacterDto>> GetCharacterByID(int id);
		 Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
		 Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
         Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    }
}



/*

5) THREADS - ASYNC CALLS ICharacterService, CharacterService
 

In synchronous call you would give a task to a thread like fethich data from a db,
and the thread waits for this task to be finished and woudnt do anything else until
the task is done.

With async call this thread woudnt wait at all. Instead it would be open for new tasks and
as soon as the other task for ex.fetching data from db is done it would grab the data and
return it. 

In synchronous call and small application we have no db calls that take lots of time its in ms. 
Additionally its very likely that u have 1 or more thread available. So even if 1 thread 
is waiting for a task, another thread can do another task, 

But in large apps with lots of users it can rly happen that all threads are busy. 
In that cas app WONT RESPOND to a request anymore ... leads to hell

It doesnt hurt to implement early in project. Although the methods in the character service
wont do anything asynchronouse they will later on on fetching data from db.

So add using System.Threading.Tasks type to our return types: 

		in ICharacterService add Task<> type:
        Task<List<Character>> GetAllCharacters();
		Task<Character> GetCharacterByID(int id);

		in CharacterService

		public async Task<List<Character>> GetAllCharacters(){
			return characters;
		}

The code will still be executed synchonosly but when we add EF with db queries later on 
we will have async calls.

Again in CONTROLLER:

we add the task type with the corresponding using directive using System.Threading.Tasks
and the async keyword to every method 

		[HttpPost]
		public async Task<IActionResult> AddCharacter(Character newCharacter){
			 return Ok(await _characterService.AddCharacter(newCharacter));
		}

additionally we add AWAIT keyword to the actual service call.

*/