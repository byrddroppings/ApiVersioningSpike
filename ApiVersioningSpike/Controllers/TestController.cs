using Microsoft.AspNetCore.Mvc;

namespace ApiVersioningSpike.Controllers
{
    [ApiController]
    [Route("~/[controller]/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class TestController
    {
        private readonly IGreetingBuilder _greetingBuilder;

        public TestController(IGreetingBuilder greetingBuilder)
        {
            _greetingBuilder = greetingBuilder;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public string Greeting() => _greetingBuilder.Build();
    }
}