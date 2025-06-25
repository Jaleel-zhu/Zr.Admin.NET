using ZR.Mall.Model;
using ZR.Mall.Model.Dto;

namespace ZR.Mall.Service.IService
{
    /// <summary>
    /// 规格模板service接口
    /// </summary>
    public interface ISpecTemplateService : IBaseService<SpecTemplate>
    {
        PagedInfo<SpecTemplateDto> GetList(SpecTemplateQueryDto parm);

        SpecTemplate GetInfo(long Id);

        SpecTemplate AddMmsSpecTemplate(SpecTemplate parm);
        int UpdateMmsSpecTemplate(SpecTemplate parm);
    }
}
