
namespace ZR.Mall.Model.Dto
{
    /// <summary>
    /// 规格模板查询对象
    /// </summary>
    public class SpecTemplateQueryDto : PagerInfo 
    {
        public string TemplateName { get; set; }
    }

    /// <summary>
    /// 规格模板输入输出对象
    /// </summary>
    public class SpecTemplateDto
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }

        public List<SpecDto> SpecJson { get; set; }
    }
}