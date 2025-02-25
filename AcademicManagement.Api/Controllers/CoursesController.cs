using AcademicManagement.Api.UseCases.Courses;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace AcademicManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CreateCourseUseCase _createCourseUseCase;
        private readonly GetCoursesUseCase _getCoursesUseCase;
        private readonly UpdateCourseUseCase _updateCourseUseCase;
        private readonly DeleteCourseUseCase _deleteCourseUseCase; 
        private readonly GetCourseStudentsUseCase _getCourseStudentsUseCase; 

        public CoursesController(
           CreateCourseUseCase createCourseUseCase,
           GetCoursesUseCase getCoursesUseCase,
           UpdateCourseUseCase updateCourseUseCase,
           DeleteCourseUseCase deleteCourseUseCase,
           GetCourseStudentsUseCase getCourseStudentsUseCase)
        {
            _createCourseUseCase = createCourseUseCase ?? throw new ArgumentNullException(nameof(createCourseUseCase));
            _getCoursesUseCase = getCoursesUseCase ?? throw new ArgumentNullException(nameof(getCoursesUseCase));
            _updateCourseUseCase = updateCourseUseCase ?? throw new ArgumentNullException(nameof(updateCourseUseCase));
            _deleteCourseUseCase = deleteCourseUseCase ?? throw new ArgumentNullException(nameof(deleteCourseUseCase));
            _getCourseStudentsUseCase = getCourseStudentsUseCase ?? throw new ArgumentNullException(nameof(getCourseStudentsUseCase));
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse(RequestCourseJson request)
        {
            var response = await _createCourseUseCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourses()
        {
            var response = await _getCoursesUseCase.Execute();

            return Ok(response);
        }

        [HttpPatch()]
        [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourses(Guid courseId, RequestCourseJson request)
        {
            var response = await _updateCourseUseCase.Execute(courseId, request);

            return Created(string.Empty, response);
        }

        [HttpDelete("/courses")]
        [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
             await _deleteCourseUseCase.Execute(courseId);

            return NoContent();
        }

        [HttpGet("/coursesStudents")]
        [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCoursesStudents(Guid courseId)
        {
            var response = await _getCourseStudentsUseCase.Execute(courseId);

            return Ok(response);
        }
    }
}
