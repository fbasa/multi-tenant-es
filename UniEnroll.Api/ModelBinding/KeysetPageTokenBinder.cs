
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace UniEnroll.Api.ModelBinding;

/// <summary>Binds keyset paging tokens (next/prev) trimming whitespace and treating empty as null.</summary>
public sealed class KeysetPageTokenBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var name = bindingContext.FieldName;
        var val = bindingContext.ValueProvider.GetValue(name).FirstValue;
        bindingContext.Result = ModelBindingResult.Success(string.IsNullOrWhiteSpace(val) ? null : val.Trim());
        return Task.CompletedTask;
    }
}
