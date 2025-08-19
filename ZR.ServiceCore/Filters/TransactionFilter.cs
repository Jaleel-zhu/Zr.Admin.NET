using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar.IOC;

namespace ZR.ServiceCore.Filters;

/// <summary>
/// 自动事务管理：在 Action 执行前自动开启数据库事务（业务代码与事务功能解耦）
/// 事务提交与回滚：
/// 当 Action 执行成功（无异常或异常已处理）时，自动提交事务
/// 当 Action 执行出现未处理异常时，自动回滚事务
/// 异常安全处理：捕获所有异常并确保事务回滚后重新抛出异常
/// </summary>
public class TransactionFilter : ActionFilterAttribute
{
    private readonly SqlSugarScope _db = DbScoped.SugarScope;

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            await _db.BeginTranAsync();

            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                await _db.CommitTranAsync();
            }
            else
            {
                await _db.RollbackTranAsync();
            }
        }
        catch (Exception)
        {
            await _db.RollbackTranAsync();
            throw;
        }
    }
}