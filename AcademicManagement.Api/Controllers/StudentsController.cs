using AcademicManagement.Api.UseCases.Enrollments;
using AcademicManagement.Api.UseCases.Students;
using AcademicManagement.Communication.Request;
using AcademicManagement.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace AcademicManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly RegisterStudentsUseCase _registerStudentsUseCase;
        private readonly GetStudentsUseCase _getStudentsUseCase;
        private readonly UpdateStudentUseCase _updateStudentUseCase;
        private readonly DeleteStudentUseCase _deleteStudentUseCase;

        private readonly EnrollStudentInCoursesUseCase _enrollStudentInCoursesUseCase;
        private readonly DeleteEnrollmentUseCase _deleteEnrollmentUseCase;
        private readonly GetEnrolledStudentsUseCase _getEnrolledStudentsUseCase;

        
        public StudentsController(
            RegisterStudentsUseCase registerStudentsUseCase,
            GetStudentsUseCase getStudentsUseCase,
            UpdateStudentUseCase updateStudentUseCase,
            DeleteStudentUseCase deleteStudentUseCase, 
            EnrollStudentInCoursesUseCase enrollStudentInCoursesUseCase,
            DeleteEnrollmentUseCase deleteEnrollmentUseCas,
            GetEnrolledStudentsUseCase getEnrolledStudentsUseCase)
        {
            _registerStudentsUseCase = registerStudentsUseCase ?? throw new ArgumentNullException(nameof(registerStudentsUseCase));
            _getStudentsUseCase = getStudentsUseCase ?? throw new ArgumentNullException(nameof(getStudentsUseCase));
            _updateStudentUseCase = updateStudentUseCase ?? throw new ArgumentNullException(nameof(updateStudentUseCase));
            _deleteStudentUseCase = deleteStudentUseCase ?? throw new ArgumentNullException(nameof(deleteStudentUseCase));
            _enrollStudentInCoursesUseCase = enrollStudentInCoursesUseCase ?? throw new ArgumentNullException(nameof(enrollStudentInCoursesUseCase));
            _deleteEnrollmentUseCase = deleteEnrollmentUseCas ?? throw new ArgumentNullException(nameof(deleteEnrollmentUseCas));
            _getEnrolledStudentsUseCase = getEnrolledStudentsUseCase ?? throw new ArgumentNullException(nameof(getEnrolledStudentsUseCase));
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterStudent(RequestStudentJson request)
        {
            var response = await _registerStudentsUseCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudents()
        {
            var response = await _getStudentsUseCase.Execute();
            return Ok(response);
        }

        [HttpPatch("{studentId}")]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(Guid studentId, [FromBody] RequestUpdateStudentJson request)
        {
            var response = await _updateStudentUseCase.Execute(studentId, request);
            return Ok(response);
        }

        [HttpDelete()]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            await _deleteStudentUseCase.Execute(studentId);
            return NoContent();
        }

        [HttpPost("/enrollments")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnrollStudentInCourse(Guid studentId, RequestEnrollStudentJson request)
        {
            request.StudentId = studentId;
            await _enrollStudentInCoursesUseCase.Execute(request);
            return NoContent(); 
        }

        [HttpGet("/enrolledStudents")]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEnrolledStudents()
        {
            var response = await _getEnrolledStudentsUseCase.Execute();
            return Ok(response);
        }

        [HttpDelete("/enrollments")]
        [ProducesResponseType(typeof(ResponseStudentJson), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEnrollment(Guid studentId, Guid courseId)
        {
            await _deleteEnrollmentUseCase.Execute(studentId, courseId);
            return NoContent();
        }
    }
}