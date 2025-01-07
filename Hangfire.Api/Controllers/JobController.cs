using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public void ListInt()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);
            }
        }

        [HttpPost]
        [Route("CreateBackgroundJob")]
        public IActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue(() => ListInt());
            return Ok();
        }

        [HttpPost]
        [Route("CreateScheduledJob")]
        public IActionResult CreateScheduledJob()
        {
            var scheduleDateTime = DateTime.Now.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduleDateTime);

            BackgroundJob.Schedule(() => Console.WriteLine("Tarefa agendada"), dateTimeOffSet);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public IActionResult CreateContinuationJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduleDateTime);

            var job1 = BackgroundJob.Schedule(() => Console.WriteLine("Tarefa agendada"), dateTimeOffSet);
            var job2 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Segundo job"));
            var job3 = BackgroundJob.ContinueJobWith(job2, () => Console.WriteLine("Terceiro job"));

            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public IActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Tarefa recorrente"), "* * * * *");

            return Ok();
        }
    }
}
