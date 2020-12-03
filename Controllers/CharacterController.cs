using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using System.Collections.Generic;

namespace dotnet_rpg.Controllers
{

	[ApiController]                                                                             // ApiController attribute type also derived types is use to serve http Api responses, enables few feautres, Routeing, automatic http 400 responses when making web service call, defines how is going to be able to
	[Route("[controller]")]                                                                     // find this specific controller string "[controller]" -> means that we can acces by using class name without Controller suffix derive from ControlerBase ( ctrl without view support ) since building api only, for view = Controller
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			// 1. ctor
			// 2. create and assing field ctrl + .
			// 3. delete this ... add _ meaning field
			// 4. POSTMAN TEST !!! 500 error ... documented at boottom of .cs

			_characterService = characterService; // _ means field name
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> Get()
		{
			// 5. replace previouse body of non DTO model Get() with service methods
			return Ok(await _characterService.GetAllCharacters());                                  // Ok = 200, BadRequest = 400, NotFound = 404 method
		}

		[HttpGet("{id}")] 
		public async Task<IActionResult> GetSingle(int id)                                          // GetSingle() works but webapi doesnt know wich Get meth to use. add attr [Route("GetAll")] instead of upper Get(); req.method
		{
			return Ok(await _characterService.GetCharacterByID(id));
		}

		[HttpPost]
		public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
		{
			return Ok(await _characterService.AddCharacter(newCharacter));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
		{
			ServiceResponse<GetCharacterDto> response = await _characterService.UpdateCharacter(updatedCharacter);

			if (response.Data == null){
				return NotFound(response);
			}
			return Ok(await _characterService.UpdateCharacter(updatedCharacter));
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = await _characterService.DeleteCharacter(id);
            if (response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
	} 
}

/*


--------------------
InitialGitCommit.txt 
--------------------



I. HTTP METHODS Intro Definition - terminology 


The POST method is used to sumbit an entity to the specific resource, 
often causeing change in state or side effects on the server.


The PUT method replaces all current representations of the target resource
with the request payload. Update of complete obj.

The DELETE method del the specified resource.


-------------------------------------------------------------------------------------------------


2. CONTROLLER CODE BEFORE DTO SERVICE!


	*** desc in InitialGitCommit.txt

		[HttpGet("GetAll")]
		public IActionResult Get(){									// No attribute, we could bhave added [HttpGet] webapi suppoorts nameing conventions and if the name of the method is named Get HTTP method is also, web service knows exactly what Get method is requested  because its only one in the Controller !
			return Ok(characters); 									// Ok = 200, BadRequest = 400, NotFound = 404 method
		}

	

-------------------------------------------------------------------------------------------------

3. DTO AND DI - ICharacterService 


	a) DEFINITION

Currently we are doing all logic of our web service calls in the controller,
if app is growing, seperate work into different classes, or if u need to do 
the same work over and over again, we dont need to copy paste into diff
controllers.

Thats where services comes in, controller should be simple and forward data
to the service and return result to the client.

To do be able to do that we will inject the neccesary services into controller,
we will use DI. Thats how we can use same service in different controllers, 
and if we want to change the implementation of the service, we just change 
one service class dont need to touch controller.

Client 	-> Controller 	-> Service 	-> DB
Client	<-				<-			<- DB

Data Transfer Object (DTO) comunication between client and server, they are
not find in db, they wont be mapped.

Example ( later in DB we will have exactly same properties or fields ), lets say
we add field DateCreated or IsDeleted ) as information that user does not need to see,
in this case ... we want to save the information into DB but dont want it send it back
to client.

	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = "Frodo";
		public int HitPoints { get; set; } = 100;
		public int Strenght { get; set; } = 10;
		public int Defence { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RpgClass Class { get; set; } = RpgClass.Knight;
		public bool IsDeleted { get; set; } = false;
		public Date DateCreated { get; set; }
	}

Here DTO comes in, grab the model and map information of the model to the DTO, libraries
like "automapper". We can create DTOs that combines properties of several models.

	public class GetCharacterDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = "Frodo";
		public int HitPoints { get; set; } = 100;
		public int Strenght { get; set; } = 10;
		public int Defence { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RpgClass Class { get; set; } = RpgClass.Knight;		
		public Date DateCreated { get; set; }
	}


Its not only about returning data to the client, we can use it at creating data.
Object with certain info that client sends to service, then service grabs that info
and maps to actual model.  

	public class AddCharacterDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = "Frodo";
		public int HitPoints { get; set; } = 100;
		public int Strenght { get; set; } = 10;
		public int Defence { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RpgClass Class { get; set; } = RpgClass.Knight;				
	}



-------------------------------------------------------------------------------------------------
 

4. STARTUP.CS

TEST (Postman) 500 error Body return:

		 System.InvalidOperationException: Unable to resolve service for type 
		'dotnet_rpg.Services.CharacterService.ICharacterService' 
		 while attempting to activate 'dotnet_rpg.Controllers.CharacterController'.

The webapi wants to inject the ICharacterService but it doesnt know wich
implementation class it should use

		 services.AddScoped<ICharacterService, CharacterService>(); in Startup.cs

Now the webapi knows that it has to use the CharacterService implementation when ever
the controller wants to INJECT the ICharacterService 
		
Beauty of that is when ever we want to use another implementation class for instance
we just change that line. We dont have to change the anything in the  controllers injecting the ICharacterService
With AddScoped we create a new instance of the requested service for every request that comes in !
		
Also there are AddTransient<>(); - provides a new instance to every controller and to every service
even within the same request, and AddSingleton<>(); creates only one instance that is used for every request.


-------------------------------------------------------------------------------------------------

*/

