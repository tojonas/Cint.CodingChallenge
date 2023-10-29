using Cint.CodingChallenge.Core.Interfaces;
using Cint.CodingChallenge.Model;
using Cint.CodingChallenge.Web.Extensions;
using Cint.CodingChallenge.Web.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Htmx;

namespace Cint.CodingChallenge.Web.Controllers
{
    [Route("[controller]")]
    [Consumes("application/json", "application/x-www-form-urlencoded")]
    public class SurveyController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository<Survey, Guid> _repository;

        public SurveyController(IRepository<Survey, Guid> repository, ILogger<SurveyController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> Search(string? name)
        {
            var surveys = _repository.GetAsync(s => string.IsNullOrWhiteSpace(name) || s.Name.ToLower().Contains(name.ToLower()));

            var results = await surveys
                .OrderByDescending(s => s.IncentiveEuros / s.LengthMinutes)
                .Select(s => s.ToView())
                .ToArrayAsync();                // Not enough time to change tests from [] to IAsyncEnumerable

            if (!Request.IsHtmx() && !Request.IsHtmlRequest())
            {
                return Ok(results);
            }

            if (!Request.IsHtmx())
            {
                return View("Index", results);
            }

            return PartialView("_Surveys", results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var survey = await _repository.GetByIdAsync(id);
            if (survey == null)
            {
                _logger.LogWarning($"Survey with id {id} not found");
                return NotFound();
            }
            if (!Request.IsHtmx())
            {
                return Ok(survey.ToView());
            }
            ViewBag.Title = "View Survey";
            ViewBag.ViewName = "_NewSurvey";
            return PartialView("_NewSurvey");
        }

        [HttpGet, Route("")]
        public IActionResult Create()
        {
            ViewBag.Title = "Create Survey";
            ViewBag.ViewName = "_NewSurvey";
            return PartialView("_Modal");
        }

        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] SurveyCreateViewModel surveyCreateViewModel)
        {
            return await CreateImpl(surveyCreateViewModel);
        }

        //https://github.com/dotnet/aspnetcore/issues/14369
        //https://github.com/bigskysoftware/htmx/issues/210
        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CreateFromForm([FromForm] SurveyCreateViewModel surveyCreateViewModel)
        {
            return await CreateImpl(surveyCreateViewModel);
        }

        private async Task<IActionResult> CreateImpl(SurveyCreateViewModel surveyCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                // We shouldn't arrive here, but if it does, log it
                _logger.LogError("Invalid model state in controller");
                return BadRequest();
            }
            // TODO: Let database create Id
            var survey = surveyCreateViewModel.ToModel(Guid.NewGuid());
            await _repository.AddAsync(survey);
            await _repository.SaveChangesAsync();

            if (!Request.IsHtmx())
            {
                return Created($"survey/{survey.Id}", survey.ToView());
            }
            return PartialView("_Empty");
        }
    }
}
