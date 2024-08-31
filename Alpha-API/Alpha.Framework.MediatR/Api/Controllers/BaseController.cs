using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using System.Linq;

namespace Alpha.Framework.MediatR.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult ReturnBadRequest()
        {
            if (ModelState.IsValid) return null;
            var messageSB = new StringBuilder();

            // Obter todas as mensagens de validação.
            var erroneousFields =
                ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Description = GetErrorsInline(x.Value.Errors)
                    });

            var itemIndex = 1;
            foreach (var item in erroneousFields)
            {
                // Obter nome do campo validado e remover o índice, se houver.
                var fieldName = item.Field;
                if (item.Field.Contains('.'))
                    fieldName = item.Field.Split('.')[1];

                // Índice (one-based) - nome do campo: mensagem de validação.
                messageSB.AppendLine($"{itemIndex} - {fieldName}: {item.Description}");
                itemIndex++;
            }

            return new BadRequestObjectResult(messageSB.ToString());
        }

        protected OkObjectResult ReturnPagedSearchViewResult(PagedSearchResult pagedSearchResult, object viewModel)
        {
            var result = new PagedSearchViewResult
            {
                SearchResult = viewModel,
                PageSize = pagedSearchResult.PageSize,
                PageCount = pagedSearchResult.PageCount,
                PageIndex = pagedSearchResult.PageIndex,
                TotalRecords = pagedSearchResult.TotalRecords
            };

            return Ok(result);
        }

        private static string GetErrorsInline(ModelErrorCollection errors)
        {
            var errorList = errors.Select(c => c.ErrorMessage).ToList();
            return string.Join(",", errorList);
        }
    }
}
