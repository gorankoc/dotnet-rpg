> Udemy - .NET Core 3.1 Web API & Entity Framework Core Jumpstart
> Chapter II Web Api Core




# 1) HTTP METHODS Intro Definition

> **Terminology in CharacterController.cs**

  

# 2) CONTROLLER CODE BEFORE DTO SERVICE!

> **apiController**

Attribute type also derived types is use to serve http Api responses, enables few feautres, Routeing, automatic http 400 responses when making web service call, 

> **[Route("[controller]")]**

Find this specific controller string "[controller]" -> means that we can access by using class name without Controller suffix derive from ControlerBase ( ctrl without view support ) since building api only, for view = Controller. Data or json obj. is send via body request


	public class CharacterController: ControllerBase{
		private static readonly List<Character> characters = new List<Character>(){
			new Character(),
			new Character { Id = 1, Name = "Sam"}
		};
	
		[Route("GetAll")] http://localhost:5000/character/getall
		 
	    No attribute, we could bhave added [HttpGet] webapi suppoorts nameing conventions 
	    and if the name of the method is named Get HTTP method is also, web service knows 
	    exactly what Get method is requested because its only one in the Controller !
	    Ok = 200, BadRequest = 400, NotFound = 404 method 
	    */
	    
		[HttpGet("GetAll")]
		public IActionResult Get(){
	    
			return Ok(characters); 	
		}
		
	    [HttpGet("{id}")]
	    public IActionResult GetSingle(int id) 
		    // GetSingle() works but webapi doesnt know wich Get method to use. 
	    	// Add attr [Route("GetAll")] instead of upper Get() request method
		{
	    	return Ok(characters.FirstOrDefault(c => c.Id == id)); 
		}
	
		[HttpPost]
		public IActionResult AddCharacter(Character newCharacter){	
			characters.Add(newCharacter);    
		    return Ok(characters);

> **Postman - new tab Body - raw - json, write: **

    {
    	"id": 3,
    	"name": "Percival"
    }

  

# 3) DTO AND DI - ICharacterService 

>  a) DEFINITION 
>  b) DTO USE CASES 
>  c) change CharacterController for ICharacterService:

1. ctor

2. create and assing field ctrl + .

3. delete this ... add _ meaning field
4. POSTMAN TEST !!! 500 error ... documented at boottom of .cs

  

# 4) STARTUP.CS

> **TEST (Postman) 500 error** Body return:
> System.InvalidOperationException: Unable to resolve service for type dotnet_rpg.Services.CharacterService.ICharacterService' 
> while attempting to activate 'dotnet_rpg.Controllers.CharacterController'.


The **webapi** wants to inject the **ICharacterService** but it doesnt know wich implementation 
class it should use:

```c#
     services.AddScoped<ICharacterService, CharacterService>();
```


Now the webapi knows that it has to use the **CharacterService** implementation when ever
the controller wants to INJECT the **ICharacterService**.

**Beauty of that is when ever we want to use another implementation class for instance we just change that line.** 


We dont have to change the anything in the controllers injecting the **ICharacterService** With **AddScoped** we create a new instance of the requested service for every request that comes in !

Also there are *AddTransient*<>(); - provides a new instance to every controller and to every service even within the same request, and *AddSingleton*<>(); creates only one instance that is used for every request.



# 5) THREADS - ASYNC CALLS

- ICharacterService,
- CharacterService,
- CharacterController

  

# 6) ServiceResponse GENERICS


- ICharacterService,
- CharacterService