namespace ZR.Model.System.Model.Dto
{
    /// <summary>
    /// 文件分组查询对象
    /// </summary>
    public class SysFileGroupQueryDto : PagerInfo
    {
    }

    /// <summary>
    /// 文件分组输入输出对象
    /// </summary>
    public class SysFileGroupDto
    {
        [Required(ErrorMessage = "id不能为空")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        public string GroupName { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "排序不能为空")]
        public int Sort { get; set; }

        public int? IsDelete { get; set; }

        public DateTime? AddTime { get; set; }



        [ExcelColumn(Name = "是否删除")]
        public string IsDeleteLabel { get; set; }
    }
}