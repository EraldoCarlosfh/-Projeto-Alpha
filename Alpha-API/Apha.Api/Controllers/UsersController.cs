using Alpha.Api.ViewModels.Users;
using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Responses.Users;
using Alpha.Framework.MediatR.Api.Authorizations;
using Alpha.Framework.MediatR.Api.Controllers;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Integrations.Encryption;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Alpha.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [CustomAuthorization]
    public class UsersController : AlphaBaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserQuery _userQuery;
        private readonly IEncryptionService _encryptionService;
        private readonly AuthenticatedUserModel _authenticatedUserModel;

        public UsersController(
            IMediator mediator,
            IMapper mapper,
            IUserQuery userQuery,
            IEncryptionService encryptionService,
            AuthenticatedUserModel authenticatedUserModel) : base(mapper)
        {
            _mediator = mediator;
            _userQuery = userQuery;
            _encryptionService = encryptionService;
            _authenticatedUserModel = authenticatedUserModel;
        }

        [HttpGet]
        [Route("{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserByUserId(string userId)
        {
            var response = await _userQuery.GetById(userId.ToEntityId());
            if (response == null) return NotFound($"O usuário com o id {userId} não foi encontrado");

            var viewModel = _mapper.Map<UserViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpGet]
        [Route("check-email/{email}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckEmailIfExists(string email)
        {
            var emailDecrypt = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(email));
            var response = await _userQuery.GetUserByEmail(emailDecrypt);
            return response == null ? Ok(false) : Ok(true);
        }

        [HttpGet]
        [Route("check-cellphone/{cellphone}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckCellPhoneIfExists(string cellphone)
        {
            var response = await _userQuery.GetUserByCellPhone(cellphone);
            return response == null ? Ok(false) : Ok(true);
        }

        [HttpGet]
        [Route("check-document/{document}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckDocumentIfExists(string document)
        {
            var response = await _userQuery.GetUserByDocumentNumber(document);
            return response == null ? Ok(false) : Ok(true);
        }

        [HttpGet]
        [Route("{emailOrCellPhone}/documentNumber")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserDocumentNumberByEmailOrCellPhone(string emailOrCellPhone)
        {
            var response = await _userQuery.GetUserByEmailOrCellPhone(emailOrCellPhone);

            if (response == null) return NotFound($"O usuário com o contato {emailOrCellPhone} não foi encontrado");

            var decryptedDocumentNumber = _encryptionService.Decrypt(response.EncryptedDocumentNumber);

            return Ok(new ApiResult(true, "Success", decryptedDocumentNumber));
        }

        [HttpGet]
        [Route("emailOrDocumentNumber/{emailOrDocumentNumber}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserByEmailOrDocumentNumber(string emailOrDocumentNumber)
        {
            var response = await _userQuery.GetUserByEmailOrDocumentNumber(emailOrDocumentNumber);

            if (response == null) return NotFound($"O usuário associado a {emailOrDocumentNumber} não foi encontrado");

            return CustomResponse<User, UserViewModel>(response);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<UserViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetActiveUsers()
        {
            var response = await _userQuery.GetActives();
            if (response == null) return NotFound();

            return CustomResponse<List<User>, List<UserViewModel>>(response);
        }
       
        [HttpPost]
        [Route("new-user")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateNewUser(CreateNewUserCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse<User, UserViewModel>(response);
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LoginUser(LoginUserCommand request)
        {
            var response = await _mediator.Send(request);

            if (response == null)
                return Unauthorized("Usuário ou senha incorretos");

            return CustomResponse<LoginUserResponse, LoginResponseViewModel>(response);
        }

        [HttpPost]
        [Route("login/phone/generate-code")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LoginPhoneGenerateCode(LoginPhoneGenerateCodeCommand request)
        {
            var response = await _mediator.Send(request);

            if (response == null)
                return Unauthorized("Usuário não encontrado");

            return CustomResponse<LoginPhoneGenerateCodeResponse, LoginPhoneGenerateCodeResponseViewModel>(response);
        }      

        [HttpPut]
        [Route("update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse(response);
        } 

        [HttpPut]
        [Route("activation")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActivateUser(ActivateUserCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse<LoginUserResponse, LoginResponseViewModel>(response);
        }

        [HttpPut]
        [Route("new-terms-acceptance")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActivateUser(AcceptNewUserTermsCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse<LoginUserResponse, LoginResponseViewModel>(response);
        }  

        [HttpGet]
        [Route("decryption/{userId}/document-number")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserDocumentNumner(string userId)
        {
            var response = await _userQuery.GetById(userId.ToEntityId());
            if (response == null) return NotFound($"O usuário com o id {userId} não foi encontrado");

            var documentnumber = _encryptionService.Decrypt(response.EncryptedDocumentNumber);

            return Ok(new ApiResult(true, "Success", documentnumber));
        }       
    }
}
