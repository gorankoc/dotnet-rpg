namespace dotnet_rpg.Models
{
    public class ServiceResponse<T>
    {
		public T Data { get; set; }
		public bool Success { get; set; } = true;
		public string Message { get; set; } = null;
    }
}

/*

6) WRAPPER OBJ - ServiceResponse

Practice to return wrapper obj in pro projects to the client with every service call. 
Advantages are that u can add additional info to the returning result lik a success
or exception.

before in ICharacterService 

         Task<List<Character>> GetAllCharacters();
		 Task<Character> GetCharacterByID(int id);
		 Task<List<Character>> AddCharacter(Character newCharacter);


The frontend is able to react to these addition information and read the actual data
with the help of HDP GDP interceptors for instance and we can make use of generics.
to use the correct types.

T is actual type of the DATA we want to send back. 

Similiar to sync impl. its not so usefull now but it comes very handy in bigger proj.
Catching exceptions. 

We have to modify the return types of 
character service and a character service methods:

ICharacterService

	from 

         Task<List<Character>> GetAllCharacters();
	to
         Task<ServiceResponse<List<Character>>> GetAllCharacters();


CharacterService:

	from 
		public Task<List<Character>> AddCharacter(Character newCharacter)
		{
			characters.Add(newCharacter);
			return characters;
		}

	to
		public async Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)
		{
			ServiceResponse<List<Character>> serviceResponse = new ServiceResponse<List<Character>>();			
			characters.Add(newCharacter);
			serviceResponse.Data = characters;
			return serviceResponse;
		}

and
		public async Task<ServiceResponse<Character>> GetCharacterByID(int id)
		{
			ServiceResponse<Character> serviceResponse = new ServiceResponse<Character>();
			serviceResponse.Data = characters.FirstOrDefault(c => c.Id == id);

			return serviceResponse;
		}
*/