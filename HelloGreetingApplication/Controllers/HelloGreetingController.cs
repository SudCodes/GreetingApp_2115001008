using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private readonly ILogger<HelloGreetingController> _logger;
        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger)
        {
            _greetingBL = greetingBL;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Starting process of getting greeting");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                string greetingMsg = _greetingBL.GetGreetingBL();
                responceModel.Success = true;
                responceModel.Message = "Greeting Successful";
                responceModel.Data = greetingMsg;
                _logger.LogInformation("Greeting successfull");
                return Ok(responceModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting greeting {ex.Message}");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                responceModel.Success = false;
                responceModel.Message = "OOPS error occured";
                responceModel.Data = ex.Message;

                return BadRequest(responceModel);
            }


        }


        [HttpPost]
        public IActionResult Post([FromBody] UserRegistrationModel userRegistrationModel)
        {
            try
            {
                _logger.LogInformation("Starting post process of registering user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User Added successfully";
                responseModel.Data = $"First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
                _logger.LogInformation($"User added successfully Email : {userRegistrationModel.Email}");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "User registration process failed";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while registering user Error : {ex.Message}");
                return BadRequest(responseModel);
            }

        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRegistrationModel userRegistrationModel)
        {
            try
            {
                _logger.LogInformation($"Starting updating process for User with Email : {userRegistrationModel.Email}");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User updated successfully";
                responseModel.Data = $"Updated User - First Name : {userRegistrationModel.FirstName} Last Name : {userRegistrationModel.LastName} Email : {userRegistrationModel.Email}";
                _logger.LogInformation($"User Updated succesfully new user with email : {userRegistrationModel.Email}");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while updating user";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while updating user with Email : {userRegistrationModel.Email}");
                return BadRequest(responseModel);
            }

        }

        [HttpPatch]
        public IActionResult Patch(UpdateRequestModel updateRequestModel)
        {
            try
            {
                _logger.LogInformation("Starting process of updatin value for user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "FirstName updated successfully";
                responseModel.Data = $"Updated User - First Name : {updateRequestModel.FirstName}";
                _logger.LogInformation("value updated successfully");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while updating firstname";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while updating value Error : {ex.Message}");
                return BadRequest(responseModel);
            }


        }

        [HttpDelete]
        public IActionResult Delete(UserRegistrationModel userDeletion)

        {
            try
            {
                _logger.LogInformation("Starting prcess of deleting user");
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = true;
                responseModel.Message = "User Deleted successfully";
                responseModel.Data = $"User deleted successfully with email : {userDeletion.Email}";
                userDeletion.FirstName = string.Empty;
                userDeletion.LastName = string.Empty;
                userDeletion.Email = string.Empty;
                userDeletion.Password = string.Empty;
                _logger.LogInformation("User deleted successfuly");
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                ResponseModel<string> responseModel = new ResponseModel<string>();
                responseModel.Success = false;
                responseModel.Message = "error occured while deleting user";
                responseModel.Data = ex.Message;
                _logger.LogError($"Error occured while deleting user Error : {ex.Message}");
                return BadRequest(responseModel);
            }

        }

        [HttpPost]
        [Route("GetGreeting")]
        public IActionResult Greeting([FromBody] GreetingRequestModel greetingRequest)
        {
            try
            {
                _logger.LogInformation("Starting process of getting greeting");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                string greetingMsg = _greetingBL.GetGreetingBL(greetingRequest);
                responceModel.Success = true;
                responceModel.Message = "Greeting Successful";
                responceModel.Data = greetingMsg;
                _logger.LogInformation("Greeting successfull");
                return Ok(responceModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting greeting {ex.Message}");
                ResponseModel<string> responceModel = new ResponseModel<string>();
                responceModel.Success = false;
                responceModel.Message = "OOPS error occured";
                responceModel.Data = ex.Message;

                return BadRequest(responceModel);
            }
        }


        //UC4

        [HttpPost]
        [Route("save")]

        public IActionResult SaveGreeting([FromBody] GreetingModel greetingModel)
        {
            var result = _greetingBL.SaveGreetingBL(greetingModel);

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting Created",
                Data = result

            };
            return Created("Greeting Created", response);

        }

        [HttpGet("GetGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            var response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.GetGreetingByIdBL(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        [HttpGet("GetAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            ResponseModel<List<GreetingModel>> response = new ResponseModel<List<GreetingModel>>();
            try
            {
                var result = _greetingBL.GetAllGreetingsBL();
                if (result != null && result.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Greeting Messages Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "No Greeting Messages Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        //UC7
        [HttpPut("EditGreeting/{id}")]
        public IActionResult EditGreeting(int id, GreetingModel greetModel)
        {
            ResponseModel<GreetingModel> response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.EditGreetingBL(id, greetModel);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Updated Successfully";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }
    }
}
