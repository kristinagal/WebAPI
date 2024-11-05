using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P100_ResponseTypes.Database;
using P100_ResponseTypes.Dto;
using P100_ResponseTypes.Services;

namespace P100_ResponseTypes.Controllers
{
    /*
         Kokie atsakymų tipai galimi .NET API kontroleryje?
        GERI:
         1. Ok (200 OK) Grąžina sėkmingą atsakymą kartu su duomenimis, kai užklausa įvykdyta be klaidų. 
            Paprastai naudojamas GET užklausose, kai norima grąžinti konkrečius duomenis.
         2. Created (201 Created) Naudojamas, kai sėkmingai sukuriamas naujas resursas (pvz., naujas objektas). 
            Šis atsakymas informuoja klientą, kad resursas buvo sėkmingai sukurtas ir pateikia jo buvimo vietą (location).
            Paprastai naudojamas POST užklausose, kai norima grąžinti naujo objekto ID arba kitą informaciją.
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item)
         3. NoContent (204 No Content) Nurodo, kad operacija buvo sėkminga, tačiau nėra jokių grąžinamų duomenų. 
            Dažniausiai naudojamas PUT arba DELETE užklausose, kai objektas sėkmingai atnaujintas arba ištrintas, 
            bet nereikia grąžinti jokios informacijos.
        
        BLOGI:
        4. BadRequest (400 Bad Request) Naudojamas, kai užklausa netinkama ar turi klaidų, 
            pavyzdžiui, trūksta būtinų duomenų arba jie neatitinka galiojančių reikalavimų. 
            Klientui grąžinama klaidos informacija, kurioje nurodomi neatitikimai ar klaidos priežastys.
        4a. ValidationProblem (400 Bad Request) Naudojamas, kai užklausa netinkama ar turi klaidų, susijusių su duomenų validacija. 
            Grąžina išsamesnę informaciją apie klaidas ir jų priežastis.
            Naudinga, kai atliekama daugiau nei viena validacija ir norima klientui pateikti visų nesėkmių sąrašą.
            Jis tinka, kai naudojate ModelState ir ASP.NET built-in validacijos mechanizmus.
            return ValidationProblem(ModelState)
        5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            Dažnai naudojamas kartu su autentifikacijos mechanizmais, pvz., „JWT“, kad apsaugotų API nuo neleistinų užklausų.
            return Unauthorized()
        6. Forbidden (403 Forbidden)  Reiškia, kad klientas neturi teisės prieiti prie resurso, net jei jis yra autentifikuotas. 
           Šis atsakymas dažnai naudojamas, kai tam tikri vartotojai ar vartotojų grupės neturi teisės atlikti konkrečių veiksmų.
           return Forbid()
        7. NotFound (404 Not Found) Naudojamas, kai ieškomas resursas nerandamas. 
           Dažnai naudojamas GET arba DELETE užklausose, kai resursas su nurodytu ID neegzistuoja arba buvo pašalintas.
           return NotFound()
        8. Conflict (409 Conflict) Indikuoja konfliktą, dažnai susijusį su resurso būsena. 
           Pvz., bandant pridėti jau egzistuojantį unikalų resursą, gali būti naudojamas „Conflict“ atsakymas,
           kad nurodytų neatitikimą ar konfliktą su esamais duomenimis.
        9. UnprocessableEntity (422 Unprocessable Entity) Naudojamas, kai serveris suprato užklausą, bet negali jos apdoroti dėl duomenų validacijos klaidų. 
           Šis atsakymas informuoja klientą, 
           kad duomenys yra sintaksiškai teisingi, bet neatitinka verslo logikos taisyklių. 
           return UnprocessableEntity("Netinkami duomenys.")
        
        LAAAAABAI BLOGI:
        10. InternalServerError (500 Internal Server Error) Naudojamas, kai API įvyksta nenumatyta klaida. 
           Šis atsakymas rodo serverio problemą ir leidžia klientui suprasti, kad klaida įvyko ne dėl jo užklausos.
           return StatusCode(500, "Vidinė serverio klaida")

        11. 418 I'm a teapot (418 I'm a teapot) - šis statusas naudojamas kaip juokas arba parodija, 
            kai serveris yra pasiruošęs priimti užklausas, bet yra suprogramuotas kaip arbatos puodas. 
            Tai yra HTTP standarto dalis, kuri buvo įtraukta kaip juokas. 
            return StatusCode(418, "I'm a teapot")
         */
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private readonly IFakeTodoDatabase _fakeTodoDatabase;
        private readonly ITodoMapper _todoMapper;
        public TodoController(IFakeTodoDatabase fakeTodoDatabase, ITodoMapper todoMapper)
        {
            _fakeTodoDatabase = fakeTodoDatabase;
            _todoMapper = todoMapper;
        }


        [HttpGet]
        public IActionResult Get([FromHeader] string userId)
        {
            //!7. NotFound (404 Not Found) kai prašomas objektų sąrašas nenaudojamas, nes tusčias sąrašas yra sėkmingas atsakymas

            // 5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }


            //Ką daryti jei DB nera irasu? Koks grąžinamas statusas? - Tiesiog Ok su tusciu masyvu.
            var todos = _fakeTodoDatabase.GetTodoItems();
            var dto = _todoMapper.Map(todos);
            return Ok(dto);
        }

        [HttpGet("filterByName")]
        public IActionResult Get([FromHeader] string userId, [FromQuery] string? name)
        {
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }

            var todos = _fakeTodoDatabase.GetTodoItems().Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            var dto = _todoMapper.Map(todos);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromHeader] string userId, int id)
        {
            // 5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }

            var todo = _fakeTodoDatabase.GetTodoItem(id);
            if (todo == null)
            {
                // 7. NotFound (404 Not Found) Šis atsakymas naudojamas, kai ieškomas objektas nerandamas. 
                return NotFound("Tokio įrašo nėra");
            }

            var dto = _todoMapper.Map(todo);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Post([FromHeader] string userId, [FromBody] TodoItemRequest request)
        {
            // 5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }

            // 4. BadRequest (400 Bad Request) Naudojamas, kai užklausa netinkama ar turi klaidų, 
            if (request == null)
            {
                return BadRequest("Nenurodyti duomenys");
            }
            // 4. BadRequest (400 Bad Request) jei nenurodytas Name
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Nenurodytas pavadinimas");
            }
            // 4. BadRequest (400 Bad Request) jei nenurodytas Content
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Nenurodytas turinys");
            }
            //9. UnprocessableEntity (422 Unprocessable Entity) Jei data yra praeityje
            if (request.EndDate < DateTime.Now)
            {
                return UnprocessableEntity("Netinkama data");
            }

            var todo = _todoMapper.Map(request);
            _fakeTodoDatabase.AddTodoItem(todo);

            // 2. Created (201 Created) Naudojamas, kai sėkmingai sukuriamas naujas resursas
            return Created(nameof(Get), new { id = todo.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromHeader] string userId, int id, [FromBody] TodoItemRequest request)
        {
            // 5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }

            var todo = _fakeTodoDatabase.GetTodoItem(id);
            if (todo == null)
            {
                // 7. NotFound (404 Not Found) Šis atsakymas naudojamas, kai ieškomas objektas nerandamas. 
                return NotFound("Tokio įrašo nėra");
            }

            // 4. BadRequest (400 Bad Request) Naudojamas, kai užklausa netinkama ar turi klaidų, 
            if (request == null)
            {
                return BadRequest("Nenurodyti duomenys");
            }
            // 4. BadRequest (400 Bad Request) jei nenurodytas Name
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Nenurodytas pavadinimas");
            }
            // 4. BadRequest (400 Bad Request) jei nenurodytas Content
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Nenurodytas turinys");
            }
            //9. UnprocessableEntity (422 Unprocessable Entity) Jei data yra praeityje
            if (request.EndDate < DateTime.Now)
            {
                return UnprocessableEntity("Netinkama data");
            }

            _todoMapper.Project(todo, request);
            _fakeTodoDatabase.UpdateTodoItem(todo);

            // 3. NoContent (204 No Content) Nurodo, kad operacija buvo sėkminga, tačiau nėra jokių grąžinamų duomenų. 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromHeader] string userId, int id)
        {
            // 5. Unauthorized (401 Unauthorized) Indikuoja, kad klientas nėra autentifikuotas. 
            if (!_fakeTodoDatabase.UserExists(userId))
            {
                return Unauthorized("Vartotojas neegzistuoja");
            }

            var todo = _fakeTodoDatabase.GetTodoItem(id);
            if (todo == null)
            {
                // 7. NotFound (404 Not Found) Šis atsakymas naudojamas, kai ieškomas objektas nerandamas. 
                return NotFound("Tokio įrašo nėra");
            }

            _fakeTodoDatabase.DeleteTodoItem(id);

            // 3. NoContent (204 No Content) Nurodo, kad operacija buvo sėkminga, tačiau nėra jokių grąžinamų duomenų. 
            return NoContent();
        }
    }
}