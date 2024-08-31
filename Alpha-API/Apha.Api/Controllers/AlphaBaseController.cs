using Alpha.Api.ViewModels;
using Alpha.Framework.MediatR.Api.Authorizations;
using Alpha.Framework.MediatR.Api.Controllers;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Alpha.Domain.Commands;
using System;
using System.Collections.Generic;

namespace Alpha.Api.Controllers
{
    [Route("")]
    [ApiController]
    [CustomAuthorization]
    public class AlphaBaseController : BaseController
    {
        public readonly IMapper _mapper;

        public AlphaBaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IActionResult CustomResponse<T>(ICommandResult<T> response, IViewModel viewmodel)
        {
            if (response.IsSuccess)
                return Ok(new ApiResult(true, "Success", viewmodel));
            else
                return BadRequest(new ApiResult(false, "Fail", response.Notifications));
        }

        protected IActionResult CustomResponse<T, V>(ICommandResult<T> response)
        {
            try
            {
                if (response.IsSuccess)
                {
                    var viewmodel = _mapper.Map<V>(response.Data);
                    return Ok(new ApiResult(true, "Success", viewmodel));
                }
                if (!response.IsSuccess && response.Data != null)
                {
                    var viewmodel = _mapper.Map<V>(response.Data);
                    return BadRequest(new ApiResult(false, "Fail", viewmodel, response.Notifications));
                }
                else
                    return BadRequest(new ApiResult(false, "Fail", response.Notifications));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult(false, "Fail", ex.Message));
            }
        }

        protected IActionResult CustomResponse<T, V>(T response)
        {
            try
            {
                if (response != null)
                {
                    var viewmodel = _mapper.Map<V>(response);
                    return Ok(new ApiResult(true, "Success", viewmodel));
                }
                else
                    return Ok(new ApiResult(false, "Fail", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult(false, "Fail", null));
            }
        }

        protected IActionResult CustomResponse<T, V>(List<T> response)
        {
            if (response != null)
            {
                var viewmodel = _mapper.Map<V>(response);
                return Ok(new ApiResult(true, "Success", viewmodel));
            }
            else
                return BadRequest(new ApiResult(false, "Fail", null));
        }

        protected IActionResult CustomResponse<T>(ICommandResult<T> response)
        {
            if (response.IsSuccess)
                return Ok(new ApiResult(true, "Success", new { }));
            else
                return BadRequest(new ApiResult(false, "Fail", response.Notifications));
        }

        protected IActionResult CustomCommandResponse<T>(ICommandResult<T> response)
        {
            if (response.IsSuccess)
                return Ok(new ApiResult(true, "Success", response.Data));
            else
                return BadRequest(new ApiResult(false, "Fail", response.Notifications));
        }

        protected IActionResult CustomQueryResponse<T>(T viewmodel)
        {
            return Ok(new ApiResult(true, "Success", viewmodel));
        }

        protected IActionResult CustomQueryResponse<T, V>(T response)
        {
            if (response != null)
            {
                var viewmodel = _mapper.Map<V>(response);
                return Ok(new ApiResult(true, "Success", viewmodel));
            }
            else
                return BadRequest(new ApiResult(false, "Fail", null));
        }

        protected IActionResult CustomQueryResponse<V>(PagedSearchResult pagedSearchResult)
        {
            if (pagedSearchResult != null)
            {
                var viewmodel = _mapper.Map<V>(pagedSearchResult.SearchResult);

                var result = new PagedSearchViewResult
                {
                    SearchResult = viewmodel,
                    PageSize = pagedSearchResult.PageSize,
                    PageCount = pagedSearchResult.PageCount,
                    PageIndex = pagedSearchResult.PageIndex,
                    TotalRecords = pagedSearchResult.TotalRecords
                };

                return Ok(new ApiResult(true, "Success", result));
            }
            else
                return BadRequest(new ApiResult(false, "Fail", null));
        }

        protected IActionResult CustomPageResponse<T>(T viewmodel, PagedSearchResult pagedSearchResult)
        {
            var result = new PagedSearchViewResult
            {
                SearchResult = viewmodel,
                PageSize = pagedSearchResult.PageSize,
                PageCount = pagedSearchResult.PageCount,
                PageIndex = pagedSearchResult.PageIndex,
                TotalRecords = pagedSearchResult.TotalRecords
            };

            return Ok(new ApiResult(true, "Success", result));
        }

        protected IActionResult CustomPageResponse<V, T>(V viewmodel, PagedResult<T> pagedSearchResult) where T : class
        {
            var result = new PagedSearchViewResult
            {
                SearchResult = viewmodel,
                PageSize = pagedSearchResult.PageSize,
                PageCount = pagedSearchResult.PageCount,
                PageIndex = pagedSearchResult.PageIndex,
                TotalRecords = pagedSearchResult.TotalRecords
            };

            return Ok(new ApiResult(true, "Success", result));
        }

        protected IActionResult CustomPageResponse<V>(PagedSearchResult pagedSearchResult)
        {
            try
            {
                if (pagedSearchResult != null)
                {
                    var viewmodel = _mapper.Map<V>(pagedSearchResult.SearchResult);

                    var result = new PagedSearchViewResult
                    {
                        SearchResult = viewmodel,
                        PageSize = pagedSearchResult.PageSize,
                        PageCount = pagedSearchResult.PageCount,
                        PageIndex = pagedSearchResult.PageIndex,
                        TotalRecords = pagedSearchResult.TotalRecords
                    };

                    return Ok(new ApiResult(true, "Success", result));
                }
                else
                    return BadRequest(new ApiResult(false, "Fail", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult(false, "Fail", null));
            }
        }

        protected IActionResult CustomPageResponse<V, T>(PagedResult<T> pagedResult) where T : class
        {
            try
            {
                if (pagedResult != null)
                {
                    var viewmodel = _mapper.Map<V>(pagedResult.SearchResult);

                    var result = new PagedSearchViewResult
                    {
                        SearchResult = viewmodel,
                        PageSize = pagedResult.PageSize,
                        PageCount = pagedResult.PageCount,
                        PageIndex = pagedResult.PageIndex,
                        TotalRecords = pagedResult.TotalRecords
                    };

                    return Ok(new ApiResult(true, "Success", result));
                }
                else
                    return BadRequest(new ApiResult(false, "Fail", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult(false, "Fail", null));
            }
        }

        protected IActionResult CustomResponse(IViewModel viewmodel)
        {
            return Ok(new ApiResult(true, "Success", viewmodel));
        }

        protected IActionResult CustomResponse<T>(List<T> viewmodel)
        {
            return Ok(new ApiResult(true, "Success", viewmodel));
        }

        protected IActionResult CustomResponse(string response)
        {
            return Ok(new ApiResult(true, "Success", response));
        }

        protected IActionResult CustomResponse(bool sucesso)
        {
            if (sucesso) return Ok(new ApiResult(true, "Success", new { }));
            else return BadRequest(new ApiResult(false, "Fail", null));
        }

        protected IActionResult CustomFailResponse()
        {
            return BadRequest(new ApiResult(false, "Fail", new { }));
        }

        protected IActionResult CustomFailResponse<T, V>(T data)
        {
            var viewmodel = _mapper.Map<V>(data);
            return BadRequest(new ApiResult(false, "Fail", viewmodel));
        }
    }
}
