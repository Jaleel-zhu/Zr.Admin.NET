using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

namespace ZR.Mall.Service
{
    /// <summary>
    /// 规格模板Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ISpecTemplateService), ServiceLifetime = LifeTime.Transient)]
    public class SpecTemplateService : BaseService<SpecTemplate>, ISpecTemplateService
    {
        /// <summary>
        /// 查询规格模板列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<SpecTemplateDto> GetList(SpecTemplateQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<SpecTemplate, SpecTemplateDto>(parm);

            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SpecTemplate GetInfo(long Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加规格模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SpecTemplate AddMmsSpecTemplate(SpecTemplate model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改规格模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMmsSpecTemplate(SpecTemplate model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<SpecTemplate> QueryExp(SpecTemplateQueryDto parm)
        {
            var predicate = Expressionable.Create<SpecTemplate>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TemplateName), it => it.TemplateName.Contains(parm.TemplateName));
            return predicate;
        }
    }
}